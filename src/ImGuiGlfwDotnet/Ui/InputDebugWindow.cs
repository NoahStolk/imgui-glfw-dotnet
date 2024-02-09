using ImGuiGlfwDotnet.Internals;
using ImGuiNET;

namespace ImGuiGlfwDotnet.Ui;

public static class InputDebugWindow
{
	private static readonly string[] _debugTextInput = new string[1024];
	private static bool _checkbox;

	static InputDebugWindow()
	{
		for (int i = 0; i < _debugTextInput.Length; i++)
			_debugTextInput[i] = string.Empty;
	}

	public static void Render(ref bool showWindow)
	{
		if (ImGui.Begin("Input debug", ref showWindow))
		{
			ImGui.SeparatorText("Test keyboard input");

			ImGui.InputText("Letters, numbers", ref _debugTextInput[0], 1024);
			ImGui.InputText("Letters, numbers (SHIFT)", ref _debugTextInput[1], 1024);

			ImGui.InputTextMultiline("Enter, arrow keys", ref _debugTextInput[2], 1024, new(0, 64));
			ImGui.InputTextMultiline("Tab", ref _debugTextInput[3], 1024, new(0, 64), ImGuiInputTextFlags.AllowTabInput);
			ImGui.InputTextMultiline("CTRL shortcuts (CTRL+A, CTRL+C, CTRL+V)", ref _debugTextInput[4], 1024, new(0, 64));
			ImGui.InputTextMultiline("SHIFT shortcuts (SHIFT+arrow keys, SHIFT+HOME)", ref _debugTextInput[5], 1024, new(0, 64));

			ImGui.SeparatorText("Test mouse input");

			ImGui.Checkbox("Checkbox", ref _checkbox);

			if (ImGui.BeginChild("Scroll area", new(256, 128)))
			{
				for (int i = 0; i < 50; i++)
				{
					Rgba color = (i % 3) switch
					{
						0 => Rgba.Yellow,
						1 => Rgba.Aqua,
						_ => Rgba.Red,
					};
					ReadOnlySpan<char> text = (i % 3) switch
					{
						0 => "Scrolling should not go to top or bottom instantly",
						1 => "Scrolling should go evenly per frame (not missing inputs or jumping)",
						_ => "This should work with and without VSync",
					};

					ImGui.PushStyleColor(ImGuiCol.Text, color);
					ImGui.TextWrapped(text);
					ImGui.PopStyleColor();

					ImGui.Separator();
				}
			}

			ImGui.EndChild();

			ImGui.TextWrapped("""
				ISSUES:

				1. Certain keys currently cause lag for some reason:
				   Keypad: 4 5 6 1 2 3 0
				   Digits row: - = `
				   Other keys (not keypad): [ ] \ ; ' , . / CAPSLOCK

				   Maybe GLFW doesn't repeat them as fast???

				2. CTRL+A, CTRL+C, and CTRL+V don't work.
				""");
		}

		ImGui.End();
	}
}
