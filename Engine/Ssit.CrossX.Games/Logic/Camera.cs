using System.Numerics;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Logic;

internal class Camera: ICamera
{
    public Vector2 LookAt => (_body?.Position ?? Vector2.Zero) + _offset;

    private Body _body;
    private Vector2 _offset;
    
    public void SetTarget(Body body, Vector2 offset)
    {
        _body = body;
        _offset = offset;
    }

    public void Update(float dt)
    {
        
    }
}