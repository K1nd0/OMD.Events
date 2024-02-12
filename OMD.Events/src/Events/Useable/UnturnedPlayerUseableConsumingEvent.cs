using OpenMod.API.Eventing;
using OpenMod.Unturned.Players;
using SDG.Unturned;

namespace OMD.Events.Useable;

public sealed class UnturnedPlayerUseableConsumingEvent(UnturnedPlayer instigator, UnturnedPlayer target, ItemConsumeableAsset asset) : UnturnedPlayerUseableEvent(instigator, target, asset), ICancellableEvent
{
    public bool IsCancelled { get; set; } = false;
}
