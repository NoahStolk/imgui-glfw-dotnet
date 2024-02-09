using ImGuiNET;
using Silk.NET.GLFW;

namespace ImGuiGlfw.Sample.Ui;

public static class SettingsWindow
{
	private static bool _vsync;

	public static void Render(Glfw glfw)
	{
		if (ImGui.Begin("Settings"))
		{
			if (ImGui.Checkbox("VSync", ref _vsync))
				glfw.SwapInterval(_vsync ? 1 : 0);
		}

		ImGui.End();
	}
}
