using ImGuiGlfw.Sample;
using ImGuiGlfw.Sample.Services;
using StrongInject;

using Owned<App> app = new Container().Resolve();
app.Value.Run();
