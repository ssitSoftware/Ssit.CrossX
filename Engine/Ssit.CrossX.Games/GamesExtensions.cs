using Ssit.CrossX.Content;
using Ssit.CrossX.XxFormats;

namespace Ssit.CrossX.Games;

public static class GamesExtensions
{
    public static void RegisterGameContentTypes(this IContentManager contentManager, OriginAlignment alignment)
    {
        contentManager.RegisterLoader<GameObject>(path => new GameObject(path, contentManager, alignment));
    }
}