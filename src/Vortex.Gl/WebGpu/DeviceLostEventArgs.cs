namespace Vortex.Gl.WebGpu;

/// <summary>
///   Carries additional information for a lost WebGPU device.
/// </summary>
public sealed class DeviceLostEventArgs : EventArgs {
    /// <summary>
    ///   Initializes a new instance of <see cref="DeviceLostEventArgs" />.
    /// </summary>
    /// <param name="reason">The optional reason the device was lost.</param>
    public DeviceLostEventArgs(Exception? reason = null) => Reason = reason;

    /// <summary>
    ///   Gets the optional exception that caused the device loss.
    /// </summary>
    public Exception? Reason { get; }
}