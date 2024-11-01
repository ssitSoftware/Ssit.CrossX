using Ssit.CrossX.IoC.Impl;

namespace Ssit.CrossX.IoC;

public static class IoC
{
    /// <summary>
    /// Creates a new instance of IIoCContainerBuilder.
    /// </summary>
    /// <return>A new instance of IIoCContainerBuilder which can be used to configure and build an IoC container.</return>
    public static IIoCContainerBuilder NewBuilder() => new IoCContainerBuilder();
}