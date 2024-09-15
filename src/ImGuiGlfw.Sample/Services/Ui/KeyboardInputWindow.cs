﻿using ImGuiGlfw.Sample.Utils;
using ImGuiNET;
using Silk.NET.GLFW;
using System.Numerics;

namespace ImGuiGlfw.Sample.Services.Ui;

public sealed class KeyboardInputWindow
{
	private readonly Dictionary<Keys, string> _keyDisplayStringCache = [];

	private readonly GlfwInput _glfwInput;

	public KeyboardInputWindow(GlfwInput glfwInput)
	{
		_glfwInput = glfwInput;
	}

	public void Render()
	{
		if (ImGui.Begin("Keyboard input"))
		{
			ImGuiIOPtr io = ImGui.GetIO();

			ImGui.SeparatorText("ImGui key modifiers");
			ImGui.TextColored(io.KeyCtrl ? Rgba.White : Rgba.Gray(0.4f), "CTRL");
			ImGui.SameLine();
			ImGui.TextColored(io.KeyShift ? Rgba.White : Rgba.Gray(0.4f), "SHIFT");
			ImGui.SameLine();
			ImGui.TextColored(io.KeyAlt ? Rgba.White : Rgba.Gray(0.4f), "ALT");
			ImGui.SameLine();
			ImGui.TextColored(io.KeySuper ? Rgba.White : Rgba.Gray(0.4f), "SUPER");

			ImGui.SeparatorText("GLFW keys");
			if (ImGui.BeginTable("GLFW keys", 8))
			{
				for (int i = 0; i < 1024; i++)
				{
					if (i == 0)
						ImGui.TableNextRow();

					Keys key = (Keys)i;
					if (!Enum.IsDefined(key))
						continue;

					bool isDown = _glfwInput.IsKeyDown(key);

					ImGui.TableNextColumn();

					if (!_keyDisplayStringCache.TryGetValue(key, out string? displayString))
					{
						displayString = key.ToString();
						_keyDisplayStringCache[key] = displayString;
					}

					ImGui.TextColored(isDown ? Rgba.White : Rgba.Gray(0.4f), displayString);
				}

				ImGui.EndTable();
			}

			ImGui.SeparatorText("GLFW pressed chars");

			ImGui.Text(Inline.Span($"{_glfwInput.CharsPressed.Count} key(s):"));
			if (ImGui.BeginChild("CharsPressedChildWindow", new Vector2(0, 48), ImGuiChildFlags.Border, ImGuiWindowFlags.AlwaysVerticalScrollbar))
			{
				for (int i = 0; i < _glfwInput.CharsPressed.Count; i++)
				{
					ImGui.Text(Inline.Span((char)_glfwInput.CharsPressed[i]));
				}
			}

			ImGui.EndChild();

			ImGui.SeparatorText("GLFW [SPACE] key state");

			ImGui.Text(Inline.Span($"Down: {(_glfwInput.IsKeyDown(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Span($"Repeating: {(_glfwInput.IsKeyRepeating(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Span($"Pressed: {(_glfwInput.IsKeyPressed(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Span($"Released: {(_glfwInput.IsKeyReleased(Keys.Space) ? "true" : "false")}"));
		}

		ImGui.End();
	}
}
