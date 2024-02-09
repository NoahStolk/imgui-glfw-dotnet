﻿using ImGuiGlfwDotnet.Extensions;
using ImGuiGlfwDotnet.Internals;
using ImGuiNET;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Runtime.InteropServices;

namespace ImGuiGlfwDotnet;

public sealed class ImGuiController
{
	private const string _vertexShader = """
		#version 330
		layout (location = 0) in vec2 position;
		layout (location = 1) in vec2 uv;
		layout (location = 2) in vec4 color;

		uniform mat4 projectionMatrix;

		out vec2 fragUv;
		out vec4 fragColor;

		void main()
		{
			fragUv = uv;
			fragColor = color;
			gl_Position = projectionMatrix * vec4(position.xy, 0, 1);
		}
		""";

	private const string _fragmentShader = """
		#version 330
		in vec2 fragUv;
		in vec4 fragColor;

		uniform sampler2D image;

		layout (location = 0) out vec4 outColor;

		void main()
		{
			outColor = fragColor * texture(image, fragUv.st);
		}
		""";

	private static readonly IReadOnlyList<Keys> _allKeys = (Keys[])Enum.GetValues(typeof(Keys));

	private readonly uint _vbo;
	private readonly uint _ebo;
	private uint _vao;

	private readonly GL _gl;
	private readonly IntPtr _context;
	private readonly uint _shaderId;
	private readonly int _projectionMatrixLocation;
	private readonly int _imageLocation;

	private int _windowWidth;
	private int _windowHeight;

	public ImGuiController(GL gl, int windowWidth, int windowHeight)
	{
		_gl = gl;
		_windowWidth = windowWidth;
		_windowHeight = windowHeight;

		_context = ImGui.CreateContext();
		ImGui.SetCurrentContext(_context);
		ImGui.StyleColorsDark();

		ImGuiIOPtr io = ImGui.GetIO();
		io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

		_vbo = _gl.GenBuffer();
		_ebo = _gl.GenBuffer();

		_shaderId = ShaderLoader.Load(_gl, _vertexShader, _fragmentShader);
		_projectionMatrixLocation = _gl.GetUniformLocation(_shaderId, "projectionMatrix");
		_imageLocation = _gl.GetUniformLocation(_shaderId, "image");

		RecreateFontDeviceTexture();
	}

	#region Initialization

