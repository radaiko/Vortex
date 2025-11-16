#region
using Vortex.Controls;
using Vortex.Gl.WebGpu;
#endregion

namespace Vortex.Examples;

/// <summary>
///   Example demonstrating how to load and render an OBJ model using WebGpuCanvas.
/// </summary>
internal class CubeRenderingExample {
  private static async Task Main(string[] args) {
    Console.WriteLine("=== Vortex WebGPU Canvas Example ===");
    Console.WriteLine("Loading cube model and initializing canvas...\n");

    // Create a device factory (in this case, using the software simulator).
    var deviceFactory = new SoftwareWebGpuDeviceFactory();

    // Create the canvas with dependency injection.
    var canvas = new WebGpuCanvas(deviceFactory);

    // Initialize the canvas and its WebGPU device.
    await canvas.InitializeAsync();
    Console.WriteLine("✓ Canvas initialized successfully.");

    // Enable the FPS counter.
    canvas.ShowFpsCounter = true;

    // Subscribe to FPS updates.
    canvas.FpsUpdated += (sender, e) => { Console.WriteLine($"  FPS: {e.FramesPerSecond:F1} | Frame Time: {e.FrameTimeMs:F2}ms"); };

    // Load the cube OBJ model from the embedded asset.
    var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    var cubePath = Path.Combine(repoRoot, "src", "Vortex.Common", "Assets", "cube.obj");

    if (!File.Exists(cubePath)) {
      Console.WriteLine($"✗ Error: Cube asset not found at {cubePath}");
      return;
    }

    await using var cubeStream = File.OpenRead(cubePath);
    await canvas.LoadModelAsync(cubeStream);
    Console.WriteLine("✓ Cube model loaded and uploaded to device.\n");

    // Start the render loop for 5 seconds.
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    canvas.StartRenderLoop(cts.Token);

    Console.WriteLine("Rendering for 5 seconds (showing FPS updates)...\n");

    try {
      await Task.Delay(5100, cts.Token);
    }
    catch (OperationCanceledException) {
      // Expected when the 5-second window closes.
    }

    // Stop the render loop and clean up.
    await canvas.StopRenderLoopAsync();
    await canvas.DisposeAsync();

    Console.WriteLine("\n✓ Example completed successfully.");
  }
}