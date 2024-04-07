# OMD.Events
An OpenMod / Unturned plugin which implements additional events and allows developers to easily implement their own events interface. Created primarily for developers use.

[![Nuget](https://img.shields.io/nuget/v/OMD.Events)](https://www.nuget.org/packages/OMD.Events/)
[![Nuget](https://img.shields.io/nuget/dt/OMD.Events?label=nuget%20downloads)](https://www.nuget.org/packages/OMD.Events/)

# How to install
Run this command: `openmod install OMD.Events`

# How to easily implement your own additional events 
You can implement you custom handler classes by inheriting from [OMD.Events.Models.EventsHandler](https://github.com/DevInc0/OMD.Events/blob/master/OMD.Events/Models/EventsHandler.cs)

Here's an example on how to do so:

```cs
using OMD.Events.Models;
using OMD.Events.Services;
using SDG.Unturned;
using OpenMod.Unturned.Players;
using Action = System.Action;

namespace Your.Namespace;

internal sealed class YourCustomEventsHandler : EventsHandler
{
    private static event Action? SomeEventFromPatching;

    // Make sure you implement a constructor with IEventsService parameter and pass it to the base constructor
    internal YourCustomEventsHandler(IEventsService eventsService) : base(eventsService) { }

    public override void Subscribe()
    {
        // Subscribe to events you want
        SomeClass.SomeEvent += Events_Handler;
        SomeEventFromPatching += Events_PatchingHandler;
    }

    public override void Unsubscribe()
    {
        // Make sure you unsubscribe from them
        SomeClass.SomeEvent -= Events_Handler;
        SomeEventFromPatching -= Events_PatchingHandler;
    }

    private void Events_Handler(Player player) 
    {
        UnturnedPlayer unturnedPlayer = GetUnturnedPlayer(player); // get UnturnedPlayer instance from SDG.Unturned.Player one
        YourCustomEvent @event = new YourCustomEvent(unturnedPlayer);

        Emit(@event); // emit your event and handle it whereever you want
    }

    private void Events_PatchingHandler()
    {
        // Same idea as in Events_Handler method
    }

    // You can also use harmony patches
    // OpenMod will patch it automatically, if YourCustomEventsHandler is declared in plugin assembly
    [HarmonyPatch(typeof(TargetTypeToPatch))]
    private static class YourPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("TargetMethod")]
        private static void YourPrefix()
        {
             SomeEventFromPatching?.Invoke();
        }
    }
}

internal sealed class YourCustomEventListener : IEventListener<YourCustomEvent>
{
    // Now you can easily handle your event using OpenMod's event listeners
    // https://openmod.github.io/openmod-docs/devdoc/concepts/events.html
}
```

OMD.Events plugin will scan through all types in every loaded plugin. And call `Subscribe()` and `Unsubscribe()`, so you can focus on creating your plugin.

# Built-in events
#### OMD.Events.Weapons
- `UnturnedPlayerChangedFiremodeEvent` fired when player changed his weapons fire mode
	- `UnturnedPlayer Player` - player's instance
	- `UseableGun Gun` - weapon which player is holding
	- `EFireMode Firemode` - new weapons fire mode
- `UnturnedPlayerChangedMagazineEvent` fired when player changed his weapons magazine
    - `UnturnedPlayer Player` - player's instance
	- `UseableGun Gun` - weapon which player is holding
	- `ItemJar ItemJar` - item jar of new weapons magazine
- `UnturnedPlayerInspectingWeaponEvent` fired when player start to inspect his weapon
    - `UnturnedPlayer Player` - player's instance
	- `UseableGun Gun` - weapon which player is holding
- `UnturnedPlayerSpawnedBulletEvent` - fired when player shoot from his weapon
    - `UnturnedPlayer Player` - player's instance
	- `UseableGun Gun` - weapon which player is holding
	- `BulletInfo BulletInfo` - information about the fired bullet

#### OMD.Events.Useable
- `UnturnedPlayerUseableConsumedEvent` fired **when** player ate, drank or used any medicine on himself or other player
    - `UnturnedPlayer Player` - instigator's instance
	- `UnturnedPlayer Target` - instance of player on whom useable has been used
	- `ItemConsumeableAsset Asset` - asset info of useable which has been used
- `UnturnedPlayerUseableConsumingEvent` fired **before** player ate, drank or used any medicine on himself or other player
    - `UnturnedPlayer Player` - instigator's instance
	- `UnturnedPlayer Target` - instance of player on whom useable is going to be used
	- `ItemConsumeableAsset Asset` - asset info of useable which is going to be used
	- `bool IsCancelled` - property, which defines, does the execution of consuming is going to be terminated

#### OMD.Events.Permissions | Works only with `rocketmodIntegration:permissionSystem` set to `OpenMod` in `openmod.unturned.yaml` | Might change it in future
- `UnturnedPlayerPermissionAddedEvent` fired when player got a permission
	- `UnturnedPlayer Player` - player's instance
	- `string Permission` - permission which has been granted
- `UnturnedPlayerPermissionRemovedEvent` fired when player's permission has been taken away
	- `UnturnedPlayer Player` - player's instance
	- `string Permission` - permission which has been taken away

#### OMD.Events.NPC
- `UnturnedPlayerNPCEventTriggeredEvent` fired when player triggered NPC event
	- `UnturnedPlayer Player` - player's instance
	- `string EventId` - Id of triggered event
