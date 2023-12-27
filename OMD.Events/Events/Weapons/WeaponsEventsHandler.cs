using HarmonyLib;
using OMD.Events.Models;
using OMD.Events.Services;
using SDG.Unturned;

namespace OMD.Events.Weapons;

internal delegate void OnChangedMagazineHandler(UseableGun gun, ItemJar itemJar);

internal delegate void OnChangedFiremodeHandler(UseableGun gun, EFiremode firemode);

internal sealed class WeaponsEventsHandler(IEventsService eventsService) : EventsHandler(eventsService)
{
    private static event OnChangedMagazineHandler? OnChangedMagazine;

    private static event OnChangedFiremodeHandler? OnChangedFiremode;

    public override void Subscribe()
    {
        UseableGun.onBulletSpawned += Events_OnBulletSpawned;
        PlayerEquipment.OnInspectingUseable_Global += Events_OnInspectingUseable;
        OnChangedMagazine += Events_OnChangedMagazine;
        OnChangedFiremode += Events_OnChangedFiremode;
    }

    public override void Unsubscribe()
    {
        UseableGun.onBulletSpawned -= Events_OnBulletSpawned;
        PlayerEquipment.OnInspectingUseable_Global -= Events_OnInspectingUseable;
        OnChangedMagazine -= Events_OnChangedMagazine;
        OnChangedFiremode -= Events_OnChangedFiremode;
    }

    private void Events_OnBulletSpawned(UseableGun gun, BulletInfo bulletInfo)
    {
        var unturnedPlayer = GetUnturnedPlayer(gun.player);
        var @event = new UnturnedPlayerSpawnedBulletEvent(unturnedPlayer, gun, bulletInfo);

        Emit(@event);
    }

    private void Events_OnInspectingUseable(PlayerEquipment equipment)
    {
        var unturnedPlayer = GetUnturnedPlayer(equipment.player);
        var @event = new UnturnedPlayerInspectingWeaponEvent(unturnedPlayer);

        Emit(@event);
    }

    private void Events_OnChangedMagazine(UseableGun gun, ItemJar itemJar)
    {
        var unturnedPlayer = GetUnturnedPlayer(gun.player);
        var @event = new UnturnedPlayerChangedMagazineEvent(unturnedPlayer, gun, itemJar);

        Emit(@event);
    }

    private void Events_OnChangedFiremode(UseableGun gun, EFiremode firemode)
    {
        var unturnedPlayer = GetUnturnedPlayer(gun.player);
        var @event = new UnturnedPlayerChangedFiremodeEvent(unturnedPlayer, gun, firemode);

        Emit(@event);
    }

    [HarmonyPatch(typeof(UseableGun))]
    internal static class UseableGunPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(UseableGun.ReceiveAttachMagazine))]
        private static void AttachMagazinePostfix(UseableGun __instance, byte page, byte x, byte y)
        {
            var inventory = __instance.player.inventory;
            var index = inventory.getIndex(page, x, y);
            var magazine = inventory.getItem(page, index);

            OnChangedMagazine?.Invoke(__instance, magazine);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(UseableGun.ReceiveChangeFiremode))]
        private static void ChangeFiremodePostfix(UseableGun __instance, EFiremode newFiremode)
        {
            OnChangedFiremode?.Invoke(__instance, newFiremode);
        }
    }
}
