using System;

namespace Ssit.CrossX.Core;

public interface IMessenger
{
    event Action<object> Message;
    void PostMessage(object message);
}