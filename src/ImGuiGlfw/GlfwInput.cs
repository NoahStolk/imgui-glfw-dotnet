using Silk.NET.GLFW;
using System.Numerics;

namespace ImGuiGlfw;

public sealed class GlfwInput
{
	private readonly Dictionary<MouseButton, bool> _mouseButtonsDown = new();
	private readonly List<MouseButton> _mouseButtonsChanged = [];

	private readonly Dictionary<Keys, bool> _keysDown = [];
	private readonly List<Keys> _keysChanged = [];

	private readonly List<uint> _charsPressed = [];

	public Vector2 CursorPosition { get; private set; }
	public float MouseWheelY { get; private set; }

	public IReadOnlyList<MouseButton> MouseButtonsChanged => _mouseButtonsChanged;
	public IReadOnlyList<Keys> KeysChanged => _keysChanged;
	public IReadOnlyList<uint> CharsPressed => _charsPressed;

	#region Callbacks

	public void CursorPosCallback(double x, double y)
	{
		CursorPosition = new Vector2((float)x, (float)y);
	}

	public void MouseWheelCallback(double deltaY)
	{
		MouseWheelY = (float)deltaY;
	}

	public void MouseButtonCallback(MouseButton button, InputAction state)
	{
		_mouseButtonsChanged.Add(button);
		_mouseButtonsDown[button] = state is InputAction.Press or InputAction.Repeat;
	}

	public void KeyCallback(Keys key, InputAction state)
	{
		_keysChanged.Add(key);
		_keysDown[key] = state is InputAction.Press or InputAction.Repeat;
	}

	public void CharCallback(uint codepoint)
	{
		_charsPressed.Add(codepoint);
	}

	#endregion Callbacks

	public bool IsMouseButtonDown(MouseButton button)
	{
		return _mouseButtonsDown.TryGetValue(button, out bool isDown) && isDown;
	}

	public bool IsMouseButtonPressed(MouseButton button)
	{
		return _mouseButtonsChanged.Contains(button) && IsMouseButtonDown(button);
	}

	public bool IsMouseButtonReleased(MouseButton button)
	{
		return _mouseButtonsChanged.Contains(button) && !IsMouseButtonDown(button);
	}

	public bool IsKeyDown(Keys key)
	{
		return _keysDown.TryGetValue(key, out bool isDown) && isDown;
	}

	public bool IsKeyPressed(Keys key)
	{
		return _keysChanged.Contains(key) && IsKeyDown(key);
	}

	public bool IsKeyReleased(Keys key)
	{
		return _keysChanged.Contains(key) && !IsKeyDown(key);
	}

	public void PostRender()
	{
		_mouseButtonsChanged.Clear();
		_keysChanged.Clear();
		_charsPressed.Clear();
		MouseWheelY = 0;
	}
}