	private unsafe void RecreateFontDeviceTexture()
	{
		// Build texture atlas
		ImGuiIOPtr io = ImGui.GetIO();

		// Load as RGBA 32-bit (75% of the memory is wasted, but default font is so small) because it is more likely to be compatible with user's existing shaders.
		// If your ImTextureId represent a higher-level concept than just a GL texture id, consider calling GetTexDataAsAlpha8() instead to save on GPU memory.
		io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

		byte[] data = new byte[width * height * bytesPerPixel];
		Marshal.Copy(pixels, data, 0, data.Length);
		uint textureId = _gl.GenTexture();

		_gl.BindTexture(TextureTarget.Texture2D, textureId);

		_gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
		_gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);
		_gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
		_gl.TexParameterI(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

		fixed (byte* b = data)
			_gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)width, (uint)height, 0, GLEnum.Rgba, PixelType.UnsignedByte, b);

		io.Fonts.SetTexID((IntPtr)textureId);
	}

	#endregion Initialization

	public void Destroy()
	{
		_gl.DeleteBuffer(_vbo);
		_gl.DeleteBuffer(_ebo);
		_gl.DeleteVertexArray(_vao);

		ImGui.DestroyContext(_context);
	}

	public void WindowResized(int width, int height)
	{
		_windowWidth = width;
		_windowHeight = height;
	}

	public void Render()
	{
		ImGui.Render();
		RenderImDrawData(ImGui.GetDrawData());
	}

	public void Update(float deltaSeconds)
	{
		ImGuiIOPtr io = ImGui.GetIO();
		io.DisplaySize = new(_windowWidth, _windowHeight);
		io.DisplayFramebufferScale = Vector2.One;
		io.DeltaTime = deltaSeconds;

		UpdateImGuiInput();

		ImGui.NewFrame();

		// TODO: This doesn't work. Figure out where these should be set (or, figure out where they're being reset exactly).
		io.KeyCtrl = GlfwInput.IsKeyDown(Keys.ControlLeft) || GlfwInput.IsKeyDown(Keys.ControlRight);
		io.KeyAlt = GlfwInput.IsKeyDown(Keys.AltLeft) || GlfwInput.IsKeyDown(Keys.AltRight);
		io.KeyShift = GlfwInput.IsKeyDown(Keys.ShiftLeft) || GlfwInput.IsKeyDown(Keys.ShiftRight);
		io.KeySuper = GlfwInput.IsKeyDown(Keys.SuperLeft) || GlfwInput.IsKeyDown(Keys.SuperRight);
	}

	#region Input

	private void UpdateImGuiInput()
	{
		ImGuiIOPtr io = ImGui.GetIO();

		io.MousePos = GlfwInput.CursorPosition;
		io.MouseWheel = GlfwInput.MouseWheelY;

		io.MouseDown[0] = GlfwInput.IsMouseButtonDown(MouseButton.Left);
		io.MouseDown[1] = GlfwInput.IsMouseButtonDown(MouseButton.Right);
		io.MouseDown[2] = GlfwInput.IsMouseButtonDown(MouseButton.Middle);

		for (int i = 0; i < _allKeys.Count; i++)
		{
			Keys key = _allKeys[i];
			int keyValue = (int)key;
			if (keyValue < 0)
				continue;

			io.AddKeyEvent(key.GetImGuiKey(), GlfwInput.IsKeyDown(key));
		}

		for (int i = 0; i < GlfwInput.CharsPressed.Count; i++)
			io.AddInputCharacter(GlfwInput.CharsPressed[i]);
	}

	#endregion Input

	#region Rendering

	private unsafe void SetUpRenderState(ImDrawDataPtr drawDataPtr)
	{
		// Setup render state: alpha-blending enabled, no face culling, no depth testing, scissor enabled, polygon fill
		_gl.Enable(GLEnum.Blend);
		_gl.BlendEquation(GLEnum.FuncAdd);
		_gl.BlendFuncSeparate(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha, GLEnum.One, GLEnum.OneMinusSrcAlpha);
		_gl.Disable(GLEnum.CullFace);
		_gl.Disable(GLEnum.DepthTest);
		_gl.Disable(GLEnum.StencilTest);
		_gl.Enable(GLEnum.ScissorTest);
		_gl.Disable(GLEnum.PrimitiveRestart);
		_gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);

		Matrix4x4 orthographicProjection = Matrix4x4.CreateOrthographicOffCenter(
			left: drawDataPtr.DisplayPos.X,
			right: drawDataPtr.DisplayPos.X + drawDataPtr.DisplaySize.X,
			bottom: drawDataPtr.DisplayPos.Y + drawDataPtr.DisplaySize.Y,
			top: drawDataPtr.DisplayPos.Y,
			zNearPlane: -1,
			zFarPlane: 1);

		_gl.UseProgram(_shaderId);
		_gl.Uniform1(_imageLocation, 0);
		_gl.UniformMatrix4x4(_projectionMatrixLocation, orthographicProjection);

		_gl.BindSampler(0, 0);

		// Setup desired GL state
		// Recreate the VAO every time (this is to easily allow multiple GL contexts to be rendered to. VAO are not shared among GL contexts)
		// The renderer would actually work without any VAO bound, but then our VertexAttrib calls would overwrite the default one currently bound.
		_vao = _gl.GenVertexArray();
		_gl.BindVertexArray(_vao);

		// Bind vertex/index buffers and setup attributes for ImDrawVert
		_gl.BindBuffer(GLEnum.ArrayBuffer, _vbo);
		_gl.BindBuffer(GLEnum.ElementArrayBuffer, _ebo);

		_gl.EnableVertexAttribArray(0);
		_gl.EnableVertexAttribArray(1);
		_gl.EnableVertexAttribArray(2);

		_gl.VertexAttribPointer(0, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)0);
		_gl.VertexAttribPointer(1, 2, GLEnum.Float, false, (uint)sizeof(ImDrawVert), (void*)8);
		_gl.VertexAttribPointer(2, 4, GLEnum.UnsignedByte, true, (uint)sizeof(ImDrawVert), (void*)16);
	}

	private unsafe void RenderImDrawData(ImDrawDataPtr drawDataPtr)
	{
		int framebufferWidth = (int)(drawDataPtr.DisplaySize.X * drawDataPtr.FramebufferScale.X);
		int framebufferHeight = (int)(drawDataPtr.DisplaySize.Y * drawDataPtr.FramebufferScale.Y);
		if (framebufferWidth <= 0 || framebufferHeight <= 0)
			return;

		SetUpRenderState(drawDataPtr);

		// Will project scissor/clipping rectangles into framebuffer space
		Vector2 clipOff = drawDataPtr.DisplayPos; // (0,0) unless using multi-viewports
		Vector2 clipScale = drawDataPtr.FramebufferScale; // (1,1) unless using retina display which are often (2,2)

		// Render command lists
		for (int n = 0; n < drawDataPtr.CmdListsCount; n++)
		{
			ImDrawListPtr cmdListPtr = drawDataPtr.CmdLists[n];

			// Upload vertex/index buffers
			_gl.BufferData(GLEnum.ArrayBuffer, (nuint)(cmdListPtr.VtxBuffer.Size * sizeof(ImDrawVert)), (void*)cmdListPtr.VtxBuffer.Data, GLEnum.StreamDraw);
			_gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(cmdListPtr.IdxBuffer.Size * sizeof(ushort)), (void*)cmdListPtr.IdxBuffer.Data, GLEnum.StreamDraw);

			for (int cmdI = 0; cmdI < cmdListPtr.CmdBuffer.Size; cmdI++)
			{
				ImDrawCmdPtr cmdPtr = cmdListPtr.CmdBuffer[cmdI];
				if (cmdPtr.UserCallback != IntPtr.Zero)
					throw new NotImplementedException();

				Vector4 clipRect;
				clipRect.X = (cmdPtr.ClipRect.X - clipOff.X) * clipScale.X;
				clipRect.Y = (cmdPtr.ClipRect.Y - clipOff.Y) * clipScale.Y;
				clipRect.Z = (cmdPtr.ClipRect.Z - clipOff.X) * clipScale.X;
				clipRect.W = (cmdPtr.ClipRect.W - clipOff.Y) * clipScale.Y;

				if (clipRect.X >= framebufferWidth || clipRect.Y >= framebufferHeight || clipRect.Z < 0.0f || clipRect.W < 0.0f)
					continue;

				// Apply scissor/clipping rectangle
				_gl.Scissor((int)clipRect.X, (int)(framebufferHeight - clipRect.W), (uint)(clipRect.Z - clipRect.X), (uint)(clipRect.W - clipRect.Y));

				// Bind texture, Draw
				_gl.BindTexture(GLEnum.Texture2D, (uint)cmdPtr.TextureId);
				_gl.DrawElementsBaseVertex(GLEnum.Triangles, cmdPtr.ElemCount, GLEnum.UnsignedShort, (void*)(cmdPtr.IdxOffset * sizeof(ushort)), (int)cmdPtr.VtxOffset);
			}
		}

		// Destroy the temporary VAO
		_gl.DeleteVertexArray(_vao);
		_vao = 0;

		// Restore scissors
		_gl.Disable(EnableCap.ScissorTest);
	}

	#endregion Rendering
}
