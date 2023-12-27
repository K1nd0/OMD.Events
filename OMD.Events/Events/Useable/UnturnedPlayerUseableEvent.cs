using OpenMod.Unturned.Events;
using OpenMod.Unturned.Players;
using SDG.Unturned;

namespace OMD.Events.Useable;

public abstract class UnturnedPlayerUseableEvent(UnturnedPlayer instigator, UnturnedPlayer target, ItemConsumeableAsset asset) : UnturnedPlayerEvent(instigator)
{
    public UnturnedPlayer Target { get; } = target;

    public ItemConsumeableAsset Asset { get; } = asset;
}
