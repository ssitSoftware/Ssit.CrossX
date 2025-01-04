using Ssit.CrossX.Content;

namespace Ssit.CrossX.Games;

public static class GamesExtensions
{
    public static void RegisterGameContentTypes(this IContentManager contentManager)
    {
        contentManager.RegisterLoader<GameObject>(path => new GameObject(path, contentManager));
    }
}