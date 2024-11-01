namespace Ssit.CrossX.Core;

public class WindowParameters: ParametersBase
{
    private int _width = 800;
    private int _height = 600;
    private bool _allowResize = true;
    private bool _fullScreen;

    public int Width
    {
        get => _width;
        set => SetField(ref _width, value);
    }

    public int Height
    {
        get => _height;
        set => SetField(ref _height, value);
    }

    public bool AllowResize
    {
        get => _allowResize;
        set => SetField(ref _allowResize, value);
    }

    public bool FullScreen
    {
        get => _fullScreen;
        set => SetField(ref _fullScreen, value);
    }

    public WindowParameters()
    {
        
    }

    public WindowParameters(int width, int height, bool allowResize = true, bool fullScreen = false)
    {
        _width = width;
        _height = height;
        _allowResize = allowResize;
        _fullScreen = fullScreen;
    }
}