using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiGlfw.Sample.Utils;

#pragma warning disable RCS1163
[InterpolatedStringHandler]
internal ref struct InlineInterpolatedStringHandler
{
	private int _charsWritten;

	// ReSharper disable UnusedParameter.Local
	public InlineInterpolatedStringHandler(int literalLength, int formattedCount)
	{
	}

	// ReSharper restore UnusedParameter.Local
	public static implicit operator ReadOnlySpan<byte>(InlineInterpolatedStringHandler handler)
	{
		Inline.Buffer[handler._charsWritten] = 0x00; // Null-terminate the UTF-8 string.
		return Inline.Buffer[..handler._charsWritten];
	}

	public unsafe void AppendLiteral(string s)
	{
		fixed (char* utf16Ptr = s)
		{
			int utf8ByteCount = Encoding.UTF8.GetByteCount(utf16Ptr, s.Length);
			if (utf8ByteCount == 0)
				return;

			if (utf8ByteCount > Inline.Buffer.Length - _charsWritten)
				throw new InvalidOperationException("The formatted string is too long.");

			fixed (byte* bufferPtr = Inline.Buffer)
				_charsWritten += Encoding.UTF8.GetBytes(utf16Ptr, s.Length, bufferPtr + _charsWritten, utf8ByteCount);
		}
	}

	public void AppendFormatted(ReadOnlySpan<byte> s)
	{
		if (!s.TryCopyTo(Inline.Buffer[_charsWritten..]))
			throw new InvalidOperationException("The formatted string is too long.");

		_charsWritten += s.Length;
	}

	public void AppendFormatted<T>(T t, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : IUtf8SpanFormattable
	{
		if (!t.TryFormat(Inline.Buffer[_charsWritten..], out int charsWritten, format, provider))
			throw new InvalidOperationException("The formatted string is too long.");

		_charsWritten += charsWritten;
	}
}
