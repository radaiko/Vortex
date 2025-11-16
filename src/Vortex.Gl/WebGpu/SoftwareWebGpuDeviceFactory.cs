namespace Vortex.Gl.WebGpu;

/// <summary>
///   A simple factory that creates <see cref="SoftwareWebGpuDevice" /> instances.
/// </summary>
public sealed class SoftwareWebGpuDeviceFactory : IWebGpuDeviceFactory {
    /// <inheritdoc />
    public ValueTask<IWebGpuDevice> CreateDeviceAsync(CancellationToken cancellationToken = default) {
    var device = new SoftwareWebGpuDevice();
    return new ValueTask<IWebGpuDevice>(device);
  }
}