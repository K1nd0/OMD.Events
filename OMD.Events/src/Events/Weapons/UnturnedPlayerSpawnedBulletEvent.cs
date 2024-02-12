using OpenMod.Unturned.Players;
using SDG.Unturned;

namespace OMD.Events.Weapons;

public sealed class UnturnedPlayerSpawnedBulletEvent(UnturnedPlayer player, UseableGun gun, BulletInfo bulletInfo) : UnturnedPlayerGunEvent(player, gun)
{
    public BulletInfo BulletInfo { get; } = bulletInfo;
}
