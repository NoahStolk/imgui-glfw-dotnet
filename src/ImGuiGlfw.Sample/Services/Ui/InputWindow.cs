using ImGuiGlfw.Sample.Utils;
using ImGuiNET;
using Silk.NET.GLFW;
using System.Numerics;

namespace ImGuiGlfw.Sample.Services.Ui;

public sealed class InputWindow
{
	private readonly Dictionary<Keys, string> _keyDisplayStringCache = [];
	private readonly Dictionary<MouseButton, string> _mouseButtonDisplayStringCache = [];

	private readonly GlfwInput _glfwInput;

	public InputWindow(GlfwInput glfwInput)
	{
		_glfwInput = glfwInput;
	}

	public void Render()
	{
		if (ImGui.Begin("Input"))
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

			ImGui.SeparatorText("Space key state");

			ImGui.Text(Inline.Span($"Down: {(_glfwInput.IsKeyDown(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Span($"Repeating: {(_glfwInput.IsKeyRepeating(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Span($"Pressed: {(_glfwInput.IsKeyPressed(Keys.Space) ? "true" : "false")}"));
			ImGui.Text(Inline.Span($"Released: {(_glfwInput.IsKeyReleased(Keys.Space) ? "true" : "false")}"));

			ImGui.SeparatorText("LMB state");

			ImGui.Text(Inline.Span($"Down: {(_glfwInput.IsMouseButtonDown(MouseButton.Left) ? "true" : "false")}"));
			ImGui.Text(Inline.Span($"Pressed: {(_glfwInput.IsMouseButtonPressed(MouseButton.Left) ? "true" : "false")}"));
			ImGui.Text(Inline.Span($"Released: {(_glfwInput.IsMouseButtonReleased(MouseButton.Left) ? "true" : "false")}"));

			ImGui.SeparatorText("GLFW mouse buttons");

			if (ImGui.BeginTable("GLFW mouse buttons", 8))
			{
				for (int i = 0; i < 8; i++)
				{
					if (i == 0)
						ImGui.TableNextRow();

					MouseButton button = (MouseButton)i;
					if (!Enum.IsDefined(button))
						continue;

					bool isDown = _glfwInput.IsMouseButtonDown(button);

					ImGui.TableNextColumn();

					if (!_mouseButtonDisplayStringCache.TryGetValue(button, out string? displayString))
					{
						displayString = button.ToString();
						_mouseButtonDisplayStringCache[button] = displayString;
					}

					ImGui.TextColored(isDown ? Rgba.White : Rgba.Gray(0.4f), displayString);
				}

				ImGui.EndTable();
			}

			ImGui.SeparatorText("GLFW mouse wheel");

			ImGui.Text(Inline.Span($"Y: {_glfwInput.MouseWheelY:0.00}"));

			ImGui.SeparatorText("GLFW mouse position");

			ImGui.Text(Inline.Span(_glfwInput.CursorPosition));
		}

		ImGui.End();
	}
}
