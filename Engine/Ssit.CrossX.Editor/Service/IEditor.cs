using System.Collections.Generic;
using System.Numerics;
using Ssit.CrossX.Editor.Helpers;
using Ssit.CrossX.Editor.ViewModels;
using SkiaSharp;
using Ssit.CrossX.XxFormats.Map;

namespace Ssit.CrossX.Editor.Service;

public interface IEditor
{
    void Redraw();
    MapLayer SelectedLayer { get; set; }
    Vector2 ScreenToMap(Vector2 position);
    Vector2 MapToScreen(Vector2 position);
    Vector2 Offset { get; set; }
    RectangleF DrawEditorImage(EditorImage image, bool flipped, Vector2 position, SKCanvas skCanvas, GRContext grContext, SKPaint skPaint);
    void EnsurePanInBounds();
    ZoomViewModel Zoom { get; }
    IList<SKImage> GetTileSetImages(GRContext context);
    void GetMapObjects(IList<MapObjectInfo> buffer, Vector2 screenPosition);
    int SelectedObject { get; set; }
    bool ShowMaterials { set; }
}