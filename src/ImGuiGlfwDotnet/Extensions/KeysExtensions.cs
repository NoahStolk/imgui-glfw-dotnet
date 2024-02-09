using ImGuiNET;
using Silk.NET.GLFW;

namespace ImGuiGlfwDotnet.Extensions;

// Use dictionaries to prevent string allocations.
public static class KeysExtensions
{
	private static readonly Dictionary<Keys, string> _displayStrings = new()
	{
		[Keys.Keypad0] = "0",
		[Keys.Keypad1] = "1",
		[Keys.Keypad2] = "2",
		[Keys.Keypad3] = "3",
		[Keys.Keypad4] = "4",
		[Keys.Keypad5] = "5",
		[Keys.Keypad6] = "6",
		[Keys.Keypad7] = "7",
		[Keys.Keypad8] = "8",
		[Keys.Keypad9] = "9",

		[Keys.Number0] = "0",
		[Keys.Number1] = "1",
		[Keys.Number2] = "2",
		[Keys.Number3] = "3",
		[Keys.Number4] = "4",
		[Keys.Number5] = "5",
		[Keys.Number6] = "6",
		[Keys.Number7] = "7",
		[Keys.Number8] = "8",
		[Keys.Number9] = "9",

		[Keys.Enter] = "Enter",
		[Keys.Space] = "Space",
		[Keys.Delete] = "Delete",
		[Keys.Comma] = ",",
		[Keys.Period] = ".",
		[Keys.Slash] = "/",
		[Keys.BackSlash] = "\\",
		[Keys.Semicolon] = ";",
		[Keys.Apostrophe] = "'",
		[Keys.LeftBracket] = "[",
		[Keys.RightBracket] = "]",
		[Keys.Minus] = "-",
		[Keys.Equal] = "=",
		[Keys.GraveAccent] = "`",

		[Keys.A] = "a",
		[Keys.B] = "b",
		[Keys.C] = "c",
		[Keys.D] = "d",
		[Keys.E] = "e",
		[Keys.F] = "f",
		[Keys.G] = "g",
		[Keys.H] = "h",
		[Keys.I] = "i",
		[Keys.J] = "j",
		[Keys.K] = "k",
		[Keys.L] = "l",
		[Keys.M] = "m",
		[Keys.N] = "n",
		[Keys.O] = "o",
		[Keys.P] = "p",
		[Keys.Q] = "q",
		[Keys.R] = "r",
		[Keys.S] = "s",
		[Keys.T] = "t",
		[Keys.U] = "u",
		[Keys.V] = "v",
		[Keys.W] = "w",
		[Keys.X] = "x",
		[Keys.Y] = "y",
		[Keys.Z] = "z",

		[Keys.F1] = "F1",
		[Keys.F2] = "F2",
		[Keys.F3] = "F3",
		[Keys.F4] = "F4",
		[Keys.F5] = "F5",
		[Keys.F6] = "F6",
		[Keys.F7] = "F7",
		[Keys.F8] = "F8",
		[Keys.F9] = "F9",
		[Keys.F10] = "F10",
		[Keys.F11] = "F11",
		[Keys.F12] = "F12",
	};

