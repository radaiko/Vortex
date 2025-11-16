namespace Vortex.Gl.WebGpu;

/// <summary>
///   Creates WebGPU devices in an asynchronous, platform-agnostic manner.
/// </summary>
public interface IWebGpuDeviceFactory {
    /// <summary>
    ///   Creates a new <see cref="IWebGpuDevice" />.
    /// </summary>
    /// <param name="cancellationToken">The token that can cancel device creation.</param>
    /// <returns>The initialized WebGPU device.</returns>
    ValueTask<IWebGpuDevice> CreateDeviceAsync(CancellationToken cancellationToken = default);
}