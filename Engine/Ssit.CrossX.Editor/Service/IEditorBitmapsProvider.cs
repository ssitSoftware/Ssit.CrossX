using SkiaSharp;

namespace Ssit.CrossX.Editor.Service;

public interface IEditorBitmapsProvider
{
    SKImage[] MaterialsPreview { get; }
}