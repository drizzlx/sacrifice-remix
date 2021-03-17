using BepInEx;
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
        public string SectionSummoner = "2.0 Summoner";
        public string SectionDrops = "3.0 Drops";
        public string SectionDropsNormal = "3.1 Drops.Normal";
        public string SectionDropsElite = "3.2 Drops.Elite";
        public string SectionDropsBoss = "3.3 Drops.Boss";
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
            CloversRerollRarity,
            DropOnMonsterPos;

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
            NormalBossItemChance,
            NormalEquipmentChance,
            // Drops.Elite
            EliteGreenItemChance,
            EliteRedItemChance,
            EliteBossItemChance,
            EliteEquipmentChance,
            // Drops.Boss
            BossGreenItemChance,
            BossRedItemChance,
            BossBossItemChance,
            BossEquipmentChance,
            // Summoner
            SummonDroneChance;

        public Configurations()
        {
            CheckConfigVersion();
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
            // Init config file
            srConfig = new ConfigFile(configFilePath, true);            
            // Developer            
            ModuleVersion = srConfig.Bind<string>(SectionDeveloper, 
                "Version", SacrificeRemix.Version, 
                "The configuration file version. Do not modify.");
            IsDeveloperMode = srConfig.Bind<bool>(SectionDeveloper, 
                "IsDeveloperMode", false, 
                "Enable custom logs for debugging.");
            // General
            IsModuleEnabled = srConfig.Bind<bool>(SectionGeneral, 
                "IsModuleEnabled", true, 
                "Enable or disable the module.");
            BoostSpawnRates = srConfig.Bind<bool>(SectionGeneral, 
                "BoostSpawnRates", true, 
                "Monsters spawn faster than usual; this is influenced by the SpawnIntensity multiplier.");
            SpawnIntensity = srConfig.Bind<float>(SectionGeneral, 
                "SpawnIntensity", 1.25f, 
                "A multiplier for the rate at which more difficult monsters spawn; 1 is the default rate.");
            SpawnIntensityPerPlayer = srConfig.Bind<float>(SectionGeneral, 
                "SpawnIntensityPerPlayer", 0,
                "Scale SpawnIntensity for each additional player (0 to disable). " +
                "Example: SpawnIntensity 1 + 0.25 PerPlayer = 100%/125%/150%/175% with 1/2/3/4 players.");            
            // Summoner
            SummonDroneChance = srConfig.Bind<float>(SectionSummoner, 
                "SummonDroneChance", 0.5f, 
                "The chance to spawn a temporary drone on kill. 0 to disable.");
            // Drops
            NormalDropChance = srConfig.Bind<float>(SectionDrops,
                "NormalDropChance", 1, 
                "Percent chance for normal monsters to drop an item. 0 to disable.");
            EliteDropChance = srConfig.Bind<float>(SectionDrops,
                "EliteDropChance", 2, 
                "Percent chance for elite monsters to drop an item. 0 to disable.");
            BossDropChance = srConfig.Bind<float>(SectionDrops,
                "BossDropChance", 4, 
                "Percent chance for bosses to drop an item. 0 to disable.");
            CloversRerollDrops = srConfig.Bind<bool>(SectionDrops, 
                "CloversRerollDrops", false, 
                "Can clovers reroll the chance of an item dropping.");
            CloversRerollRarity = srConfig.Bind<bool>(SectionDrops, 
                "CloversRerollRarity", true, 
                "Can clovers reroll the rarity of an item that's dropping (e.g. increased chance for red or green).");
            DropOnMonsterPos = srConfig.Bind<bool>(SectionDrops,
                "DropOnMonsterPos", false,
                "If enabled, drops will spawn on the monster position. If disabled, drops will spawn on the player position.");
            // Drops.Normal            
            NormalGreenItemChance = srConfig.Bind<float>(SectionDropsNormal, 
                "GreenItem", 27, 
                "Percent chance for normal monsters to roll uncommon item.");
            NormalRedItemChance = srConfig.Bind<float>(SectionDropsNormal, 
                "RedItem", 3, 
                "Percent chance for normal monsters to roll legendary item.");
            NormalBossItemChance = srConfig.Bind<float>(SectionDropsNormal,
                "BossItem", 0,
                "Percent chance for normal monsters to roll boss item.");
            NormalEquipmentChance = srConfig.Bind<float>(SectionDropsNormal,
                "Equipment", 0,
                "Percent chance for normal monsters to roll equipment.");
            // Drops.Elite
            EliteGreenItemChance = srConfig.Bind<float>(SectionDropsElite, 
                "GreenItem", 45, 
                "Percent chance for elite monsters to roll uncommon item.");
            EliteRedItemChance = srConfig.Bind<float>(SectionDropsElite, 
                "RedItem", 5, 
                "Percent chance for elite monsters to roll legendary item.");
            EliteBossItemChance = srConfig.Bind<float>(SectionDropsElite,
                "BossItem", 0,
                "Percent chance for elite monsters to roll boss item.");
            EliteEquipmentChance = srConfig.Bind<float>(SectionDropsElite,
                "Equipment", 0,
                "Percent chance for elite monsters to roll equipment.");
            // Drops.Boss
            BossGreenItemChance = srConfig.Bind<float>(SectionDropsBoss, 
                "GreenItem", 62, 
                "Percent chance for bosses to roll uncommon item.");
            BossRedItemChance = srConfig.Bind<float>(SectionDropsBoss, 
                "RedItem", 8, 
                "Percent chance for bosses to roll legendary item.");
            BossBossItemChance = srConfig.Bind<float>(SectionDropsBoss,
                "BossItem", 0,
                "Percent chance for bosses to roll boss item.");
            BossEquipmentChance = srConfig.Bind<float>(SectionDropsBoss,
                "Equipment", 0,
                "Percent chance for bosses to roll equipment.");
        }
    }
}