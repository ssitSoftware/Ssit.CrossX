using System;

namespace Ssit.CrossX.Games.Logic.Map;

public interface ILinkMap
{
    void RequestLink<TLink>(int id, Action<TLink> action) where TLink : class;
}