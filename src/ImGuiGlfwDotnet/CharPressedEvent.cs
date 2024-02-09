using Silk.NET.GLFW;

namespace ImGuiGlfwDotnet;

public readonly record struct CharPressedEvent(uint Codepoint, KeyModifiers KeyModifiers);
