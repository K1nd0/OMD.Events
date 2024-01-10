using OMD.Events.Services;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Players.Connections.Events;
using System.Threading.Tasks;

namespace OMD.Events.Events.Listeners;

public sealed class ConnectionEvents(IEventsService eventsService) :
    IEventListener<UnturnedPlayerConnectedEvent>,
    IEventListener<UnturnedPlayerDisconnectedEvent>
{
    public Task HandleEventAsync(object? sender, UnturnedPlayerConnectedEvent @event)
    {
        eventsService.SubscribePlayer(@event.Player);

        return Task.CompletedTask;
    }

    public Task HandleEventAsync(object? sender, UnturnedPlayerDisconnectedEvent @event)
    {
        eventsService.UnsubscribePlayer(@event.Player);

        return Task.CompletedTask;
    }
}
