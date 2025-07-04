using System.Collections.Generic;
using Godot;
using KentingStation.Interface;
using Player = KentingStation.Entity.Instance.Player;

namespace KentingStation.Common;

public class OnlinePlayers : IUpdatingGroup<CharacterBody2D>
{
    private readonly HashSet<Player> _onlinePlayers;

    private OnlinePlayers()
    {
        _onlinePlayers = new HashSet<Player>();
    }

    public static OnlinePlayers Singleton { get; } = new();

    public IReadOnlyCollection<CharacterBody2D> GetUpdatedCharacters()
    {
        return _onlinePlayers;
    }

    public static void RegisterPlayer(Player newOnlinePlayer)
    {
        Singleton._onlinePlayers.Add(newOnlinePlayer);
    }

    public static void UnregisterPlayer(Player newOnlinePlayer)
    {
        Singleton._onlinePlayers.Remove(newOnlinePlayer);
    }
}