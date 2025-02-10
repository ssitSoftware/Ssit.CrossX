namespace Ssit.CrossX.SDL3.Common;

public unsafe class SdlHandle<TResource>(TResource* pointer)
    where TResource : unmanaged
{
    public TResource* Pointer { get; } = pointer;
}