﻿using System.Numerics;

namespace ImGuiGlfw.Sample;

/// <summary>
/// Unsafe methods to quickly format values into a <see cref="Span{T}"/> without allocating memory.
/// These must only be used inline, as the <see cref="Span{T}"/> is only valid until the next method call.
/// </summary>
internal static class Inline
{
	private static readonly char[] _buffer = new char[2048];

	public static Span<char> Buffer => _buffer;

	public static ReadOnlySpan<char> Span(InlineInterpolatedStringHandler interpolatedStringHandler)
	{
		return interpolatedStringHandler;
	}

	public static ReadOnlySpan<char> Span<T>(T t, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		return t.TryFormat(_buffer, out int charsWritten, format, provider) ? _buffer.AsSpan()[..charsWritten] : ReadOnlySpan<char>.Empty;
	}

	public static ReadOnlySpan<char> Span(Vector2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		TryWrite(_buffer, ref charsWritten, value.X, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.Y, format, provider);
		return _buffer.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Span(Vector3 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		TryWrite(_buffer, ref charsWritten, value.X, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.Y, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.Z, format, provider);
		return _buffer.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Span(Vector4 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		TryWrite(_buffer, ref charsWritten, value.X, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.Y, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.Z, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.W, format, provider);
		return _buffer.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Span(Quaternion value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		TryWrite(_buffer, ref charsWritten, value.X, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.Y, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.Z, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.W, format, provider);
		return _buffer.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Span(Rgba value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		TryWrite(_buffer, ref charsWritten, value.R, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.G, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.B, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.A, format, provider);
		return _buffer.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Span(Matrix3x2 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		TryWrite(_buffer, ref charsWritten, value.M11, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M12, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M21, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M22, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M31, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M32, format, provider);
		return _buffer.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Span(Matrix4x4 value, ReadOnlySpan<char> format = default, IFormatProvider? provider = default)
	{
		int charsWritten = 0;
		TryWrite(_buffer, ref charsWritten, value.M11, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M12, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M13, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M14, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M21, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M22, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M23, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M24, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M31, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M32, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M33, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M34, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M41, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M42, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M43, format, provider);
		TryWriteString(_buffer, ref charsWritten, ", ");
		TryWrite(_buffer, ref charsWritten, value.M44, format, provider);
		return _buffer.AsSpan(0, charsWritten);
	}

	public static ReadOnlySpan<char> Span(string str)
	{
		return str.AsSpan();
	}

	private static bool TryWriteChar(Span<char> destination, ref int charsWritten, char value)
	{
		if (destination.IsEmpty)
			return false;

		if (charsWritten + 1 >= destination.Length)
			return false;

		destination[charsWritten++] = value;
		return true;
	}

	private static bool TryWriteString(Span<char> destination, ref int charsWritten, string value)
	{
		if (destination.IsEmpty)
			return false;

		if (charsWritten + value.Length >= destination.Length)
			return false;

		value.AsSpan().CopyTo(destination[charsWritten..]);
		charsWritten += value.Length;
		return true;
	}

	private static bool TryWrite<T>(Span<char> destination, ref int charsWritten, T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : ISpanFormattable
	{
		if (destination.IsEmpty)
			return false;

		if (!value.TryFormat(destination[charsWritten..], out int charsWrittenValue, format, provider))
			return false;

		charsWritten += charsWrittenValue;
		return true;
	}
}
