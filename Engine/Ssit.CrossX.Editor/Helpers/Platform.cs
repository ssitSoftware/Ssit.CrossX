using System.Runtime.InteropServices;

namespace Ssit.CrossX.Editor.Helpers
{
    public static class Platform
    {
        public static bool ShowMenu => !RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
}