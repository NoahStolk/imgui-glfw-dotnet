using ImGuiGlfw;
using ImGuiGlfw.Sample;
using ImGuiNET;
using System.Numerics;

Graphics.CreateWindow(new Graphics.WindowState("ImGuiGlfw - Sample", Constants.WindowWidth, Constants.WindowHeight, false));
Graphics.SetWindowSizeLimits(1024, 768, 4096, 2160);

ImGuiController imGuiController = new(Graphics.Gl, Input.GlfwInput, Constants.WindowWidth, Constants.WindowHeight);
imGuiController.CreateDefaultFont();

ImGuiStylePtr style = ImGui.GetStyle();
style.WindowPadding = new Vector2(4, 4);
style.ItemSpacing = new Vector2(4, 4);

Graphics.OnChangeWindowSize = (w, h) =>
{
	Graphics.Gl.Viewport(0, 0, (uint)w, (uint)h);
	imGuiController.WindowResized(w, h);
};

App app = new(imGuiController);
app.Run();
