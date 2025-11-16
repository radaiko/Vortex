#region
using Vortex.Common.Models;
#endregion

namespace Vortex.Gl.WebGpu;

/// <summary>
///   Represents an initialized WebGPU device able to render loaded models.
/// </summary>
public interface IWebGpuDevice {
    /// <summary>
    ///   Performs async initialization work.
    /// </summary>
    ValueTask InitializeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///   Uploads model data to the GPU simulator.
    /// </summary>
    ValueTask UploadModelAsync(ObjModel model, CancellationToken cancellationToken = default);

    /// <summary>
    ///   Renders the uploaded model with the current rotation.
    /// </summary>
    ValueTask RenderFrameAsync(ObjModel model, float rotationRadians, CancellationToken cancellationToken = default);

    /// <summary>
    ///   Triggered when the device is lost and resources must be discarded.
    /// </summary>
    event EventHandler<DeviceLostEventArgs>? DeviceLost;
}