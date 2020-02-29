
# Sacrifice Remix
*This is my creative take on the original RoR1 Sacrifice artifact:*

Monsters will drop loot, spawn faster, and be more challenging earlier on - plus some other kick a** features. As a result, the gameplay is fast-paced and packed with consistent action. Although, unlike the original Sacrifice, all chests still spawn. I found it really boring to not have chests, so this simply adds extra layers to the game without taking anything away. However, if you want to disable chests, you can totally do that!

Also, please note that this module is a work in progress. My goal is to create intense Diablo-style gameplay, in which the added difficulty is balanced out by loot rewards and other fun mechanics. To get the best experience, I highly recommend installing the mods listed below in the *Recommended Modules* section.

## Features
- Monsters have a chance to drop a random white/green/red item.
- Faster spawn rates; there's rarely a dull moment even on stage 1.
- Difficult monsters spawn sooner and more frequently.
- Chance on kill to summon a temporary attack drone with random items; the item count is based on stages completed.
- Chest spawns are enabled by default; they can be disabled if you want classic RoR1 Sacrifice gameplay.
- Simple configurations to control spawn rates, drop rates, and which chests/other interactable objects spawn.

## Recommended Modules
To get the optimal gameplay experience, I highly recommend installing the following modules:
- [NoBossNoWait](https://thunderstore.io/package/mistername/NoBossNoWait/) *by mistername*
- [ShareSuite](https://thunderstore.io/package/FunkFrog-and-Sipondo/ShareSuite/) *by FunkFrog and Sipondo*
- [BiggerBazaar](https://thunderstore.io/package/MagnusMagnuson/BiggerBazaar/) *by MagnusMagnuson*

## Installation
- Install [BepInEx Mod Pack](https://thunderstore.io/package/bbepis/BepInExPack/) (if you haven't already)
- Install [R2API](https://thunderstore.io/package/tristanmcpherson/R2API/) (if you haven't already)
- Place the mod in the *Risk of Rain 2\BepInEx\plugins* folder

## Configurations
- Run the game once after installing the mod so the config file is created, and then close the game.
- Open SacrificeRemix.cfg in *Risk of Rain 2\BepInEx\config*.
- If your game is running you will need to close it before changes take effect, or you can run the in-game console command *sr_reload*.

## Change Log

1.1.0
- **New Features**
  - Faster spawn rates; there's rarely a dull moment even on stage 1.
  - Scale spawn intensity per additional player.
  - Chance on kill to spawn a temporary attack drone with random items (item count based on stages completed); in the future we may add a variety of drone types.
  - Added a sound effect for loot drops.
- **Misc**  
  - Now the Configuration file is rebuilt when a new Sacrifice Remix version is installed; the previous file is backed up in the same folder.  
  - Added console command *sr_reload* to reload configuration file while in-game.
  - Changes to the default spawn/drop rates.
- **Configurations**
  - IsModuleEnabled: enable or disable Sacrifice Remix functionality
  - BoostSpawnRates: enable or disable increased spawn rates
  - SpawnIntensity: replaces the previous config *MonsterSpawnDifficulty*
  - SpawnIntensityPerPlayer: scale the *SpawnIntensity* per additional player
  - SummonDroneChance: the chance to spawn a temporary attack drone on kill
  - CloversRerollDrops: now disabled by default (CloversRerollRarity is still enabled)
- **Bug Fixes**
  - Fixed an issue where loot drops could spawn on ally minion/turret positions; now they will always spawn on the player.

1.0.1-1.0.2
- Changes to the default drop rates.

1.0.0
- First release