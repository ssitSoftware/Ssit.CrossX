using Ssit.CrossX.IO;

namespace Ssit.CrossX.Sounds.Retro
{
    public class RetroSoundEffects
    {
        private class SourceClass : IAssetsSource
        {
            public IFilesProvider FilesProvider { get; } = new EmbeddedFilesProvider(typeof(RetroSoundEffects).Assembly, "Ssit.CrossX.Sounds.Retro.Assets");
            public string DriveName => "RetroSounds:";
        }

        public static class Set1
        {
            public const string ButtonPush = "RetroSounds:/Set1/ButtonPush.wav";
            public const string ButtonRelease = "RetroSounds:/Set1/ButtonRelease.wav";
            public const string Navigate = "RetroSounds:/Set1/Navigate.wav";
            public const string NavigateBack = "RetroSounds:/Set1/NavigateBack.wav";
            public const string ItemNavigation = "RetroSounds:/Set1/ItemNavigation.wav";
        }

        public static readonly IAssetsSource Source = new SourceClass();
    }
}