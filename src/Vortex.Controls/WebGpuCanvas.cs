#region
using System.Diagnostics;
using Vortex.Common.Models;
using Vortex.Common.Obj;
using Vortex.Gl.WebGpu;
#endregion

namespace Vortex.Controls;

/// <summary>
///   A platform-agnostic WebGPU canvas that handles rendering, FPS tracking, and model loading.
/// </summary>
public sealed class WebGpuCanvas {
  private readonly IWebGpuDeviceFactory _deviceFactory;
  private IWebGpuDevice? _device;
  private ObjModel? _model;
  private CancellationTokenSource? _renderLoopCts;
  private Task? _renderLoopTask;
  private float _rotationRadians;
  private bool _showFpsCounter;
  private Stopwatch? _frameTimer;
  private int _frameCount;

  /// <summary>
  ///   Initializes a new instance of <see cref="WebGpuCanvas" />.
  /// </summary>
  /// <param name="deviceFactory">Factory to create WebGPU devices via DI.</param>
  public WebGpuCanvas(IWebGpuDeviceFactory deviceFactory) {
    _deviceFactory = deviceFactory ?? throw new ArgumentNullException(nameof(deviceFactory));
    _rotationRadians = 0;
    _showFpsCounter = false;
    _frameCount = 0;
  }

  /// <summary>
  ///   Gets or sets whether to display the FPS counter.
  /// </summary>
  public bool ShowFpsCounter {
    get => _showFpsCounter;
    set {
      if (_showFpsCounter != value) {
        _showFpsCounter = value;
        OnShowFpsCounterChanged();
      }
    }
  }

  /// <summary>
  ///   Initializes the canvas and WebGPU device asynchronously.
  /// </summary>
  /// <param name="cancellationToken">Token to cancel initialization.</param>
  public async ValueTask InitializeAsync(CancellationToken cancellationToken = default) {
    _device = await _deviceFactory.CreateDeviceAsync(cancellationToken).ConfigureAwait(false);
    await _device.InitializeAsync(cancellationToken).ConfigureAwait(false);
    _frameTimer = Stopwatch.StartNew();
  }

  /// <summary>
  ///   Loads an OBJ model from the given stream and uploads it to the device.
  /// </summary>
  /// <param name="stream">The stream containing OBJ data.</param>
  /// <param name="cancellationToken">Token to cancel the load operation.</param>
  public async ValueTask LoadModelAsync(Stream stream, CancellationToken cancellationToken = default) {
    if (_device is null) {
      throw new InvalidOperationException("Canvas must be initialized before loading models.");
    }

    _model = await ObjLoader.LoadAsync(stream).ConfigureAwait(false);
    await _device.UploadModelAsync(_model, cancellationToken).ConfigureAwait(false);
  }

  /// <summary>
  ///   Starts the render loop with continuous frame updates.
  /// </summary>
  /// <param name="cancellationToken">Token to stop the render loop.</param>
  public void StartRenderLoop(CancellationToken cancellationToken = default) {
    if (_renderLoopTask is not null) {
      throw new InvalidOperationException("Render loop is already running.");
    }

    _renderLoopCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    _renderLoopTask = RenderLoopAsync(_renderLoopCts.Token);
  }

  /// <summary>
  ///   Stops the active render loop.
  /// </summary>
  public async ValueTask StopRenderLoopAsync() {
    if (_renderLoopCts is null || _renderLoopTask is null) {
      return;
    }

    _renderLoopCts.Cancel();
    try {
      await _renderLoopTask.ConfigureAwait(false);
    }
    catch (OperationCanceledException) {
      // Expected when cancellation is requested.
    }
    finally {
      _renderLoopCts.Dispose();
      _renderLoopCts = null;
      _renderLoopTask = null;
    }
  }

  /// <summary>
  ///   Disposes resources and stops the render loop.
  /// </summary>
  public async ValueTask DisposeAsync() {
    await StopRenderLoopAsync().ConfigureAwait(false);
    _frameTimer?.Stop();
  }

  /// <summary>
  ///   Triggered when the FPS is updated (typically every ~1 second).
  /// </summary>
  public event EventHandler<FpsUpdatedEventArgs>? FpsUpdated;

  private async Task RenderLoopAsync(CancellationToken cancellationToken) {
    if (_device is null || _model is null) {
      throw new InvalidOperationException("Device and model must be initialized before rendering.");
    }

    _frameTimer?.Restart();
    var lastFpsReportTime = _frameTimer?.Elapsed.TotalMilliseconds ?? 0;

    while (!cancellationToken.IsCancellationRequested) {
      var frameStartTime = _frameTimer?.Elapsed.TotalMilliseconds ?? 0;

      // Rotate the model slightly each frame.
      _rotationRadians += MathF.PI / 180f; // 1 degree per frame

      try {
        await _device.RenderFrameAsync(_model, _rotationRadians, cancellationToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException) {
        break;
      }

      var frameElapsedMs = (_frameTimer?.Elapsed.TotalMilliseconds ?? 0) - frameStartTime;
      _frameCount++;

      var currentTime = _frameTimer?.Elapsed.TotalMilliseconds ?? 0;
      var timeSinceLastReport = currentTime - lastFpsReportTime;

      // Report FPS every ~1 second.
      if (timeSinceLastReport >= 1000) {
        var fps = _frameCount / (timeSinceLastReport / 1000.0);
        FpsUpdated?.Invoke(this, new FpsUpdatedEventArgs(fps, frameElapsedMs));
        _frameCount = 0;
        lastFpsReportTime = currentTime;
      }

      // Yield control to avoid blocking.
      await Task.Yield();
    }
  }

  private void OnShowFpsCounterChanged() {
    // This is a hook for platform-specific implementations to toggle FPS display.
    // In a real scenario, derived/adapted classes would use this to update UI.
  }
}