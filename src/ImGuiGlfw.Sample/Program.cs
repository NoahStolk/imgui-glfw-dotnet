using ImGuiGlfw.Sample;
using StrongInject;

Container container = new();
Owned<App> app = container.Resolve();
app.Value.Run();
