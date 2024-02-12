using OpenMod.Unturned.Events;
using OpenMod.Unturned.Players;

namespace OMD.Events.Permissions;

public abstract class UnturnedPlayerPermissionUpdatedEvent(UnturnedPlayer player, string permission) : UnturnedPlayerEvent(player)
{
    public string Permission { get; } = permission;
}
