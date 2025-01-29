using System.IO;
using Avalonia.Media.Imaging;
using SkiaSharp;

namespace Ssit.CrtossX.Editor.Service;

public interface IEditorBitmapsProvider
{
    SKImage[] MaterialsPreview { get; }
}