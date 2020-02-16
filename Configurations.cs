using BepInEx;
using BepInEx.Configuration;

namespace SacrificeRemix
{
    class Configurations
    {
        // Custom configuration file
        private static ConfigFile SrConfig { get; set; }
        // General
        public static ConfigEntry<float> MonsterSpawnDifficulty { get; set; }       
        // Chances
        public static ConfigEntry<float> NormalDropChance { get; set; }
        public static ConfigEntry<float> EliteDropChance { get; set; }
        public static ConfigEntry<float> BossDropChance { get; set; }
        public static ConfigEntry<bool> CloversRerollDrops { get; set; }
        public static ConfigEntry<bool> CloversRerollRarity { get; set; }
        // Chances.Normal
        public static ConfigEntry<float> NormalWhiteItemChance { get; set; }
        public static ConfigEntry<float> NormalGreenItemChance { get; set; }
        public static ConfigEntry<float> NormalRedItemChance { get; set; }
        // Chances.Elite
        public static ConfigEntry<float> EliteWhiteItemChance { get; set; }
        public static ConfigEntry<float> EliteGreenItemChance { get; set; }
        public static ConfigEntry<float> EliteRedItemChance { get; set; }
        // Chances.Boss
        public static ConfigEntry<float> BossWhiteItemChance { get; set; }
        public static ConfigEntry<float> BossGreenItemChance { get; set; }
        public static ConfigEntry<float> BossRedItemChance { get; set; }
        // Interactables
        public static ConfigEntry<float> InteractableSpawnMultiplier { get; set; }
        public static ConfigEntry<float> InteractableCostMultiplier { get; set; }    
        // Interactables.Chances
        public static ConfigEntry<float> ChestSmallChance { get; set; }
        public static ConfigEntry<float> ChestLargeChance { get; set; }
        public static ConfigEntry<float> ChestDamageChance { get; set; }
        public static ConfigEntry<float> ChestHealingChance { get; set; }
        public static ConfigEntry<float> ChestUtilityChance { get; set; }
        public static ConfigEntry<float> ChestGoldChance { get; set; }
        public static ConfigEntry<float> ChestLunarChance { get; set; }
        public static ConfigEntry<float> ChestStealthedChance { get; set; }
        public static ConfigEntry<float> EquipmentBarrelChance { get; set; }
        public static ConfigEntry<float> TripleShopSmallChance { get; set; }
        public static ConfigEntry<float> TripleShopLargeChance { get; set; }
        public static ConfigEntry<float> BarrelChance { get; set; }
        public static ConfigEntry<float> ShrineHealingChance { get; set; }
        public static ConfigEntry<float> ShrineBloodChance { get; set; }
        public static ConfigEntry<float> ShrineBossChance { get; set; }
        public static ConfigEntry<float> ShrineChanceChance { get; set; }
        public static ConfigEntry<float> ShrineCombatChance { get; set; }
        public static ConfigEntry<float> ShrineRestackChance { get; set; }
        public static ConfigEntry<float> ShrineGoldshoresAccessChance { get; set; }
        public static ConfigEntry<float> BrokenDrone1Chance { get; set; }
        public static ConfigEntry<float> BrokenDrone2Chance { get; set; }
        public static ConfigEntry<float> BrokenMegaDroneChance { get; set; }
        public static ConfigEntry<float> BrokenMissileDroneChance { get; set; }
        public static ConfigEntry<float> BrokenEquipmentDroneChance { get; set; }
        public static ConfigEntry<float> BrokenFlameDroneChance { get; set; }
        public static ConfigEntry<float> BrokenTurretChance { get; set; }
        public static ConfigEntry<float> DuplicatorSmallChance { get; set; }
        public static ConfigEntry<float> DuplicatorLargeChance { get; set; }
        public static ConfigEntry<float> DuplicatorMilitaryChance { get; set; }
        // Other
        public static ConfigEntry<float> RadarTowerChance { get; set; }

