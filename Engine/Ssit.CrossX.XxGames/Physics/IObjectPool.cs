using System;

namespace Ssit.CrossX.XxGames.Physics;

public interface IObjectPool<TObject, TParameters> where TObject: class, IBodyOwner, IDisposable
{
    TObject Spawn(TParameters parameters);
}