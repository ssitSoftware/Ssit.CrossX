using ObjCRuntime;
using Ssit.Pixel.Framework.NET;
using UIKit;

namespace SampleGame;

public class Program
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        PixelAppRunner<GameApp>.Run();
    }
}