using System;

namespace Ssit.CrossX.XxGames.Platformer.Builders;

public interface ILinkMap
{
    void RequestLink<TLink>(int id, Action<TLink> action) where TLink : class;
}