	private static readonly Dictionary<Keys, string> _displayStringsShift = new()
	{
		[Keys.Number0] = ")",
		[Keys.Number1] = "!",
		[Keys.Number2] = "@",
		[Keys.Number3] = "#",
		[Keys.Number4] = "$",
		[Keys.Number5] = "%",
		[Keys.Number6] = "^",
		[Keys.Number7] = "&",
		[Keys.Number8] = "*",
		[Keys.Number9] = "(",

		[Keys.Enter] = "Enter",
		[Keys.Space] = "Space",
		[Keys.Delete] = "Delete",
		[Keys.Comma] = "<",
		[Keys.Period] = ">",
		[Keys.Slash] = "?",
		[Keys.BackSlash] = "|",
		[Keys.Semicolon] = ":",
		[Keys.Apostrophe] = "\"",
		[Keys.LeftBracket] = "{",
		[Keys.RightBracket] = "}",
		[Keys.Minus] = "_",
		[Keys.Equal] = "+",
		[Keys.GraveAccent] = "~",

		[Keys.A] = "A",
		[Keys.B] = "B",
		[Keys.C] = "C",
		[Keys.D] = "D",
		[Keys.E] = "E",
		[Keys.F] = "F",
		[Keys.G] = "G",
		[Keys.H] = "H",
		[Keys.I] = "I",
		[Keys.J] = "J",
		[Keys.K] = "K",
		[Keys.L] = "L",
		[Keys.M] = "M",
		[Keys.N] = "N",
		[Keys.O] = "O",
		[Keys.P] = "P",
		[Keys.Q] = "Q",
		[Keys.R] = "R",
		[Keys.S] = "S",
		[Keys.T] = "T",
		[Keys.U] = "U",
		[Keys.V] = "V",
		[Keys.W] = "W",
		[Keys.X] = "X",
		[Keys.Y] = "Y",
		[Keys.Z] = "Z",

		[Keys.F1] = "F1",
		[Keys.F2] = "F2",
		[Keys.F3] = "F3",
		[Keys.F4] = "F4",
		[Keys.F5] = "F5",
		[Keys.F6] = "F6",
		[Keys.F7] = "F7",
		[Keys.F8] = "F8",
		[Keys.F9] = "F9",
		[Keys.F10] = "F10",
		[Keys.F11] = "F11",
		[Keys.F12] = "F12",
	};

	/// <summary>
	/// Gets the display string for the given key.
	/// Can be used to display shortcut information.
	/// </summary>
	public static string GetDisplayString(this Keys key, bool shift)
	{
		return (shift ? _displayStringsShift : _displayStrings).TryGetValue(key, out string? displayString) ? displayString : "[unmapped key]";
	}

	internal static ImGuiKey GetImGuiInputKey(this Keys key)
	{
		return key switch
		{
			Keys.Tab => ImGuiKey.Tab,

			Keys.Left => ImGuiKey.LeftArrow,
			Keys.Right => ImGuiKey.RightArrow,
			Keys.Up => ImGuiKey.UpArrow,
			Keys.Down => ImGuiKey.DownArrow,
			Keys.PageUp => ImGuiKey.PageUp,
			Keys.PageDown => ImGuiKey.PageDown,
			Keys.Home => ImGuiKey.Home,
			Keys.End => ImGuiKey.End,

			Keys.Insert => ImGuiKey.Insert,

			Keys.Delete => ImGuiKey.Delete,
			Keys.Backspace => ImGuiKey.Backspace,

			Keys.Enter => ImGuiKey.Enter,
			Keys.Escape => ImGuiKey.Escape,

			Keys.CapsLock => ImGuiKey.CapsLock,
			Keys.ScrollLock => ImGuiKey.ScrollLock,
			Keys.NumLock => ImGuiKey.NumLock,
			Keys.PrintScreen => ImGuiKey.PrintScreen,
			Keys.Pause => ImGuiKey.Pause,

			Keys.KeypadEnter => ImGuiKey.KeypadEnter,

			// Keys.ShiftLeft => ImGuiKey.LeftShift,
			// Keys.ControlLeft => ImGuiKey.LeftCtrl,
			// Keys.AltLeft => ImGuiKey.LeftAlt,
			// Keys.SuperLeft => ImGuiKey.LeftSuper,
			// Keys.ShiftRight => ImGuiKey.RightShift,
			// Keys.ControlRight => ImGuiKey.RightCtrl,
			// Keys.AltRight => ImGuiKey.RightAlt,
			// Keys.SuperRight => ImGuiKey.RightSuper,

			// Keys.ShiftLeft or Keys.ShiftRight => ImGuiKey.ModShift,
			// Keys.ControlLeft or Keys.ControlRight => ImGuiKey.ModCtrl,
			// Keys.AltLeft or Keys.AltRight => ImGuiKey.ModAlt,
			// Keys.SuperLeft or Keys.SuperRight => ImGuiKey.ModSuper,

			Keys.Menu => ImGuiKey.Menu,

			_ => ImGuiKey.None,
		};
	}
}
