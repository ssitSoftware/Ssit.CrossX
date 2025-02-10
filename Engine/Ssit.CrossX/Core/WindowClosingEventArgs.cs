using System;

namespace Ssit.CrossX.Core;

public class WindowClosingEventArgs : EventArgs
{
    public bool Cancel { get; set; }
}