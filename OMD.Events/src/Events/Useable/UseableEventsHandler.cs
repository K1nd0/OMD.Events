using HarmonyLib;
using OMD.Events.Models;
using OMD.Events.Services;
using SDG.Unturned;

namespace OMD.Events.Useable;

internal delegate void OnUsingConsumeableHandler(Player nativeInstigator, Player nativeTarget, ItemConsumeableAsset asset, ref bool isCancelled);

internal delegate void OnUsedConsumeableHandler(Player nativeInstigator, Player nativeTarget, ItemConsumeableAsset asset);

internal sealed class UseableEventsHandler(IEventsService eventsService) : EventsHandler(eventsService)
{
    private static event OnUsingConsumeableHandler? OnUsingConsumeable;

    private static event OnUsedConsumeableHandler? OnUsedConsumeable;

    public override void Subscribe()
    {
        OnUsingConsumeable += Events_OnUsingConsumeable;
        OnUsedConsumeable += Events_OnConsumePerformed;
    }

    public override void Unsubscribe()
    {
        OnUsingConsumeable -= Events_OnUsingConsumeable;
        OnUsedConsumeable -= Events_OnConsumePerformed;
    }

    private void Events_OnUsingConsumeable(Player nativeInstigator, Player nativeTarget, ItemConsumeableAsset asset, ref bool isCancelled)
    {
        var instigator = GetUnturnedPlayer(nativeInstigator);
        var target = GetUnturnedPlayer(nativeTarget);
        var @event = new UnturnedPlayerUseableConsumingEvent(instigator, target, asset) {
            IsCancelled = isCancelled
        };

        Emit(@event);

        isCancelled = @event.IsCancelled;
    }

    private void Events_OnConsumePerformed(Player nativeInstigator, Player nativeTarget, ItemConsumeableAsset asset)
    {
        var instigator = GetUnturnedPlayer(nativeInstigator);
        var target = GetUnturnedPlayer(nativeTarget);
        var @event = new UnturnedPlayerUseableConsumedEvent(instigator, target, asset);

        Emit(@event);
    }

    [HarmonyPatch(typeof(UseableConsumeable))]
    private static class UseableConsumeablePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("performUseOnSelf")]
        private static bool PerformOnSelfPrefix(UseableConsumeable __instance, ItemConsumeableAsset asset)
        {
            var isCancelled = false;

            OnUsingConsumeable?.Invoke(__instance.player, __instance.player, asset, ref isCancelled);

            if (isCancelled)
                __instance.player.equipment.dequip();

            return !isCancelled;
        }

        [HarmonyPrefix]
        [HarmonyPatch("performAid")]
        private static bool PerformAidPrefix(UseableConsumeable __instance, ItemConsumeableAsset asset, Player ___enemy)
        {
            var isCancelled = false;

            OnUsingConsumeable?.Invoke(__instance.player, ___enemy, asset, ref isCancelled);

            if (isCancelled)
                __instance.player.equipment.dequip();

            return !isCancelled;
        }

        [HarmonyPostfix]
        [HarmonyPatch("performUseOnSelf")]
        private static void PerformOnSelfPostfix(UseableConsumeable __instance, ItemConsumeableAsset asset, bool __runOriginal)
        {
            if (!__runOriginal)
                return;

            OnUsedConsumeable?.Invoke(__instance.player, __instance.player, asset);
        }

        [HarmonyPostfix]
        [HarmonyPatch("performAid")]
        private static void PerformAidPostfix(UseableConsumeable __instance, ItemConsumeableAsset asset, Player ___enemy, bool __runOriginal)
        {
            if (!__runOriginal)
                return;

            OnUsedConsumeable?.Invoke(__instance.player, ___enemy, asset);
        }
    }
}
