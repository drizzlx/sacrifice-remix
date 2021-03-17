using BepInEx;
using RoR2;
using UnityEngine;

namespace SacrificeRemix
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.drizzlx.SacrificeRemix", "Sacrifice Remix", "1.1.3")]
    public sealed class SacrificeRemix : BaseUnityPlugin
    {
        // Module version
        public const string Version = "1.1.3";
        // Config file
        private Configurations configs;
        // Monster credit manipulators { Min, Max }
        private readonly float[] monsterCreditBase = { 15, 40 };
        private readonly float[] monsterCreditInterval = { 10, 20 };        
        private readonly float[] seriesSpawnInterval = { 0.1f, 1 }; // default { 0.1, 1 }        
        private readonly float[] rerollSpawnInterval = { 2.333f, 4.333f }; // default { 2.333, 4.333 }
        // Timers
        private float monsterCreditTimer = 0;

        // Called when loaded by BepInEx
        private void Awake()
        {                        
            // Init configs
            configs = Configurations.Instance();

            // Handler: Override core variables
            On.RoR2.CombatDirector.Awake += (orig, self) =>
            {
                self.minSeriesSpawnInterval = seriesSpawnInterval[0];
                self.maxSeriesSpawnInterval = seriesSpawnInterval[1];                
                self.minRerollSpawnInterval = rerollSpawnInterval[0];
                self.maxRerollSpawnInterval = rerollSpawnInterval[1];               

                orig(self);
            };

            // Handler: Monster credit manipulator
            On.RoR2.CombatDirector.Simulate += (orig, self, deltaTime) =>
            {
                if (!IsModuleEnabled())
                {
                    orig(self, deltaTime);
                    return;
                }

                // Update interval
                monsterCreditTimer -= deltaTime;

                // Update credit for faster spawns
                if (monsterCreditTimer < 0)
                {                                        
                    // Credit multiplier
                    float creditMultiplier = configs.SpawnIntensity.Value;
                    float additionalPlayers = NetworkUser.readOnlyInstancesList.Count - 1f;
                    creditMultiplier += additionalPlayers * configs.SpawnIntensityPerPlayer.Value;

                    // Spawn boost min max
                    float minBaseCredit = monsterCreditBase[0];
                    float maxBaseCredit = monsterCreditBase[1] + (GetStageCount() * creditMultiplier);

                    // Scale difficulty
                    self.monsterCredit *= creditMultiplier > 0 ? creditMultiplier : 1;                    

                    if (configs.BoostSpawnRates.Value && self.monsterCredit < minBaseCredit)
                    {
                        self.monsterCredit = Random.Range(minBaseCredit, maxBaseCredit);
                    }

                    if (configs.IsDeveloperMode.Value)
                    {
                        Chat.AddMessage("Spawn Credit: " + self.monsterCredit);
                    }

                    // Reset timer
                    monsterCreditTimer = Random.Range(monsterCreditInterval[0], monsterCreditInterval[1]);                    
                }

                orig(self, deltaTime);
            };

            // Handler: Drop rewards
            On.RoR2.DeathRewards.OnKilledServer += (orig, self, damageReport) =>
            {
                if (IsModuleEnabled())
                {
                    DropHandler.Instance().DropLoot(damageReport);
                }                

                orig(self, damageReport);
            };
        }

        // Called at the first frame of the game.
        private void Start()
        {
            if (IsModuleEnabled())
            {
                Chat.AddMessage("Welcome to Sacrifice Remix! Don't forget to smoke up <3");            
            }            
        }

        public static bool IsModuleEnabled()
        {
            return Configurations.Instance().IsModuleEnabled.Value;
        }

        public static int GetStageCount()
        {
            return Run.instance.stageClearCount + 1;
        }

        [ConCommand(commandName = "sr_reload", flags = ConVarFlags.None, helpText = "SacrificeRemix: Reloads the configuration file.")]
        private static void CCReloadConfig(ConCommandArgs args)
        {
            Configurations.Instance().Reload();
            Console.print("SacrificeRemix.cfg has been reloaded!");
        }
    }
}
