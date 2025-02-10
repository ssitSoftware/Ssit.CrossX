using System;

namespace Ssit.CrossX.Core;

public interface IAppWindowManager
{
    event Action<WindowClosingEventArgs> Closing; 
    void Close();
    void SetFullscreen();
    void SetWindowed(Size size);
    void SetTitle(string title);
}