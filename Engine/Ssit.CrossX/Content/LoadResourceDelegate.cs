using System;

namespace Ssit.CrossX.Content;

/// <summary>
/// Delegate to method, which loads resource from given path.
/// </summary>
public delegate IDisposable LoadResourceDelegate(string path);