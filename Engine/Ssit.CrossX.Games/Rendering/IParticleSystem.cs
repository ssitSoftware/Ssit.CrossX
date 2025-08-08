using System.Numerics;
using Ssit.CrossX.Graphics.Renderer;

namespace Ssit.CrossX.Games.Rendering;

public interface IParticleSystem
{
    int RequestContextId();
    
    void Draw(IRenderer2 renderer, int context);
    
    void AddParticle(int context, int particleGroupId, Vector2 position, Vector2 direction,  Vector2 gravity, float speed, float timeToLive);
    IParticleSystem RegisterParticleGroup(int id, string image, Size size, float minScale);
}