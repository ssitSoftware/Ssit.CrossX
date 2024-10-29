using System;

namespace Ssit.Pixel.Content;

public interface IInstanceCountingResource: IDisposable
{
    Action<Guid> AddUser {set;}
    Action<Guid> RemoveUser {set;}
}