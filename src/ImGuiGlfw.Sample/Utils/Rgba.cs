using System.Numerics;

namespace ImGuiGlfw.Sample.Utils;

internal readonly record struct Rgba(byte R, byte G, byte B, byte A)
{
	public static Rgba Invisible { get; } = new(0, 0, 0, 0);

	public static Rgba White { get; } = new(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public static Rgba Black { get; } = new(0, 0, 0, byte.MaxValue);

	public static Rgba HalfTransparentWhite { get; } = new(byte.MaxValue, byte.MaxValue, byte.MaxValue, 127);

	public static Rgba HalfTransparentBlack { get; } = new(0, 0, 0, 127);

	public static Rgba Red { get; } = new(byte.MaxValue, 0, 0, byte.MaxValue);

	public static Rgba Green { get; } = new(0, byte.MaxValue, 0, byte.MaxValue);

	public static Rgba Blue { get; } = new(0, 0, byte.MaxValue, byte.MaxValue);

	public static Rgba Yellow { get; } = new(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);

	public static Rgba Aqua { get; } = new(0, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public static Rgba Purple { get; } = new(byte.MaxValue, 0, byte.MaxValue, byte.MaxValue);

	public static Rgba Orange { get; } = new(byte.MaxValue, 127, 0, byte.MaxValue);

	public static implicit operator Vector3(Rgba color)
	{
		return new Vector3(color.R / (float)byte.MaxValue, color.G / (float)byte.MaxValue, color.B / (float)byte.MaxValue);
	}

	public static implicit operator Vector4(Rgba color)
	{
		return new Vector4(color.R / (float)byte.MaxValue, color.G / (float)byte.MaxValue, color.B / (float)byte.MaxValue, color.A / (float)byte.MaxValue);
	}

	public static Rgba Gray(float value)
	{
		if (value is < 0 or > 1)
			throw new ArgumentOutOfRangeException(nameof(value));

		byte component = (byte)(value * byte.MaxValue);
		return new Rgba(component, component, component, byte.MaxValue);
	}

	public static Rgba FromVector3(Vector3 vector)
	{
		return new Rgba((byte)(vector.X * byte.MaxValue), (byte)(vector.Y * byte.MaxValue), (byte)(vector.Z * byte.MaxValue), byte.MaxValue);
	}

	public static Rgba FromVector4(Vector4 vector)
	{
		return new Rgba((byte)(vector.X * byte.MaxValue), (byte)(vector.Y * byte.MaxValue), (byte)(vector.Z * byte.MaxValue), (byte)(vector.W * byte.MaxValue));
	}
}
