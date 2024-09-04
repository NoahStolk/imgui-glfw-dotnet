﻿using ImGuiGlfw.Sample.Utils;
using ImGuiNET;

namespace ImGuiGlfw.Sample.Ui;

public static class PerformanceWindow
{
	private static long _previousAllocatedBytes;

	public static void Render(PerformanceMeasurement performanceMeasurement)
	{
		if (ImGui.Begin("Performance"))
		{
			ImGui.SeparatorText("Rendering");

			ImGui.Text(Inline.Span($"{performanceMeasurement.Fps} FPS"));
			ImGui.Text(Inline.Span($"Frame time: {performanceMeasurement.FrameTime:0.0000} s"));

			ImGui.SeparatorText("Allocations");

			long allocatedBytes = GC.GetAllocatedBytesForCurrentThread();
			ImGui.Text(Inline.Span($"Allocated: {allocatedBytes:N0} bytes"));
			ImGui.Text(Inline.Span($"Since last update: {allocatedBytes - _previousAllocatedBytes:N0} bytes"));
			_previousAllocatedBytes = allocatedBytes;

			for (int i = 0; i < GC.MaxGeneration + 1; i++)
				ImGui.Text(Inline.Span($"Gen{i}: {GC.CollectionCount(i)} times"));

			ImGui.Text(Inline.Span($"Total memory: {GC.GetTotalMemory(false):N0} bytes"));
			ImGui.Text(Inline.Span($"Total pause duration: {GC.GetTotalPauseDuration().TotalSeconds:0.000} s"));
		}

		ImGui.End();
	}
}
