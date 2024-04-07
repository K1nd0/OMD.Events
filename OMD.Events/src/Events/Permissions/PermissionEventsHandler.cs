using HarmonyLib;
using OMD.Events.Models;
using OMD.Events.Services;
using OpenMod.API.Permissions;
using OpenMod.API.Users;
using OpenMod.Unturned.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OMD.Events.Permissions;

internal delegate void OnPermissionUpdatedHandler(IPermissionActor actor, string permission);

internal sealed class PermissionEventsHandler(IEventsService eventsService) : EventsHandler(eventsService)
{
    private static OnPermissionUpdatedHandler? OnPermissionAdded;

    private static OnPermissionUpdatedHandler? OnPermissionRemoved;

    private static readonly Lazy<Type[]> TargetTypes = new(() => {
        var targetType = typeof(IPermissionStore);
        var domainTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => {
            try { return assembly.GetTypes(); }
            catch (Exception) { return []; }
        });

        return domainTypes.Where(type => {
            try { return type != targetType && targetType.IsAssignableFrom(type); }
            catch (Exception) { return false; }
        }).ToArray();
    });

    public override void Subscribe()
    {
        OnPermissionAdded += Events_OnPermissionAdded;
        OnPermissionRemoved += Events_OnPermissionRemoved;
    }

    public override void Unsubscribe()
    {
        OnPermissionAdded -= Events_OnPermissionAdded;
        OnPermissionRemoved -= Events_OnPermissionRemoved;
    }

    private void Events_OnPermissionAdded(IPermissionActor actor, string permission)
    {
        var player = GetUnturnedPlayer(actor);
        var @event = new UnturnedPlayerPermissionAddedEvent(player, permission);

        Emit(@event);
    }

    private void Events_OnPermissionRemoved(IPermissionActor actor, string permission)
    {
        var player = GetUnturnedPlayer(actor);
        var @event = new UnturnedPlayerPermissionRemovedEvent(player, permission);

        Emit(@event);
    }

    private UnturnedPlayer GetUnturnedPlayer(IPermissionActor actor)
    {
        return EventsService.UserDirectory.FindUser(actor.Id, UserSearchMode.FindById)!.Player;
    }

    [HarmonyPatch]
    internal static class PermissionAddedPatches
    {
        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> FindTargetMethods()
        {
            return TargetTypes.Value.Select(type => type.GetMethod(nameof(IPermissionStore.AddGrantedPermissionAsync)));
        }

        [HarmonyPostfix]
        private static void AddPermissionPostfix(IPermissionActor actor, string permission)
        {
            OnPermissionAdded?.Invoke(actor, permission);
        }
    }

    [HarmonyPatch]
    internal static class PermissionRemovedPatches
    {
        [HarmonyTargetMethods]
        private static IEnumerable<MethodBase> FindTargetMethods()
        {
            return TargetTypes.Value.Select(type => type.GetMethod(nameof(IPermissionStore.RemoveGrantedPermissionAsync)));
        }

        [HarmonyPostfix]
        private static void RemovePermissionPostfix(IPermissionActor actor, string permission)
        {
            OnPermissionRemoved?.Invoke(actor, permission);
        }
    }
}
