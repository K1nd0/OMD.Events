using OpenMod.Unturned.Players;

namespace OMD.Events.Permissions;

public sealed class UnturnedPlayerPermissionAddedEvent(UnturnedPlayer player, string permission) : UnturnedPlayerPermissionUpdatedEvent(player, permission) { }
