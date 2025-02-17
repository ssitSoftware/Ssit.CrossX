using System.Numerics;

namespace Ssit.CrossX.Games.Logic.Map;

public class ObjectCreationParameters
{
    public Vector2 Position { get; internal set; }
    public bool Flipped { get; internal set; }
    
    internal virtual object ParametersObject
    {
        set
        {
        }
    }
}

public class ObjectCreationParameters<TParameters>: ObjectCreationParameters
    where TParameters : class
{
    public TParameters Parameters { get; private set; }

    internal override object ParametersObject
    {
        set =>  Parameters = (TParameters)value;
    }
}