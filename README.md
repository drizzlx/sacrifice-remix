
# Sacrifice Remix

### Let's be friends: [https://discord.gg/UubUgCh](http://bit.ly/drizzlx-discord)

## Tidbits:
- ### It is recommended to install the gameplay mods listed below; see *Recommended Modules* section.
- ### Default gameplay is balanced for *Monsoon* difficulty
- ### Feedback is SUPER helpful! Join the [Discord](http://bit.ly/drizzlx-discord) :)
- ### Open source on [GitHub](http://bit.ly/sacrifice-remix-github)

## About

*This is my creative take on the original RoR1 Sacrifice artifact:*

Monsters will drop loot, spawn faster, and be more challenging earlier on - plus some other kick a** features. As a result, the gameplay is fast-paced and packed with consistent action.

Unlike the original Sacrifice, all chests still spawn by default. This simply adds extra layers to the game without taking anything away. However, if you want to disable chests, you can totally do that! It's super customizable and can fit any play style.

Also, please keep in mind that this module is a work in progress. My goal is to create intense *Diablo-like* gameplay, in which the added difficulty is balanced out by kill rewards and other fun mechanics. New features will continue to be added, and the default gameplay experience may change.


*<3 drizzlx*

## Features

- Monsters have a chance to drop a random white/green/red item.
- Faster spawn rates; there's hordes of mobs to hack and slash.
- Difficult monsters spawn more frequently.
- Chance on kill to summon a temporary attack drone with random items; the duration, item limit, and damage increases each stage.
- Chest spawns are enabled by default, but can be disabled if you want classic RoR1 Sacrifice gameplay.
- Highly customizable so it can be modified to fit any play style.

## Recommended Modules
To get the optimal gameplay experience, I highly recommend installing the following modules:

- **[NoBossNoWait](https://thunderstore.io/package/mistername/NoBossNoWait/)**
- **[ShareSuite](https://thunderstore.io/package/FunkFrog-and-Sipondo/ShareSuite/)**
- [BiggerBazaar](https://thunderstore.io/package/MagnusMagnuson/BiggerBazaar/)
- [BalancedObliterate](https://thunderstore.io/package/mistername/BalancedObliterate/)
- [RTAutoSprintEx](https://thunderstore.io/package/JohnEdwa/RTAutoSprintEx/)
- [LunarCoinShareOnPickup](https://thunderstore.io/package/dan8991iel/LunarCoinShareOnPickup/)

## Installation
- Install [BepInEx Mod Pack](https://thunderstore.io/package/bbepis/BepInExPack/) (if you haven't already)
- Install [R2API](https://thunderstore.io/package/tristanmcpherson/R2API/) (if you haven't already)
- Place the mod in the *Risk of Rain 2\BepInEx\plugins* folder

## Configurations
- Run the game once after installing the mod so the config file is created, and then close the game.
- Open SacrificeRemix.cfg in *Risk of Rain 2\BepInEx\config*.
- If your game is running you will need to close it before changes take effect, or you can run the in-game console command *sr_reload*.

## Change Log

### 1.1.0

- #### New Features
- Much faster spawn rates.
- Scale spawn intensity per additional player.
- Chance on kill to spawn a temporary attack drone with random items (the duration, item limit, and damage increase each stage).
- Added a sound effect for loot drops.
- #### Misc  
- Now the Configuration file is rebuilt when a new Sacrifice Remix version is installed; the previous file is backed up in the same folder.  
- Added console command *sr_reload* to reload configuration file while in-game.
- Changes to the default spawn/drop rates.
- Reworked custom logic for spawn rate and difficulty, so that it scales based on stages complete.
- #### Configurations
- IsModuleEnabled: enable or disable Sacrifice Remix functionality
- BoostSpawnRates: enable or disable increased spawn rates
- SpawnIntensity: replaces the previous config *MonsterSpawnDifficulty*
- SpawnIntensityPerPlayer: scale the *SpawnIntensity* per additional player
- SummonDroneChance: the chance to spawn a temporary attack drone on kill
- CloversRerollDrops: now disabled by default (CloversRerollRarity is still enabled)
- #### Bug Fixes
- Fixed an issue where loot drops could spawn on ally minion/turret positions; now they will always spawn on the player.

### 1.0.1-1.0.2

- Changes to the default drop rates.

### 1.0.0

- First release