using Foundation;
using SampleGame.Game;
using Ssit.CrossX.NET.Apple;
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
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}