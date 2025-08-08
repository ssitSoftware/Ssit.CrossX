namespace Ssit.CrossX.SDL.Common;

public unsafe class SdlHandle<TResource>(TResource* pointer)
    where TResource : unmanaged
{
    public TResource* Pointer { get; private set; } = pointer;
    
    public void OnDisposed() => Pointer = null;
}