        public Configurations()
        {
            SrConfig = new ConfigFile(Paths.ConfigPath + "\\SacrificeRemix.cfg", true);

            // Sections
            var SectionGeneral = "1.0 General";
            var SectionChances = "2.0 Chances";
            var SectionChancesNormal = "2.1 Chances.Normal";
            var SectionChancesElite = "2.2 Chances.Elite";
            var SectionChancesBoss = "2.3 Chances.Boss";            
            var SectionInteractables = "3.0 Interactables";
            var SectionInteractablesChances = "3.1 Interactables.Chances";

            // General
            MonsterSpawnDifficulty = SrConfig.Bind<float>(SectionGeneral, "MonsterSpawnDifficulty", 1.5f, "A periodic multiplier to scale the monster spawn difficulty. Default is 1. Example: 1.5 = 50% more difficult.");
            // Chances
            NormalDropChance = SrConfig.Bind<float>(SectionChances, "NormalDropChance", 3, "Percent chance for normal monsters to drop an item. 0 to disable.");
            EliteDropChance = SrConfig.Bind<float>(SectionChances, "EliteDropChance", 5, "Percent chance for elite monsters to drop an item. 0 to disable.");
            BossDropChance = SrConfig.Bind<float>(SectionChances, "BossDropChance", 7, "Percent chance for bosses to drop an item. 0 to disable.");
            CloversRerollDrops = SrConfig.Bind<bool>(SectionChances, "CloversRerollDrops", true, "Can clovers reroll the chance of an item dropping.");
            CloversRerollRarity = SrConfig.Bind<bool>(SectionChances, "CloversRerollRarity", true, "Can clovers reroll the rarity of an item that's dropping (e.g. increased chance for red or green).");
            // Chances.Normal            
            NormalGreenItemChance = SrConfig.Bind<float>(SectionChancesNormal, "GreenItem", 25, "Percent chance for normal monsters to roll uncommon item.");
            NormalRedItemChance = SrConfig.Bind<float>(SectionChancesNormal, "RedItem", 5, "Percent chance for normal monsters to roll legendary item.");
            // Chances.Elite
            EliteGreenItemChance = SrConfig.Bind<float>(SectionChancesElite, "GreenItem", 45, "Percent chance for elite monsters to roll uncommon item.");
            EliteRedItemChance = SrConfig.Bind<float>(SectionChancesElite, "RedItem", 5, "Percent chance for elite monsters to roll legendary item.");
            // Chances.Boss
            BossGreenItemChance = SrConfig.Bind<float>(SectionChancesBoss, "GreenItem", 90, "Percent chance for bosses to roll uncommon item.");
            BossRedItemChance = SrConfig.Bind<float>(SectionChancesBoss, "RedItem", 10, "Percent chance for bosses to roll legendary item.");
            // Interactables
            InteractableSpawnMultiplier = SrConfig.Bind<float>(SectionInteractables, "InteractableSpawnMultipler", 1, "A multiplier on the amount of interactables that will spawn in a level.");
            InteractableCostMultiplier = SrConfig.Bind<float>(SectionInteractables, "InteractableCostMultiplier", 1, "A multiplier applied to the cost of all interactables.");
            // Interactables.Chances
            // Chests
            ChestSmallChance = SrConfig.Bind<float>(SectionInteractablesChances, "SmallChest", 1, "The multiplier for this item to spawn.");
            ChestLargeChance = SrConfig.Bind<float>(SectionInteractablesChances, "LargeChest", 1, "The multiplier for this item to spawn.");
            ChestDamageChance = SrConfig.Bind<float>(SectionInteractablesChances, "CategoryDamageChest", 1, "The multiplier for this item to spawn.");
            ChestHealingChance = SrConfig.Bind<float>(SectionInteractablesChances, "CategoryHealingChest", 1, "The multiplier for this item to spawn.");
            ChestUtilityChance = SrConfig.Bind<float>(SectionInteractablesChances, "CategoryUtilityChest", 1, "The multiplier for this item to spawn.");
            ChestGoldChance = SrConfig.Bind<float>(SectionInteractablesChances, "GoldChest", 1, "The multiplier for this item to spawn.");
            ChestLunarChance = SrConfig.Bind<float>(SectionInteractablesChances, "LunarChest", 1, "The multiplier for this item to spawn.");
            ChestStealthedChance = SrConfig.Bind<float>(SectionInteractablesChances, "StealthedChest", 1, "The multiplier for this item to spawn.");
            EquipmentBarrelChance = SrConfig.Bind<float>(SectionInteractablesChances, "EquipmentBarrel", 1, "The multiplier for this item to spawn.");
            TripleShopSmallChance = SrConfig.Bind<float>(SectionInteractablesChances, "TripleShopSmall", 1, "The multiplier for this item to spawn.");
            TripleShopLargeChance = SrConfig.Bind<float>(SectionInteractablesChances, "TripleShopLarge", 1, "The multiplier for this item to spawn.");            
            BarrelChance = SrConfig.Bind<float>(SectionInteractablesChances, "Barrel", 1, "The multiplier for this item to spawn.");
            // Shrines
            ShrineHealingChance = SrConfig.Bind<float>(SectionInteractablesChances, "ShrineHealing", 1, "The multiplier for this item to spawn.");            
            ShrineBloodChance = SrConfig.Bind<float>(SectionInteractablesChances, "ShrineBlood", 1, "The multiplier for this item to spawn.");
            ShrineBossChance = SrConfig.Bind<float>(SectionInteractablesChances, "ShrineBoss", 1, "The multiplier for this item to spawn.");
            ShrineChanceChance = SrConfig.Bind<float>(SectionInteractablesChances, "ShrineChance", 1, "The multiplier for this item to spawn.");
            ShrineCombatChance = SrConfig.Bind<float>(SectionInteractablesChances, "ShrineCombat", 1, "The multiplier for this item to spawn.");
            ShrineRestackChance = SrConfig.Bind<float>(SectionInteractablesChances, "ShrineRestack", 1, "The multiplier for this item to spawn.");
            ShrineGoldshoresAccessChance = SrConfig.Bind<float>(SectionInteractablesChances, "ShrineGoldshoresAccess", 1, "The multiplier for this item to spawn.");
            // Drones
            BrokenDrone1Chance = SrConfig.Bind<float>(SectionInteractablesChances, "BrokenDrone1", 1, "The multiplier for this item to spawn.");
            BrokenDrone2Chance = SrConfig.Bind<float>(SectionInteractablesChances, "BrokenDrone2", 1, "The multiplier for this item to spawn.");
            BrokenMegaDroneChance = SrConfig.Bind<float>(SectionInteractablesChances, "BrokenMegaDrone", 1, "The multiplier for this item to spawn.");
            BrokenMissileDroneChance = SrConfig.Bind<float>(SectionInteractablesChances, "BrokenMissileDrone", 1, "The multiplier for this item to spawn.");
            BrokenEquipmentDroneChance = SrConfig.Bind<float>(SectionInteractablesChances, "BrokenEquipmentDrone", 1, "The multiplier for this item to spawn.");
            BrokenFlameDroneChance = SrConfig.Bind<float>(SectionInteractablesChances, "BrokenFlameDrone", 1, "The multiplier for this item to spawn.");
            BrokenTurretChance = SrConfig.Bind<float>(SectionInteractablesChances, "BrokenTurret", 1, "The multiplier for this item to spawn.");
            // Duplicators
            DuplicatorSmallChance = SrConfig.Bind<float>(SectionInteractablesChances, "DuplicatorSmall", 1, "The multiplier for this item to spawn.");
            DuplicatorLargeChance = SrConfig.Bind<float>(SectionInteractablesChances, "DuplicatorLarge", 1, "The multiplier for this item to spawn.");
            DuplicatorMilitaryChance = SrConfig.Bind<float>(SectionInteractablesChances, "DuplicatorMilitary", 1, "The multiplier for this item to spawn.");
            // Other
            RadarTowerChance = SrConfig.Bind<float>(SectionInteractablesChances, "RadarTower", 1, "The multiplier for this item to spawn.");
        }
    }
}
