using System;

namespace Ssit.Utils.Mvvm.Abstractions;

/// <summary>
/// Specifies constants defining which buttons to display on a message box.
/// </summary>
[Flags]
public enum MessageBoxButton
{
    None = 0,
    Ok = 1,
    Cancel = 2, 
    Yes = 4,
    No = 8
}