using ImGuiGlfwDotnet;
using ImGuiGlfwDotnet.Sample;
using ImGuiNET;

Graphics.CreateWindow(new("ImGuiGlfwDotnet - Sample", Constants.WindowWidth, Constants.WindowHeight, false));
Graphics.SetWindowSizeLimits(1024, 768, 4096, 2160);

ImGuiController imGuiController = new(Graphics.Gl, Constants.WindowWidth, Constants.WindowHeight);

ImGuiStylePtr style = ImGui.GetStyle();
style.WindowPadding = new(4, 4);
style.ItemSpacing = new(4, 4);

Graphics.OnChangeWindowSize = (w, h) =>
{
	Graphics.Gl.Viewport(0, 0, (uint)w, (uint)h);
	imGuiController.WindowResized(w, h);
};

App app = new(imGuiController);
app.Run();
