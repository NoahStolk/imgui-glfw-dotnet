using ImGuiGlfw.Sample.Extensions;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace ImGuiGlfw.Sample;

public static class Graphics
{
	private static bool _windowIsCreated;

	private static Glfw? _glfw;
	private static GL? _gl;

	public static Glfw Glfw => _glfw ?? throw new InvalidOperationException("GLFW is not initialized.");
	public static GL Gl => _gl ?? throw new InvalidOperationException("OpenGL is not initialized.");

	public static Action<int, int>? OnChangeWindowSize { get; set; }

	public static unsafe WindowHandle* Window { get; private set; }

	public static unsafe void CreateWindow(int windowWidth, int windowHeight, string windowTitle)
	{
		if (_windowIsCreated)
			throw new InvalidOperationException("Window is already created. Cannot create window again.");

		_glfw = Glfw.GetApi();
		_glfw.Init();
		_glfw.CheckError();

		_glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
		_glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		_glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		_glfw.WindowHint(WindowHintBool.Focused, true);
		_glfw.WindowHint(WindowHintBool.Resizable, true);
		_glfw.CheckError();

		Window = _glfw.CreateWindow(windowWidth, windowHeight, windowTitle, null, null);
		_glfw.CheckError();
		if (Window == null)
			throw new InvalidOperationException("Could not create window.");

		_glfw.SetFramebufferSizeCallback(Window, (_, w, h) => SetWindowSize(w, h));
		_glfw.SetCursorPosCallback(Window, (_, x, y) => Input.GlfwInput.CursorPosCallback(x, y));
		_glfw.SetScrollCallback(Window, (_, _, y) => Input.GlfwInput.MouseWheelCallback(y));
		_glfw.SetMouseButtonCallback(Window, (_, button, state, _) => Input.GlfwInput.MouseButtonCallback(button, state));
		_glfw.SetKeyCallback(Window, (_, keys, _, state, _) => Input.GlfwInput.KeyCallback(keys, state));
		_glfw.SetCharCallback(Window, (_, codepoint) => Input.GlfwInput.CharCallback(codepoint));

		(int windowX, int windowY) = _glfw.GetInitialWindowPos(windowWidth, windowHeight);
		_glfw.SetWindowPos(Window, windowX, windowY);

		_glfw.MakeContextCurrent(Window);
		_gl = GL.GetApi(_glfw.GetProcAddress);

		SetWindowSize(windowWidth, windowHeight);
		Glfw.SetWindowSizeLimits(Window, 1024, 768, -1, -1);

		_glfw.SwapInterval(0); // Turns VSync off.

		_windowIsCreated = true;
	}

	private static void SetWindowSize(int width, int height)
	{
		OnChangeWindowSize?.Invoke(width, height);
	}
}
