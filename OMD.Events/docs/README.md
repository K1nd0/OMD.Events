# OMD.Events
An Unturned OpenMod plugin which implements additional events and allows developers to easily implement their own events interface.

## How to install or update plugin
Install plugin with command: `openmod install OMD.Events`

## How to easily implement your own additional events 
- In progress...

## Implemeted events
#### OMD.Events.Weapons
- `UnturnedPlayerChangedFiremodeEvent` fired when player changed his weapons fire mode
	- `UnturnedPlayer Player` - player's instance
	- `UseableGun Gun` - weapon which player is holding
	- `EFireMode Firemode` - new weapons fire mode
- `UnturnedPlayerChangedMagazineEvent` fired when player changed his weapons magazine
    - `UnturnedPlayer Player` - player's instance
	- `UseableGun Gun` - weapon which player is holding
	- `ItemJar ItemJar` - item jar of new weapons magazine
- `UnturnedPlayerInspectingWeaponEvent` fired when player start to inspect his weapon
    - `UnturnedPlayer Player` - player's instance
	- `UseableGun Gun` - weapon which player is holding
- `UnturnedPlayerSpawnedBulletEvent` - fired when player shoot from his weapon
    - `UnturnedPlayer Player` - player's instance
	- `UseableGun Gun` - weapon which player is holding
	- `BulletInfo BulletInfo` - information about the fired bullet

#### OMD.Events.Useable
- `UnturnedPlayerUseableConsumedEvent` fired **when** player ate, drank or used any medicine on himself or other player
    - `UnturnedPlayer Player` - instigator's instance
	- `UnturnedPlayer Target` - instance of player on whom useable has been used
	- `ItemConsumeableAsset Asset` - asset info of useable which has been used
- `UnturnedPlayerUseableConsumingEvent` fired **before** player ate, drank or used any medicine on himself or other player
    - `UnturnedPlayer Player` - instigator's instance
	- `UnturnedPlayer Target` - instance of player on whom useable is going to be used
	- `ItemConsumeableAsset Asset` - asset info of useable which is going to be used
	- `bool IsCancelled` - property, which defines, does the execution of consuming is going to be terminated

#### OMD.Events.Permissions | Works only with `rocketmodIntegration:permissionSystem` set to `OpenMod` in `openmod.unturned.yaml` | Might change it in future
- `UnturnedPlayerPermissionAddedEvent` fired when player got a permission
	- `UnturnedPlayer Player` - player's instance
	- `string Permission` - permission which has been granted
- `UnturnedPlayerPermissionRemovedEvent` fired when player's permission has been taken away
	- `UnturnedPlayer Player` - player's instance
	- `string Permission` - permission which has been taken away

#### OMD.Events.NPC
- `UnturnedPlayerNPCEventTriggeredEvent` fired when player triggered NPC event
	- `UnturnedPlayer Player` - player's instance
	- `string EventId` - Id of triggered event