using Silk.NET.GLFW;
using System.Numerics;

namespace ImGuiGlfwDotnet;

public static class GlfwInput
{
	private static readonly Dictionary<MouseButton, bool> _mouseButtonsDown = new();
	private static readonly Dictionary<Keys, bool> _keysDown = [];
	private static readonly List<CharPressedEvent> _charsPressed = [];

	public static Vector2 CursorPosition { get; private set; }
	public static float MouseWheelY { get; private set; }
	public static IReadOnlyList<CharPressedEvent> CharsPressed => _charsPressed;

	#region Callbacks

	public static void CursorPosCallback(double x, double y)
	{
		CursorPosition = new((float)x, (float)y);
	}

	public static void MouseWheelCallback(double deltaY)
	{
		MouseWheelY = (float)deltaY;
	}

	public static void MouseButtonCallback(MouseButton button, InputAction state, KeyModifiers keyModifiers)
	{
		if (state is InputAction.Press or InputAction.Repeat)
		{
			_mouseButtonsDown[button] = true;

			// TODO: Handle modifiers.
		}
		else
		{
			_mouseButtonsDown[button] = false;
		}
	}

	public static void KeyCallback(Keys key, InputAction state, KeyModifiers keyModifiers)
	{
		if (state is InputAction.Press or InputAction.Repeat)
		{
			_keysDown[key] = true;

			// TODO: Handle modifiers?
		}
		else
		{
			_keysDown[key] = false;
		}
	}

	public static void CharCallback(uint codepoint)
	{
		_charsPressed.Add(new(codepoint, 0));
	}

	#endregion Callbacks

	public static bool IsMouseButtonDown(MouseButton button)
	{
		return _mouseButtonsDown.TryGetValue(button, out bool isDown) && isDown;
	}

	public static bool IsKeyDown(Keys key)
	{
		return _keysDown.TryGetValue(key, out bool isDown) && isDown;
	}

	public static void PostRender()
	{
		_charsPressed.Clear();
		MouseWheelY = 0;
	}
}
