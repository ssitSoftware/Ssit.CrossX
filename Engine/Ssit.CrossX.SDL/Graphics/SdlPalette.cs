using SDL;
using Ssit.CrossX.Core;
using Ssit.CrossX.Graphics;
using Ssit.CrossX.SDL.Common;
using Ssit.CrossX.UI.Services;
using static SDL.SDL3;

namespace Ssit.CrossX.SDL.Graphics;

public unsafe class SdlPalette: ISdlPalette, IDisposable
{
    private readonly IPaletteSource _paletteSource;
    private readonly IActionScheduler _actionScheduler;
    public event Action OnPaletteChanged;
    private readonly RgbaColor[] _palette;

    public RgbaColor[] OriginalPalette { get; }
    public SdlHandle<SDL_Palette> PaletteHandle { get; }
    public SdlHandle<SDL_Palette> GlowPaletteHandle { get; }
    public bool HasGlowPalette => _paletteSource.HasGlowPalette;

    private bool _disposed;
    
    public SdlPalette(IPaletteSource paletteSource, IActionScheduler actionScheduler)
    {
        _paletteSource = paletteSource;
        _actionScheduler = actionScheduler;

        var palettePtr = SDL_CreatePalette(_paletteSource.OriginalPalette.Count);
        var glowPalettePtr = SDL_CreatePalette(_paletteSource.OriginalPalette.Count);
        _palette = new RgbaColor[_paletteSource.OriginalPalette.Count];
        OriginalPalette = _paletteSource.OriginalPalette.ToArray();
        
        PaletteHandle = new SdlHandle<SDL_Palette>(palettePtr);
        GlowPaletteHandle = new SdlHandle<SDL_Palette>(glowPalettePtr);
        
        UpdatePalette(_paletteSource.Palette, _paletteSource.GlowPalette);
        _paletteSource.OnPaletteChanged += PaletteSourceOnOnPaletteChanged; 
    }

    private void PaletteSourceOnOnPaletteChanged()
    {
        _actionScheduler.ExecuteOnMainThread(() =>
            {
                UpdatePalette(_paletteSource.Palette, _paletteSource.GlowPalette);
                OnPaletteChanged?.Invoke();
            }
        );
    }

    public void UpdatePalette(IReadOnlyList<RgbaColor> colors, IReadOnlyList<RgbaColor> glowColors)
    {
        if ( colors.Count != _palette.Length)
            throw new ArgumentException("Palette length is not equal to original palette length");

        for (var idx = 0; idx < colors.Count; idx++)
        {
            _palette[idx] = colors[idx];
        }
        
        fixed (RgbaColor* palettePtr = _palette)
        {
            SDL_Color* ptr = (SDL_Color*)palettePtr;
            SDL_SetPaletteColors(PaletteHandle.Pointer, ptr, 0, _palette.Length);
        }

        if (glowColors != null)
        {
            for (var idx = 0; idx < glowColors.Count; idx++)
            {
                _palette[idx] = glowColors[idx];
            }

            fixed (RgbaColor* palettePtr = _palette)
            {
                SDL_Color* ptr = (SDL_Color*)palettePtr;
                SDL_SetPaletteColors(GlowPaletteHandle.Pointer, ptr, 0, _palette.Length);
            }
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
        
        if (GlowPaletteHandle is not null && GlowPaletteHandle.Pointer != null)
        {
            SDL_DestroyPalette(GlowPaletteHandle.Pointer);
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
}