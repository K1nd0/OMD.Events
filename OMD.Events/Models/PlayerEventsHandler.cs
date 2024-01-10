using OMD.Events.Services;
using OpenMod.Unturned.Players;

namespace OMD.Events.Models;

public abstract class PlayerEventsHandler(IEventsService eventsService) : EventsHandler(eventsService)
{
    public abstract void SubscribePlayer(UnturnedPlayer player);

    public abstract void UnsubscribePlayer(UnturnedPlayer player);
}
