using CommunityToolkit.Mvvm.ComponentModel;
using Ssit.CrossX;

namespace Editor.ViewModels;

public class EditorViewModel: ObservableObject
{
    public RgbaColor BackgroundColor { get; } = RgbaColor.White;
}