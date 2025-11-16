#region
using Vortex.Controls;
using Vortex.Gl.WebGpu;
using Xunit;
#endregion

namespace Vortex.Tests;

/// <summary>
///   Tests for the WebGpuCanvas control including initialization, model loading, and rendering.
/// </summary>
public sealed class WebGpuCanvasTests {
  [Fact]
  public async Task InitializeAsync_CreatesAndInitializesDevice() {
    var factory = new SoftwareWebGpuDeviceFactory();
    var canvas = new WebGpuCanvas(factory);

    await canvas.InitializeAsync();

    // If we got here without exception, initialization succeeded.
    Assert.NotNull(canvas);
  }

  [Fact]
  public async Task LoadModelAsync_LoadsCubeAsset() {
    var factory = new SoftwareWebGpuDeviceFactory();
    var canvas = new WebGpuCanvas(factory);
    await canvas.InitializeAsync();

    var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    var cubePath = Path.Combine(repoRoot, "src", "Vortex.Common", "Assets", "cube.obj");

    await using var stream = File.OpenRead(cubePath);
    await canvas.LoadModelAsync(stream);

    // If we got here without exception, the model loaded successfully.
    Assert.NotNull(canvas);
  }

  [Fact]
  public async Task StartRenderLoop_RendersSeveralFrames() {
    var factory = new SoftwareWebGpuDeviceFactory();
    var canvas = new WebGpuCanvas(factory);
    await canvas.InitializeAsync();

    var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    var cubePath = Path.Combine(repoRoot, "src", "Vortex.Common", "Assets", "cube.obj");

    await using var stream = File.OpenRead(cubePath);
    await canvas.LoadModelAsync(stream);

    var fpsUpdateCount = 0;
    canvas.FpsUpdated += (sender, e) => {
      fpsUpdateCount++;
      Assert.True(e.FramesPerSecond >= 0);
      Assert.True(e.FrameTimeMs >= 0);
    };

    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
    canvas.StartRenderLoop(cts.Token);

    try {
      await Task.Delay(2100, cts.Token);
    }
    catch (OperationCanceledException) {
      // Expected.
    }

    await canvas.StopRenderLoopAsync();
    await canvas.DisposeAsync();

    // Should have at least one FPS update in 2 seconds.
    Assert.True(fpsUpdateCount >= 1);
  }

  [Fact]
  public async Task ShowFpsCounter_CanToggle() {
    var factory = new SoftwareWebGpuDeviceFactory();
    var canvas = new WebGpuCanvas(factory);

    Assert.False(canvas.ShowFpsCounter);

    canvas.ShowFpsCounter = true;
    Assert.True(canvas.ShowFpsCounter);

    canvas.ShowFpsCounter = false;
    Assert.False(canvas.ShowFpsCounter);

    await canvas.DisposeAsync();
  }
}