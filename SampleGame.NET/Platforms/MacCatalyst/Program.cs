using Foundation;
using Ssit.Pixel.NET;
using UIKit;

namespace SampleGame;

[Register("AppDelegate")]
public class AppDelegate : PixelDelegate<GameApp>
{
}

public class Program
{
    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        //PixelAppRunner<GameApp>.Run(new WindowParameters(1280, 720));
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}