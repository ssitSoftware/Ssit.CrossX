using System.Numerics;
using Ssit.CrossX.Games.TopDown.Rendering;

namespace Ssit.CrossX.Games.TopDown;

public interface IGameObject
{
    IGameObjectRenderer Renderer => null;
    IGameObjectVertexRenderer VertexRenderer => null;
    Vector2 Position { get; }
}