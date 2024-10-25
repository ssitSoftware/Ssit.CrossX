using System;

namespace Ssit.Pixel.Content;

/// <summary>
/// Delegate to method, which loads resource from given path.
/// </summary>
public delegate IDisposable LoadResourceDelegate(string path);