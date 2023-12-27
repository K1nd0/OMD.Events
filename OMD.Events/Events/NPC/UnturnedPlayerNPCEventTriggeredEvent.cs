using OpenMod.Unturned.Events;
using OpenMod.Unturned.Players;

namespace OMD.Events.NPC;

public sealed class UnturnedPlayerNPCEventTriggeredEvent(UnturnedPlayer player, string eventId) : UnturnedPlayerEvent(player)
{
    public string EventId { get; } = eventId;
}
