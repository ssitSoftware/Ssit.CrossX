using System;

namespace Ssit.CrossX.UI.Values;

[Flags]
public enum ScrollMode
{
    None = 0,
    Horizontal = 1,
    Vertical = 2,
    Both = Horizontal | Vertical
}