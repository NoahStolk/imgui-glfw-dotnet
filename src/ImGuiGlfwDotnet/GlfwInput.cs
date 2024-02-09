using Silk.NET.GLFW;
using System.Numerics;

namespace ImGuiGlfwDotnet;

public static class GlfwInput
{
	private static readonly Dictionary<MouseButton, bool> _mouseButtonsDown = new();
	private static readonly Dictionary<Keys, bool> _keysDown = [];
	private static readonly List<Keys> _keysChanged = [];
	private static readonly List<uint> _charsPressed = [];

	public static Vector2 CursorPosition { get; private set; }
	public static float MouseWheelY { get; private set; }
	public static IReadOnlyList<Keys> KeysChanged => _keysChanged;
	public static IReadOnlyList<uint> CharsPressed => _charsPressed;

	#region Callbacks

	public static void CursorPosCallback(double x, double y)
	{
		CursorPosition = new((float)x, (float)y);
	}

	public static void MouseWheelCallback(double deltaY)
	{
		MouseWheelY = (float)deltaY;
	}

	public static void MouseButtonCallback(MouseButton button, InputAction state)
	{
		_mouseButtonsDown[button] = state is InputAction.Press or InputAction.Repeat;
	}

	public static void KeyCallback(Keys key, InputAction state)
	{
		_keysChanged.Add(key);
		_keysDown[key] = state is InputAction.Press or InputAction.Repeat;
	}

	public static void CharCallback(uint codepoint)
	{
		_charsPressed.Add(codepoint);
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
		_keysChanged.Clear();
		MouseWheelY = 0;
	}
}
