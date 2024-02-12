using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OMD.Events.Models;
using OpenMod.API;
using OpenMod.API.Eventing;
using OpenMod.API.Ioc;
using OpenMod.Core.Helpers;
using OpenMod.Unturned.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace OMD.Events.Services;

[PluginServiceImplementation(Lifetime = ServiceLifetime.Singleton)]
public sealed class EventsService(ILogger<EventsService> logger, IServiceProvider serviceProvider) : IEventsService
{
    public IOpenModHost OpenModHost { get; } = serviceProvider.GetRequiredService<IOpenModHost>();

    public IEventBus EventBus { get; } = serviceProvider.GetRequiredService<IEventBus>();

    public IUnturnedUserDirectory UserDirectory { get; } = serviceProvider.GetRequiredService<IUnturnedUserDirectory>();

    public ILogger<EventsService> Logger { get; } = logger;

    private List<EventsHandler> EventsHandlers { get; } = [];

    public void Init()
    {
        const BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        var targetTypes = FindEventHandlerTypes();

        foreach (var type in targetTypes)
        {
            try
            {
                var eventsHandler = Activator.CreateInstance(type, BindingFlags, Type.DefaultBinder, [this], CultureInfo.InvariantCulture) as EventsHandler
                    ?? throw new ArgumentNullException($"Could not create an instance of {type.FullName} as events handler");

                eventsHandler.Subscribe();

                EventsHandlers.Add(eventsHandler);

                Logger.LogInformation("Successfully created events handler instance of {typeName}",
                    type.Name);
            }
            catch (ArgumentNullException exception)
            {
                Logger.LogError(exception, "There's an exception during events handler initialization!");
            }
        }
    }

    public void Dispose()
    {
        foreach (var eventHandler in EventsHandlers)
            eventHandler.Unsubscribe();
    }

    public void Emit(IEvent @event)
    {
        AsyncHelper.RunSync(() => EventBus.EmitAsync(OpenModHost, this, @event));
    }

    private IEnumerable<Type> FindEventHandlerTypes()
    {
        var baseType = typeof(EventsHandler);

        return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => {
            try { return assembly.GetTypes(); }
            catch (Exception) { return []; }
        }).Where(type => {
            try { return !type.IsAbstract && baseType.IsAssignableFrom(type); }
            catch (Exception) { return false; }
        });
    }
}
