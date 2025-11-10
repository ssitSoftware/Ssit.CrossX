using System;
using System.Collections.Generic;
using Ssit.IoC;

namespace Ssit.CrossX.XxGames.Physics;

public class ObjectPool<TObject, TParameters> : IObjectPool<TObject, TParameters> where TObject : class, IBodyOwner, IDisposable, IParametersOwner<TParameters>
{
    private readonly IIoCContainer _iocContainer;
    private readonly List<TObject> _objects = new();

    public ObjectPool(IIoCContainer iocContainer)
    {
        _iocContainer = iocContainer;
    }
    
    public TObject Spawn(TParameters parameters)
    {
        for(var idx =0; idx < _objects.Count;)
        {
            var obj = _objects[idx];
            if (obj.Parameters.Equals(parameters))
            {
                _objects.RemoveAt(idx);
                obj.Body.ReattachToSimulation();
                return obj;
            }
        }
        return _iocContainer.IoCConstruct<TObject>(parameters);
    }
    
    public void Despawn(TObject obj)
    {
        obj.Body.DetachFromSimulation();
        _objects.Add(obj);
    }
}