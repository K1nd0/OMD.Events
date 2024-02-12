using OpenMod.Unturned.Players;
using SDG.Unturned;

namespace OMD.Events.Weapons;

public sealed class UnturnedPlayerChangedFiremodeEvent(UnturnedPlayer player, UseableGun gun, EFiremode firemode) : UnturnedPlayerGunEvent(player, gun)
{
    public EFiremode Firemode { get; } = firemode;
}
