namespace Vortex.Controls;

/// <summary>
///   Arguments for FPS update events from <see cref="WebGpuCanvas" />.
/// </summary>
public sealed class FpsUpdatedEventArgs : EventArgs {
    /// <summary>
    ///   Initializes a new instance of <see cref="FpsUpdatedEventArgs" />.
    /// </summary>
    /// <param name="framesPerSecond">The current frames per second.</param>
    /// <param name="frameTimeMs">The time taken to render the last frame in milliseconds.</param>
    public FpsUpdatedEventArgs(double framesPerSecond, double frameTimeMs) {
    FramesPerSecond = framesPerSecond;
    FrameTimeMs = frameTimeMs;
  }

    /// <summary>
    ///   Gets the calculated frames per second.
    /// </summary>
    public double FramesPerSecond { get; }

    /// <summary>
    ///   Gets the time taken to render the last frame in milliseconds.
    /// </summary>
    public double FrameTimeMs { get; }
}