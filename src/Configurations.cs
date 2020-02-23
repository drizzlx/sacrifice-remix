﻿using BepInEx;
using BepInEx.Configuration;
using System.IO;

namespace SacrificeRemix
{
    class Configurations
    {
        private static Configurations instance;
        private ConfigFile srConfig;
        public string configFilePath = Paths.ConfigPath + "\\SacrificeRemix.cfg";
        // Sections
        public string SectionGeneral = "1.0 General";        
        public string SectionDrops = "2.0 Drops";
        public string SectionDropsNormal = "2.1 Drops.Normal";
        public string SectionDropsElite = "2.2 Drops.Elite";
        public string SectionDropsBoss = "2.3 Drops.Boss";
        public string SectionInteractables = "3.0 Interactables";
        public string SectionInteractablesChances = "3.1 Interactables.Chances";
        public string SectionDeveloper = "Developer";

        public ConfigEntry<string>
            ModuleVersion;

        public ConfigEntry<bool>
            // General
            IsModuleEnabled,
            BoostSpawnRates,
            // Developer
            IsDeveloperMode,
            // Drops
            CloversRerollDrops,
            CloversRerollRarity;

        public ConfigEntry<float>
            // General
            SpawnIntensity,
            SpawnIntensityPerPlayer,
            // Drops
            NormalDropChance,
            EliteDropChance,
            BossDropChance,
            // Drops.Normal
            NormalGreenItemChance,
            NormalRedItemChance,
            // Drops.Elite
            EliteGreenItemChance,
            EliteRedItemChance,
            // Drops.Boss
            BossGreenItemChance,
            BossRedItemChance,
            // Interactables
            InteractableSpawnMultiplier,
            InteractableCostMultiplier,
            // Interactables.Chances
            // - Chests
            ChestSmallChance,
            ChestLargeChance,
            ChestDamageChance,
            ChestHealingChance,
            ChestUtilityChance,
            ChestGoldChance,
            ChestLunarChance,
            ChestStealthedChance,
            EquipmentBarrelChance,
            TripleShopSmallChance,
            TripleShopLargeChance,
            BarrelChance,
            // - Shrines
            ShrineHealingChance,
            ShrineBloodChance,
            ShrineBossChance,
            ShrineChanceChance,
            ShrineCombatChance,
            ShrineRestackChance,
            ShrineGoldshoresAccessChance,
            // - Drones
            BrokenDrone1Chance,
            BrokenDrone2Chance,
            BrokenMegaDroneChance,
            BrokenMissileDroneChance,
            BrokenEquipmentDroneChance,
            BrokenFlameDroneChance,
            BrokenTurretChance,
            DuplicatorSmallChance,
            DuplicatorLargeChance,
            DuplicatorMilitaryChance,
            // - Other
            RadarTowerChance;

        public Configurations()
        {            
            Init();
        }
        public static Configurations Instance()
        {
            return instance ?? (instance = new Configurations());
        }

        public void Reload()
        {
            srConfig.Reload();
        }   
        
        private void CheckConfigVersion()
        {                
            if (!File.Exists(configFilePath) || File.ReadAllText(configFilePath).Contains("Version = " + SacrificeRemix.Version))
            {
                return;
            }

            string backupPath = configFilePath + ".backup";

            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }

            File.Copy(configFilePath, backupPath);
            File.Delete(configFilePath);
        }

