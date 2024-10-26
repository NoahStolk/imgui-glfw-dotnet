using System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiGlfw.Sample.Utils;

#pragma warning disable CA1822, RCS1163
[InterpolatedStringHandler]
internal ref struct InlineInterpolatedStringHandler
{
	private int _charsWritten;

	public InlineInterpolatedStringHandler(int literalLength, int formattedCount)
	{
	}

	public static implicit operator ReadOnlySpan<byte>(InlineInterpolatedStringHandler handler)
	{
		return Inline.Buffer[..handler._charsWritten];
	}

	public void AppendLiteral(ReadOnlySpan<byte> s)
	{
		if (s.TryCopyTo(Inline.Buffer[_charsWritten..]))
			_charsWritten += s.Length;
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
				_ = Encoding.UTF8.GetBytes(utf16Ptr, s.Length, bufferPtr + _charsWritten, utf8ByteCount);
		}
	}

	public void AppendFormatted(ReadOnlySpan<byte> s)
	{
		if (s.TryCopyTo(Inline.Buffer[_charsWritten..]))
			_charsWritten += s.Length;
	}

	public void AppendFormatted<T>(T t, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
		where T : IUtf8SpanFormattable
	{
		t.TryFormat(Inline.Buffer[_charsWritten..], out int charsWritten, format, provider);
		_charsWritten += charsWritten;
	}
}
