using System;

namespace Ssit.CrossX.Core;

public interface IAppLifetimeManager
{
    event Action AppExiting; 
    void Exit();
}