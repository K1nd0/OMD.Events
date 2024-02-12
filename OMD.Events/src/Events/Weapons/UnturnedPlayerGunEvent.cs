using OpenMod.Unturned.Events;
using OpenMod.Unturned.Players;
using SDG.Unturned;

namespace OMD.Events.Weapons;

public abstract class UnturnedPlayerGunEvent(UnturnedPlayer player, UseableGun gun) : UnturnedPlayerEvent(player)
{
    public UseableGun Gun { get; } = gun;
}
