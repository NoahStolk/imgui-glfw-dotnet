using Silk.NET.OpenGL;

namespace ImGuiGlfwDotnet;

public static class ShaderLoader
{
	public static uint Load(GL gl, string vertexCode, string fragmentCode)
	{
		uint vs = gl.CreateShader(ShaderType.VertexShader);
		gl.ShaderSource(vs, vertexCode);
		gl.CompileShader(vs);
		CheckShaderStatus(gl, "Vertex", vs);

		uint fs = gl.CreateShader(ShaderType.FragmentShader);
		gl.ShaderSource(fs, fragmentCode);
		gl.CompileShader(fs);
		CheckShaderStatus(gl, "Fragment", fs);

		uint id = gl.CreateProgram();

		gl.AttachShader(id, vs);
		gl.AttachShader(id, fs);

		gl.LinkProgram(id);

		gl.DetachShader(id, vs);
		gl.DetachShader(id, fs);

		gl.DeleteShader(vs);
		gl.DeleteShader(fs);

		return id;
	}

	private static void CheckShaderStatus(GL gl, string shaderType, uint shaderId)
	{
		string infoLog = gl.GetShaderInfoLog(shaderId);
		if (!string.IsNullOrWhiteSpace(infoLog))
			throw new InvalidOperationException($"{shaderType} shader compile error: {infoLog}");
	}
}