        private void Init()
        {
            CheckConfigVersion();

            // Init config file
            srConfig = new ConfigFile(configFilePath, true);            
            // Developer            
            ModuleVersion = srConfig.Bind<string>(SectionDeveloper, "Version", SacrificeRemix.Version, "The configuration file version (do not change).");
            IsDeveloperMode = srConfig.Bind<bool>(SectionDeveloper, "IsDeveloperMode", false, "Enable custom logs for debugging.");
            // General
            IsModuleEnabled = srConfig.Bind<bool>(SectionGeneral, "IsModuleEnabled", true, "Enable or disable the module.");
            BoostSpawnRates = srConfig.Bind<bool>(SectionGeneral, "BoostSpawnRates", true, "Monsters spawn faster than usual; this is influenced by the SpawnIntensity multiplier.");
            SpawnIntensity = srConfig.Bind<float>(SectionGeneral, "SpawnIntensity", 1.5f, "A multiplier for the rate at which more difficult monsters spawn; 1 is the default rate.");
            SpawnIntensityPerPlayer = srConfig.Bind<float>(SectionGeneral, "SpawnIntensityPerPlayer", 0,
                "Scale SpawnIntensity for each additional player (0 to disable). " +
                "Example: SpawnIntensity 1 + 0.25 PerPlayer = 100%/125%/150%/175% with 1/2/3/4 players.");
            // Drops
            NormalDropChance = srConfig.Bind<float>(SectionDrops, "NormalDropChance", 3, "Percent chance for normal monsters to drop an item. 0 to disable.");
            EliteDropChance = srConfig.Bind<float>(SectionDrops, "EliteDropChance", 4, "Percent chance for elite monsters to drop an item. 0 to disable.");
            BossDropChance = srConfig.Bind<float>(SectionDrops, "BossDropChance", 8, "Percent chance for bosses to drop an item. 0 to disable.");
            CloversRerollDrops = srConfig.Bind<bool>(SectionDrops, "CloversRerollDrops", false, "Can clovers reroll the chance of an item dropping.");
            CloversRerollRarity = srConfig.Bind<bool>(SectionDrops, "CloversRerollRarity", true, "Can clovers reroll the rarity of an item that's dropping (e.g. increased chance for red or green).");
            // Drops.Normal            
            NormalGreenItemChance = srConfig.Bind<float>(SectionDropsNormal, "GreenItem", 25, "Percent chance for normal monsters to roll uncommon item.");
            NormalRedItemChance = srConfig.Bind<float>(SectionDropsNormal, "RedItem", 5, "Percent chance for normal monsters to roll legendary item.");
            // Drops.Elite
            EliteGreenItemChance = srConfig.Bind<float>(SectionDropsElite, "GreenItem", 44, "Percent chance for elite monsters to roll uncommon item.");
            EliteRedItemChance = srConfig.Bind<float>(SectionDropsElite, "RedItem", 6, "Percent chance for elite monsters to roll legendary item.");
            // Drops.Boss
            BossGreenItemChance = srConfig.Bind<float>(SectionDropsBoss, "GreenItem", 90, "Percent chance for bosses to roll uncommon item.");
            BossRedItemChance = srConfig.Bind<float>(SectionDropsBoss, "RedItem", 10, "Percent chance for bosses to roll legendary item.");
            // Interactables
            InteractableSpawnMultiplier = srConfig.Bind<float>(SectionInteractables, "InteractableSpawnMultiplier", 1, "A multiplier on the amount of interactables that will spawn in a level.");
            InteractableCostMultiplier = srConfig.Bind<float>(SectionInteractables, "InteractableCostMultiplier", 1, "A multiplier applied to the cost of all interactables.");
            // Interactables.Chances
            // - Chests
            ChestSmallChance = srConfig.Bind<float>(SectionInteractablesChances, "SmallChest", 1, "The multiplier for this item to spawn.");
            ChestLargeChance = srConfig.Bind<float>(SectionInteractablesChances, "LargeChest", 1, "The multiplier for this item to spawn.");
            ChestDamageChance = srConfig.Bind<float>(SectionInteractablesChances, "CategoryDamageChest", 1, "The multiplier for this item to spawn.");
            ChestHealingChance = srConfig.Bind<float>(SectionInteractablesChances, "CategoryHealingChest", 1, "The multiplier for this item to spawn.");
            ChestUtilityChance = srConfig.Bind<float>(SectionInteractablesChances, "CategoryUtilityChest", 1, "The multiplier for this item to spawn.");
            ChestGoldChance = srConfig.Bind<float>(SectionInteractablesChances, "GoldChest", 1, "The multiplier for this item to spawn.");
            ChestLunarChance = srConfig.Bind<float>(SectionInteractablesChances, "LunarChest", 1, "The multiplier for this item to spawn.");
            ChestStealthedChance = srConfig.Bind<float>(SectionInteractablesChances, "StealthedChest", 1, "The multiplier for this item to spawn.");
            EquipmentBarrelChance = srConfig.Bind<float>(SectionInteractablesChances, "EquipmentBarrel", 1, "The multiplier for this item to spawn.");
            TripleShopSmallChance = srConfig.Bind<float>(SectionInteractablesChances, "TripleShopSmall", 1, "The multiplier for this item to spawn.");
            TripleShopLargeChance = srConfig.Bind<float>(SectionInteractablesChances, "TripleShopLarge", 1, "The multiplier for this item to spawn.");
            BarrelChance = srConfig.Bind<float>(SectionInteractablesChances, "Barrel", 1, "The multiplier for this item to spawn.");
            // - Shrines
            ShrineHealingChance = srConfig.Bind<float>(SectionInteractablesChances, "ShrineHealing", 1, "The multiplier for this item to spawn.");
            ShrineBloodChance = srConfig.Bind<float>(SectionInteractablesChances, "ShrineBlood", 1, "The multiplier for this item to spawn.");
            ShrineBossChance = srConfig.Bind<float>(SectionInteractablesChances, "ShrineBoss", 1, "The multiplier for this item to spawn.");
            ShrineChanceChance = srConfig.Bind<float>(SectionInteractablesChances, "ShrineChance", 1, "The multiplier for this item to spawn.");
            ShrineCombatChance = srConfig.Bind<float>(SectionInteractablesChances, "ShrineCombat", 1, "The multiplier for this item to spawn.");
            ShrineRestackChance = srConfig.Bind<float>(SectionInteractablesChances, "ShrineRestack", 1, "The multiplier for this item to spawn.");
            ShrineGoldshoresAccessChance = srConfig.Bind<float>(SectionInteractablesChances, "ShrineGoldshoresAccess", 1, "The multiplier for this item to spawn.");
            // - Drones
            BrokenDrone1Chance = srConfig.Bind<float>(SectionInteractablesChances, "BrokenDrone1", 1, "The multiplier for this item to spawn.");
            BrokenDrone2Chance = srConfig.Bind<float>(SectionInteractablesChances, "BrokenDrone2", 1, "The multiplier for this item to spawn.");
            BrokenMegaDroneChance = srConfig.Bind<float>(SectionInteractablesChances, "BrokenMegaDrone", 1, "The multiplier for this item to spawn.");
            BrokenMissileDroneChance = srConfig.Bind<float>(SectionInteractablesChances, "BrokenMissileDrone", 1, "The multiplier for this item to spawn.");
            BrokenEquipmentDroneChance = srConfig.Bind<float>(SectionInteractablesChances, "BrokenEquipmentDrone", 1, "The multiplier for this item to spawn.");
            BrokenFlameDroneChance = srConfig.Bind<float>(SectionInteractablesChances, "BrokenFlameDrone", 1, "The multiplier for this item to spawn.");
            BrokenTurretChance = srConfig.Bind<float>(SectionInteractablesChances, "BrokenTurret", 1, "The multiplier for this item to spawn.");
            // - Duplicators
            DuplicatorSmallChance = srConfig.Bind<float>(SectionInteractablesChances, "DuplicatorSmall", 1, "The multiplier for this item to spawn.");
            DuplicatorLargeChance = srConfig.Bind<float>(SectionInteractablesChances, "DuplicatorLarge", 1, "The multiplier for this item to spawn.");
            DuplicatorMilitaryChance = srConfig.Bind<float>(SectionInteractablesChances, "DuplicatorMilitary", 1, "The multiplier for this item to spawn.");
            // - Other
            RadarTowerChance = srConfig.Bind<float>(SectionInteractablesChances, "RadarTower", 1, "The multiplier for this item to spawn.");
        }
    }
}
