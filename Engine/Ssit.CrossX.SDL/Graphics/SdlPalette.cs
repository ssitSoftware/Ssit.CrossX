using bottlenoselabs.Interop;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.SDL.Common;

using static bottlenoselabs.Interop.SDL;

namespace Ssit.CrossX.SDL.Graphics;

public unsafe class SdlPalette: ISdlPalette, IDisposable
{
    private readonly IPaletteSource _paletteSource;
    public event Action OnPaletteChanged;
    private readonly RgbaColor[] _palette;

    public RgbaColor[] OriginalPalette { get; }
    public SdlHandle<SDL_Palette> PaletteHandle { get; }
     
    private bool _disposed;
    
    public SdlPalette(IPaletteSource paletteSource)
    {
        _paletteSource = paletteSource;
        
        var palettePtr = SDL_CreatePalette(_paletteSource.OriginalPalette.Count);
        _palette = new RgbaColor[_paletteSource.OriginalPalette.Count];
        OriginalPalette = _paletteSource.OriginalPalette.ToArray();
        
        PaletteHandle = new SdlHandle<SDL_Palette>(palettePtr);
        
        UpdatePalette(_paletteSource.Palette);
        
        _paletteSource.OnPaletteChanged += PaletteSourceOnOnPaletteChanged; 
    }

    private void PaletteSourceOnOnPaletteChanged()
    {
        UpdatePalette(_paletteSource.Palette);
        OnPaletteChanged?.Invoke();
    }

    public void UpdatePalette(IReadOnlyList<RgbaColor> colors)
    {
        if ( colors.Count != _palette.Length)
            throw new ArgumentException("Palette length is not equal to original palette length");

        for (var idx = 0; idx < colors.Count; idx++)
        {
            _palette[idx] = colors[idx];
        }
        
        fixed (RgbaColor* palettePtr = _palette)
        {
            Rgba8U* ptr = (Rgba8U*)palettePtr;
            SDL_SetPaletteColors(PaletteHandle.Pointer, ptr, 0, _palette.Length);
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        
        _paletteSource.OnPaletteChanged -= PaletteSourceOnOnPaletteChanged;

        if (PaletteHandle is not null && PaletteHandle.Pointer != null)
        {
            SDL_DestroyPalette(PaletteHandle.Pointer);
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}