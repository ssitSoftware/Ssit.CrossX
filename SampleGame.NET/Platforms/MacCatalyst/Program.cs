using Ssit.Pixel.Framework.Core;
using Ssit.Pixel.Framework.NET;

namespace SampleGame;

public class Program
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        PixelAppRunner<GameApp>.Run(new WindowParameters(1280, 720));
    }
}