# Vortex WebGPU Engine

A cross-platform C# implementation of WebGPU with 3D rendering support for MAUI, Avalonia, and Blazor. Vortex provides standards-compliant WebGPU functionality with simplified APIs for loading and rendering OBJ models.

## Project Structure

- **Vortex.Common**: Shared utilities including the OBJ loader, math libraries, and asset definitions.
- **Vortex.Gl**: Core WebGPU implementation with device abstractions and rendering pipelines.
- **Vortex.Controls**: Platform-agnostic `WebGpuCanvas` control with FPS tracking and render loop management.
- **Vortex.Examples**: Demonstration projects showing how to load and render OBJ models.
- **Vortex.Tests**: Unit tests for the loader, canvas, and rendering logic.

## Quick Start

### Loading and Rendering an OBJ Model

```csharp
using Vortex.Controls;
using Vortex.Gl.WebGpu;

// Create a device factory
var deviceFactory = new SoftwareWebGpuDeviceFactory();

// Initialize the canvas
var canvas = new WebGpuCanvas(deviceFactory);
await canvas.InitializeAsync();

// Enable FPS counter
canvas.ShowFpsCounter = true;

// Subscribe to FPS updates
canvas.FpsUpdated += (sender, e) =>
{
    Console.WriteLine($"FPS: {e.FramesPerSecond:F1}");
};

// Load an OBJ model
await using var stream = File.OpenRead("assets/cube.obj");
await canvas.LoadModelAsync(stream);

// Start rendering (runs continuously, rotating the model)
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
canvas.StartRenderLoop(cts.Token);

// Wait for rendering to complete
await Task.Delay(10000);

// Clean up
await canvas.StopRenderLoopAsync();
await canvas.DisposeAsync();
```

## Features

- **OBJ Loader**: Asynchronous parsing of Wavefront OBJ files with support for positions, normals, and texture coordinates.
- **WebGPU Canvas**: Platform-agnostic 3D rendering control with async initialization and render loop management.
- **FPS Counter**: Built-in frames-per-second tracking with event-based updates.
- **Rotation Animation**: Models automatically rotate for visual demonstration.
- **Dependency Injection**: Clean DI patterns for device factories and canvas instantiation.
- **Comprehensive Tests**: Unit tests covering OBJ loading, canvas initialization, rendering, and FPS tracking.

## Building and Testing

### Build the solution:
```bash
dotnet build Vortex.slnx
```

### Run tests:
```bash
dotnet test Vortex.slnx
```

### Run the example:
```bash
dotnet run --project src/Vortex.Examples/Vortex.Examples.csproj
```

## OBJ Loading

The `ObjLoader` asynchronously parses OBJ files and populates:
- **Positions**: Vertex positions (X, Y, Z)
- **Texture Coordinates**: UV coordinates (U, V)
- **Normals**: Vertex normals for lighting calculations
- **Faces**: Triangulated face definitions with vertex references

Example:
```csharp
using Vortex.Common.Obj;

var model = await ObjLoader.LoadAsync(stream);

Console.WriteLine($"Vertices: {model.Positions.Count}");
Console.WriteLine($"Normals: {model.Normals.Count}");
Console.WriteLine($"UVs: {model.TextureCoordinates.Count}");
Console.WriteLine($"Faces: {model.Faces.Count}");
```

## Canvas API

The `WebGpuCanvas` control provides:

- **InitializeAsync()**: Initializes the WebGPU device asynchronously.
- **LoadModelAsync(stream)**: Loads an OBJ model from a stream.
- **StartRenderLoop(cancellationToken)**: Begins continuous rendering with automatic rotation.
- **StopRenderLoopAsync()**: Gracefully stops the render loop.
- **ShowFpsCounter**: Property to toggle FPS display (platform adapters implement the display).
- **FpsUpdated**: Event fired when FPS is recalculated (~1 second intervals).

## Architecture

Vortex follows clean architecture principles:

- **Platform-Agnostic Core**: `Vortex.Common` and `Vortex.Gl` contain no platform-specific code.
- **Control Abstraction**: `WebGpuCanvas` is platform-agnostic; adapters in `Vortex.Controls` (future) will derive for MAUI, Avalonia, and Blazor.
- **Async-First**: All I/O and initialization is fully asynchronous using `async`/`await`.
- **Graceful Error Handling**: Device loss events and cancellation are handled cleanly.
- **Dependency Injection**: Controls accept factories via constructor DI for testability.

## Sample Asset

A `cube.obj` asset is included in `Vortex.Common/Assets/` for testing and demonstration purposes.

## Future Enhancements

- Real WebGPU device implementations for native platforms (Win32, Metal, Vulkan).
- Platform-specific adapters (MAUI, Avalonia, Blazor).
- Advanced rendering features (texturing, shaders, lighting).
- Performance optimizations for large models.

