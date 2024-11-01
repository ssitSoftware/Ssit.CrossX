using System;

namespace Ssit.CrossX.Content;

public interface IInstanceCountingResource: IDisposable
{
    Action<Guid> AddUser {set;}
    Action<Guid> RemoveUser {set;}
}