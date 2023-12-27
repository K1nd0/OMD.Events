using OpenMod.Unturned.Players;
using SDG.Unturned;

namespace OMD.Events.Weapons;

public sealed class UnturnedPlayerChangedMagazineEvent(UnturnedPlayer player, UseableGun gun, ItemJar itemJar) : UnturnedPlayerGunEvent(player, gun)
{
    public ItemJar ItemJar { get; } = itemJar;
}
