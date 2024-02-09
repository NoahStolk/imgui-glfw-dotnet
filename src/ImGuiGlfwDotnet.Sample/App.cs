using ImGuiGlfwDotnet.Ui;
using ImGuiNET;
using Silk.NET.OpenGL;

namespace ImGuiGlfwDotnet.Sample;

public sealed class App
{
	private const float _maxMainDelta = 0.25f;

	private const double _updateRate = 60;
	private const double _mainLoopRate = 300;

	private const double _updateLength = 1 / _updateRate;
	private const double _mainLoopLength = 1 / _mainLoopRate;

	private readonly ImGuiController _imGuiController;
	private readonly PerformanceMeasurement _performanceMeasurement = new();

	private double _currentTime = Graphics.Glfw.GetTime();
	private double _accumulator;
	private double _frameTime;

	public App(ImGuiController imGuiController)
	{
		_imGuiController = imGuiController;
	}

	public unsafe void Run()
	{
		while (!Graphics.Glfw.WindowShouldClose(Graphics.Window))
		{
			double expectedNextFrame = Graphics.Glfw.GetTime() + _mainLoopLength;
			MainLoop();

			while (Graphics.Glfw.GetTime() < expectedNextFrame)
				Thread.Yield();
		}

		_imGuiController.Destroy();
		Graphics.Glfw.Terminate();
	}

	private unsafe void MainLoop()
	{
		double mainStartTime = Graphics.Glfw.GetTime();
		_frameTime = mainStartTime - _currentTime;
		if (_frameTime > _maxMainDelta)
			_frameTime = _maxMainDelta;

		_performanceMeasurement.Update(mainStartTime, _frameTime);

		_currentTime = mainStartTime;
		_accumulator += _frameTime;

		Graphics.Glfw.PollEvents();

		while (_accumulator >= _updateLength)
			_accumulator -= _updateLength;

		Render();

		Graphics.Glfw.SwapBuffers(Graphics.Window);
	}

	private void Render()
	{
		_imGuiController.Update((float)_frameTime);

		ImGui.DockSpaceOverViewport(null, ImGuiDockNodeFlags.PassthruCentralNode);

		Graphics.Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		bool temp = true;
		ImGui.ShowDemoWindow();
		InputWindow.Render(ref temp);
		InputDebugWindow.Render(ref temp);
		PerformanceWindow.Render(ref temp, _performanceMeasurement);
		SettingsWindow.Render(ref temp, Graphics.Glfw);

		_imGuiController.Render();

		GlfwInput.PostRender();
	}
}
