using OMD.Events.Models;
using OMD.Events.Services;
using SDG.Unturned;

namespace OMD.Events.NPC;

internal sealed class NPCEventsHandler(IEventsService eventsService) : EventsHandler(eventsService)
{
    public override void Subscribe()
    {
        NPCEventManager.onEvent += OnEvent;
    }

    public override void Unsubscribe()
    {
        NPCEventManager.onEvent -= OnEvent;
    }

    private void OnEvent(Player instigatingPlayer, string eventId)
    {
        var player = GetUnturnedPlayer(instigatingPlayer);
        var @event = new UnturnedPlayerNPCEventTriggeredEvent(player, eventId);

        Emit(@event);
    }
}
