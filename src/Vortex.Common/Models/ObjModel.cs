#region
using System.Numerics;
#endregion

namespace Vortex.Common.Models;

/// <summary>
///   Represents a parsed Wavefront OBJ model with vertices, texture coordinates, normals, and faces.
/// </summary>
public sealed class ObjModel {
    /// <summary>
    ///   Initializes a new instance of the <see cref="ObjModel" /> class.
    /// </summary>
    public ObjModel(
    IReadOnlyList<Vector3> positions,
    IReadOnlyList<Vector2> textureCoordinates,
    IReadOnlyList<Vector3> normals,
    IReadOnlyList<ObjFace> faces) {
    Positions = positions;
    TextureCoordinates = textureCoordinates;
    Normals = normals;
    Faces = faces;
  }

    /// <summary>
    ///   Gets the parsed positions.
    /// </summary>
    public IReadOnlyList<Vector3> Positions { get; }

    /// <summary>
    ///   Gets the parsed texture coordinates.
    /// </summary>
    public IReadOnlyList<Vector2> TextureCoordinates { get; }

    /// <summary>
    ///   Gets the parsed normals.
    /// </summary>
    public IReadOnlyList<Vector3> Normals { get; }

    /// <summary>
    ///   Gets the faces composing the mesh.
    /// </summary>
    public IReadOnlyList<ObjFace> Faces { get; }
}

/// <summary>
///   Represents one face within an OBJ model.
/// </summary>
public sealed class ObjFace {
    /// <summary>
    ///   Initializes a new instance of the <see cref="ObjFace" /> class.
    /// </summary>
    public ObjFace(IReadOnlyList<ObjVertexReference> vertices) {
    if (vertices is null) {
      throw new ArgumentNullException(nameof(vertices));
    }

    if (vertices.Count < 3) {
      throw new ArgumentException("Faces must have at least three vertices", nameof(vertices));
    }

    Vertices = vertices;
  }

    /// <summary>
    ///   Gets the vertex references composing the face.
    /// </summary>
    public IReadOnlyList<ObjVertexReference> Vertices { get; }
}

/// <summary>
///   Represents a reference to a position, normal, and/or texture coordinate within a face.
/// </summary>
public sealed class ObjVertexReference {
    /// <summary>
    ///   Initializes a new instance of the <see cref="ObjVertexReference" /> class.
    /// </summary>
    public ObjVertexReference(int positionIndex, int? textureCoordinateIndex, int? normalIndex) {
    PositionIndex = positionIndex;
    TextureCoordinateIndex = textureCoordinateIndex;
    NormalIndex = normalIndex;
  }

    /// <summary>
    ///   Gets the zero-based index of the position.
    /// </summary>
    public int PositionIndex { get; }

    /// <summary>
    ///   Gets the zero-based index of the texture coordinate, if provided.
    /// </summary>
    public int? TextureCoordinateIndex { get; }

    /// <summary>
    ///   Gets the zero-based index of the normal, if provided.
    /// </summary>
    public int? NormalIndex { get; }
}