using ImGuiNET;
using Silk.NET.GLFW;

namespace ImGuiGlfwDotnet.Ui;

public static class SettingsWindow
{
	private static bool _vsync;

	public static void Render(ref bool showWindow, Glfw glfw)
	{
		if (ImGui.Begin("Settings", ref showWindow))
		{
			if (ImGui.Checkbox("VSync", ref _vsync))
				glfw.SwapInterval(_vsync ? 1 : 0);
		}

		ImGui.End();
	}
}
