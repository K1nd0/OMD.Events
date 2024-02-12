using OpenMod.Unturned.Players;

namespace OMD.Events.Permissions;

public sealed class UnturnedPlayerPermissionRemovedEvent(UnturnedPlayer player, string permission) : UnturnedPlayerPermissionUpdatedEvent(player, permission) { }