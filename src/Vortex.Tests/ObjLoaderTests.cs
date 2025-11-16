#region
using Vortex.Common.Obj;
using Xunit;
#endregion

namespace Vortex.Tests;

public sealed class ObjLoaderTests {
  [Fact]
  public async Task LoadAsync_ParsesCubeAsset() {
    var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
    var cubePath = Path.Combine(repoRoot, "src", "Vortex.Common", "Assets", "cube.obj");

    await using var stream = File.OpenRead(cubePath);
    var model = await ObjLoader.LoadAsync(stream);

    Assert.Equal(8, model.Positions.Count);
    Assert.Equal(4, model.TextureCoordinates.Count);
    Assert.Equal(6, model.Normals.Count);
    Assert.Equal(12, model.Faces.Count);

    var firstFace = model.Faces[0];
    Assert.Equal(3, firstFace.Vertices.Count);
    Assert.Equal(0, firstFace.Vertices[0].PositionIndex);
    Assert.Equal(1, firstFace.Vertices[1].PositionIndex);
    Assert.Equal(2, firstFace.Vertices[2].PositionIndex);
  }
}