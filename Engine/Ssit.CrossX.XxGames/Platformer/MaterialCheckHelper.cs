using Ssit.CrossX.XxGames.Physics;

namespace Ssit.CrossX.XxGames.Platformer;

internal class MaterialCheckHelper
{
    public static bool MatchMaterial(IMaterial material, IMaterial[] okMaterials, IMaterial[] noMaterials)
    {
        bool matchMaterial = false;

        if (okMaterials != null)
        {
            foreach (var mat in okMaterials)
            {
                if (mat == material)
                {
                    matchMaterial = true;
                    break;
                }
            }
        }

        if (!matchMaterial && noMaterials != null)
        {
            matchMaterial = true;
            foreach (var mat in noMaterials)
            {
                if (mat == material)
                {
                    matchMaterial = false;
                    break;
                }
            }
        }
        return matchMaterial;
    }
}