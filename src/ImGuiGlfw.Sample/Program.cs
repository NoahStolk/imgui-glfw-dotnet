using ImGuiGlfw;
using ImGuiGlfw.Sample;
using ImGuiGlfw.Sample.Utils;
using ImGuiNET;
using System.Numerics;

Graphics.CreateWindow(Constants.WindowWidth, Constants.WindowHeight, "ImGuiGlfw - Sample");

ImGuiController imGuiController = new(Graphics.Gl, Input.GlfwInput, Constants.WindowWidth, Constants.WindowHeight);
imGuiController.CreateDefaultFont();

Graphics.OnChangeWindowSize = (w, h) =>
{
	Graphics.Gl.Viewport(0, 0, (uint)w, (uint)h);
	imGuiController.WindowResized(w, h);
};

App app = new(imGuiController);
app.Run();
