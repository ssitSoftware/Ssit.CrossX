using System.Runtime.InteropServices;

namespace Ssit.CrtossX.Editor.Helpers
{
    public static class Platform
    {
        public static bool ShowMenu => !RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
}