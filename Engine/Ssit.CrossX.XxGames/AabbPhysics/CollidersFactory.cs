using System;
using Ssit.CrossX.XxGames.AabbPhysics.Colliders;
using Ssit.CrossX.XxGames.Physics;
using Ssit.CrossX.XxGames.Physics.Coliders;

namespace Ssit.CrossX.XxGames.AabbPhysics;

internal static class CollidersFactory
{
    public static ICollider Create<TCreationParameters>(TCreationParameters creationParameters)
    {
        switch(creationParameters)
        {
            case RectColliderCreationParameters rectColliderCreationParameters:
                return new RectCollider(rectColliderCreationParameters);
        }
        throw new NotSupportedException();
    }
}