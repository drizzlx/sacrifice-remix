using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace SacrificeRemix
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.drizzlx.SacrificeRemix", "Sacrifice Remix", "1.1.0")]
    public sealed class SacrificeRemix : BaseUnityPlugin
    {
        // Module version
        public const string Version = "1.1.0";
        // Config file
        private Configurations configs;
        // Monster credit manipulators { Min, Max }
        private readonly float[] monsterCreditBase = { 20, 45 };
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
                    float minBaseCredit = monsterCreditBase[0] + (GetStageCount() * creditMultiplier);
                    float maxBaseCredit = monsterCreditBase[1] + (GetStageCount() * creditMultiplier);

                    // Scale difficulty
                    self.monsterCredit *= creditMultiplier > 0 ? creditMultiplier : 1;                    

                    if (configs.BoostSpawnRates.Value && self.monsterCredit < minBaseCredit)
                    {
                        self.monsterCredit = Random.Range(minBaseCredit, maxBaseCredit) + (self.monsterCredit * 0.2f);
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

            // Handler: Interactables
            On.RoR2.ClassicStageInfo.GenerateDirectorCardWeightedSelection += (orig, self, categorySelection) =>
            {                
                if (!IsModuleEnabled() || !Interactables.Instance().IsInteractableCategorySelection(categorySelection))
                {
                    return orig.Invoke(self, categorySelection);
                }

                WeightedSelection<DirectorCard> weightedSelection = new WeightedSelection<DirectorCard>(8);

                foreach (DirectorCardCategorySelection.Category category in categorySelection.categories)
                {
                    float num = categorySelection.SumAllWeightsInCategory(category);

                    foreach (DirectorCard directorCard in category.cards)
                    {                   
                        // True if interactable is enabled
                        if (Interactables.ApplyConfigModifiers(directorCard))
                        {
                            directorCard.spawnCard.directorCreditCost = Mathf.RoundToInt(directorCard.cost * configs.InteractableCostMultiplier.Value);

                            weightedSelection.AddChoice(directorCard, (float)directorCard.selectionWeight / num * category.selectionWeight);
                        }
                    }
                }

                return weightedSelection;
            };

            // Handler: Interactables spawn multiplier
            On.RoR2.SceneDirector.PopulateScene += (orig, self) =>
            {
                if (!IsModuleEnabled())
                {
                    orig.Invoke(self);
                    return;
                }

                int num = Reflection.GetFieldValue<int>(self, "interactableCredit");
                num = Mathf.RoundToInt(num * configs.InteractableSpawnMultiplier.Value);

                Reflection.SetFieldValue(self, "interactableCredit", num);
                orig.Invoke(self);
            };
        }

        // Called at the first frame of the game.
        private void Start()
        {
            if (IsModuleEnabled())
            {
                Chat.AddMessage("Welcome to Sacrifice Remix!");
                Chat.AddMessage("Let's be friends. http://bit.ly/drizzlx-discord");
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
