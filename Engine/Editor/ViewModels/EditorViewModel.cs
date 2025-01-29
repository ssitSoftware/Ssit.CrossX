using System;
using System.Numerics;
using Editor.Input;
using SkiaSharp;
using Ssit.CrossX;
using Ssit.CrossX.Games;

namespace Editor.ViewModels;

public class EditorViewModel: BindableModel, ISkRenderer, IPointerHandler
{
    private readonly IGameTemplate _gameTemplate;
    public RgbaColor BackgroundColor => _gameTemplate.PreviewBackgroundColor;

    void IPointerHandler.OnMouseMove(MouseInputInfo input)
    {
    }

    void IPointerHandler.OnButtonDown(MouseInputInfo input)
    {
    }

    void IPointerHandler.OnButtonUp(MouseInputInfo input)
    {
    }

    void IPointerHandler.OnMouseLeave(MouseInputInfo input)
    {
    }

    bool IPointerHandler.OnMouseWheel(MouseInputInfo input)
    {
        return false;
    }

    public EditorViewModel(IGameTemplate gameTemplate)
    {
        _gameTemplate = gameTemplate;
    }

    public event Action RedrawNeeded;
    
    public Vector2 Size { get; set;ś }
    
    public void Render(SKCanvas skCanvas, GRContext grContext, int width, int height)
    {
        throw new NotImplementedException();
    }

    public void UnloadResources()
    {
        throw new NotImplementedException();
    }
}