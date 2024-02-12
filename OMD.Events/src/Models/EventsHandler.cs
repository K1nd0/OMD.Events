using OMD.Events.Services;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Players;
using SDG.Unturned;

namespace OMD.Events.Models;

public abstract class EventsHandler
{
    public readonly IEventsService EventsService;

    protected EventsHandler(IEventsService eventsService)
    {
        EventsService = eventsService;
    }

    public abstract void Subscribe();

    public abstract void Unsubscribe();

    public UnturnedPlayer GetUnturnedPlayer(Player player) => EventsService.UserDirectory.GetUser(player).Player;

    public void Emit(IEvent @event) => EventsService.Emit(@event);
}
