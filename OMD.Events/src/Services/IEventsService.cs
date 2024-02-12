﻿using Microsoft.Extensions.Logging;
using OpenMod.API;
using OpenMod.API.Eventing;
using OpenMod.API.Ioc;
using OpenMod.Unturned.Users;
using System;

namespace OMD.Events.Services;

[Service]
public interface IEventsService : IDisposable
{
    IOpenModHost OpenModHost { get; }

    IEventBus EventBus { get; }

    IUnturnedUserDirectory UserDirectory { get; }

    ILogger<EventsService> Logger { get; }

    void Init();

    void Emit(IEvent @event);
}
