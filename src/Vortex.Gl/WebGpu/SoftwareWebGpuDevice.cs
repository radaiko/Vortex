#region
using Vortex.Common.Models;
#endregion

namespace Vortex.Gl.WebGpu;

/// <summary>
///   A lightweight simulated WebGPU device used for examples and tests.
/// </summary>
public sealed class SoftwareWebGpuDevice : IWebGpuDevice {
  private ObjModel? _currentlyUploadedModel;

  /// <summary>
  ///   Gets the total number of frames rendered since the last upload.
  /// </summary>
  public int RenderCount { get; private set; }

  /// <summary>
  ///   Gets the rotation applied in the most recent render.
  /// </summary>
  public float LastRotationRadians { get; private set; }

  /// <inheritdoc />
  public event EventHandler<DeviceLostEventArgs>? DeviceLost;

  /// <inheritdoc />
  public ValueTask InitializeAsync(CancellationToken cancellationToken = default) {
    RenderCount = 0;
    LastRotationRadians = 0;
    return ValueTask.CompletedTask;
  }

  /// <inheritdoc />
  public ValueTask UploadModelAsync(ObjModel model, CancellationToken cancellationToken = default) {
    _currentlyUploadedModel = model ?? throw new ArgumentNullException(nameof(model));
    RenderCount = 0;
    return ValueTask.CompletedTask;
  }

  /// <inheritdoc />
  public ValueTask RenderFrameAsync(ObjModel model, float rotationRadians, CancellationToken cancellationToken = default) {
    if (_currentlyUploadedModel is null) {
      throw new InvalidOperationException("No model uploaded before rendering.");
    }

    if (!ReferenceEquals(model, _currentlyUploadedModel)) {
      throw new InvalidOperationException("Rendered model must match uploaded reference.");
    }

    cancellationToken.ThrowIfCancellationRequested();

    RenderCount++;
    LastRotationRadians = MathF.IEEERemainder(rotationRadians, MathF.Tau);
    return ValueTask.CompletedTask;
  }
}