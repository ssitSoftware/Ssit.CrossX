using RetroGunslinger.Core;
using Ssit.CrossX.Editor;
using Ssit.CrossX.SDL;

public class Program
{
    public static int Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "editor")
        {
            EditorRunner.Run(args.Skip(1).ToArray(), new GameTemplate());
        }
        else
        {
            AppRunner<GameApp>.Run();
        }
        return 0;
    }
}

