using OpenMod.Unturned.Players;
using SDG.Unturned;

namespace OMD.Events.Useable;

public sealed class UnturnedPlayerUseableConsumedEvent(UnturnedPlayer instigator, UnturnedPlayer target, ItemConsumeableAsset asset) : UnturnedPlayerUseableEvent(instigator, target, asset) { }
