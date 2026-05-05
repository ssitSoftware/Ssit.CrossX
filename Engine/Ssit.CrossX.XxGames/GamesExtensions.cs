using Ssit.CrossX.Content;
using Ssit.CrossX.Graphics;

namespace Ssit.CrossX.XxGames;

public static class GamesExtensions
{
    public static void RegisterGameContentTypes(this IContentManager contentManager, ContentAlign alignment)
    {
        contentManager.RegisterLoader<GameObject>(path => new GameObject(path, contentManager, alignment));
    }
}