using System;

namespace Ssit.CrossX.Core;

public class WindowClosingEventArgs : EventArgs
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool Cancel { get; set; }
}