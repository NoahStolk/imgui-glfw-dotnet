using ImGuiGlfw.Sample.Extensions;
using ImGuiGlfw.Sample.Ui;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using StrongInject;

namespace ImGuiGlfw.Sample;

[Register<GraphicsOptions>(Scope.SingleInstance)]
[Register<GlfwInput>(Scope.SingleInstance)]
[Register<App>(Scope.SingleInstance)]
[Register<PerformanceMeasurement>(Scope.SingleInstance)]
[Register<InputDebugWindow>(Scope.SingleInstance)]
[Register<InputWindow>(Scope.SingleInstance)]
[Register<PerformanceWindow>(Scope.SingleInstance)]
[Register<SettingsWindow>(Scope.SingleInstance)]
public partial class Container : IContainer<App>
{
	[Factory(Scope.SingleInstance)]
	private ImGuiController CreateImGuiController(GL gl, GraphicsOptions graphicsOptions, GlfwInput glfwInput)
	{
		ImGuiController imGuiController = new(gl, glfwInput, graphicsOptions.WindowWidth, graphicsOptions.WindowHeight);
		imGuiController.WindowResized(graphicsOptions.WindowWidth, graphicsOptions.WindowHeight);
		imGuiController.CreateDefaultFont();
		return imGuiController;
	}

	[Factory(Scope.SingleInstance)]
	private Glfw GetGlfw()
	{
		Glfw glfw = Glfw.GetApi();
		glfw.Init();
		glfw.CheckError();

		glfw.WindowHint(WindowHintInt.ContextVersionMajor, 3);
		glfw.WindowHint(WindowHintInt.ContextVersionMinor, 3);
		glfw.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
		glfw.WindowHint(WindowHintBool.Focused, true);
		glfw.WindowHint(WindowHintBool.Resizable, true);
		glfw.CheckError();

		return glfw;
	}

	[Factory(Scope.SingleInstance)]
	private unsafe WindowHandle* CreateWindow(Glfw glfw, GlfwInput glfwInput, GraphicsOptions graphicsOptions)
	{
		WindowHandle* window = glfw.CreateWindow(graphicsOptions.WindowWidth, graphicsOptions.WindowHeight, graphicsOptions.WindowTitle, null, null);
		glfw.CheckError();
		if (window == null)
			throw new InvalidOperationException("Could not create window.");

		glfw.SetCursorPosCallback(window, (_, x, y) => glfwInput.CursorPosCallback(x, y));
		glfw.SetScrollCallback(window, (_, _, y) => glfwInput.MouseWheelCallback(y));
		glfw.SetMouseButtonCallback(window, (_, button, state, _) => glfwInput.MouseButtonCallback(button, state));
		glfw.SetKeyCallback(window, (_, keys, _, state, _) => glfwInput.KeyCallback(keys, state));
		glfw.SetCharCallback(window, (_, codepoint) => glfwInput.CharCallback(codepoint));

		(int windowX, int windowY) = glfw.GetInitialWindowPos(graphicsOptions.WindowWidth, graphicsOptions.WindowHeight);
		glfw.SetWindowPos(window, windowX, windowY);

		glfw.MakeContextCurrent(window);
		glfw.SetWindowSizeLimits(window, 1024, 768, -1, -1);

		return window;
	}

	[Factory(Scope.SingleInstance)]
	private GL GetGl(Glfw glfw)
	{
		return GL.GetApi(glfw.GetProcAddress);
	}
}
