using System;
using Ssit.CrossX.Games.Physics.Dynamics;

namespace Ssit.CrossX.Games.Physics.Extensions;

public class MovingStackExtension: IDisposable
{
    private readonly Body _body;

    public static void Attach(Body body)
    {
        body.SetExtension(new MovingStackExtension(body));
    }
    
    private MovingStackExtension(Body body)
    {
        _body = body;
    }
    
    public void Dispose()
    {
    }
}