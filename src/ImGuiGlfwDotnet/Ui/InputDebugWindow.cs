﻿using ImGuiGlfwDotnet.Internals;
using ImGuiNET;

namespace ImGuiGlfwDotnet.Ui;

public static class InputDebugWindow
{
	private static readonly string[] _debugTextInput =
	[
		"Type letters and numbers: ",
		"NOW HOLD SHIFT: ",
		"Enter some enters, and use the arrow keys to navigate.",
		"Insert some tabs (only works for this input field).",
		"Select all text, copy, paste, and use CTRL+arrows to navigate between words.",
		"Use SHIFT+arrows and SHIFT+home to select text.",
	];
	private static bool _checkbox;

	public static void Render(ref bool showWindow)
	{
		if (ImGui.Begin("Input debug", ref showWindow))
		{
			ImGui.SeparatorText("Test keyboard input");

			ImGui.InputText("Letters, numbers", ref _debugTextInput[0], 1024);
			ImGui.InputText("Letters, numbers (SHIFT)", ref _debugTextInput[1], 1024);

			ImGui.InputTextMultiline("Enter, arrow keys", ref _debugTextInput[2], 1024, new(0, 64));
			ImGui.InputTextMultiline("Tab", ref _debugTextInput[3], 1024, new(0, 64), ImGuiInputTextFlags.AllowTabInput);
			ImGui.InputTextMultiline("CTRL shortcut\n- CTRL+A\n- CTRL+C\n- CTRL+V\n- CTRL+arrows", ref _debugTextInput[4], 1024, new(0, 64));
			ImGui.InputTextMultiline("SHIFT shortcuts\n- SHIFT+arrows\n- SHIFT+home", ref _debugTextInput[5], 1024, new(0, 64));

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

			ImGui.SeparatorText("ISSUES");

			ImGui.TextWrapped("CTRL+A, CTRL+C, and CTRL+V don't work. Oddly enough, SHIFT+HOME etc does work.");
			ImGui.TextWrapped("CTRL+left arrow key works too, so it seems we need to do something else to get the CTRL+A shortcut working (submit A 'KEY' instead of A char?).");
		}

		ImGui.End();
	}
}
