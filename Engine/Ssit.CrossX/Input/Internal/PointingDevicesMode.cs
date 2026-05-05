using System;

namespace Ssit.CrossX.Input.Internal;

[Flags]
public enum PointingDevicesMode
{
    Disabled = 0,
    Mouse = 1,
    Touch = 2
}