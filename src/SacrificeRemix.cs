using BepInEx;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace SacrificeRemix
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.drizzlx.SacrificeRemix", "Sacrifice Remix", "1.0.2")]
    public sealed class SacrificeRemix : BaseUnityPlugin
    {
        private readonly float minMonsterCreditInterval = 15;
        private readonly float maxMonsterCreditInterval = 30;
        private float monsterCreditTimer = 0;
        private Configurations configs;

        // Called when loaded by BepInEx.
        private void Awake()
        {
            // Init configs
            configs = Configurations.Instance();                  

            // Handler: Monster spawn rate
            On.RoR2.CombatDirector.Simulate += (orig, self, deltaTime) =>
            {
                if (!IsModuleEnabled())
                {
                    orig(self, deltaTime);
                    return;
                }

                // Reduce timer
                monsterCreditTimer -= deltaTime;
                // Check if enough time has passed
                if (monsterCreditTimer < 0)
                {         
                    // Calculate credit multiplier
                    float additionalPlayers = NetworkUser.readOnlyInstancesList.Count - 1f;                   
                    float creditMultiplier = configs.MobSpawnDifficulty.Value / 100;                    
                    creditMultiplier += additionalPlayers * (configs.MobSpawnDifficultyPerPlayer.Value / 100);
                    // Apply credit multiplier
                    self.monsterCredit *= creditMultiplier;
                    // Reset timer
                    monsterCreditTimer = Random.Range(minMonsterCreditInterval, maxMonsterCreditInterval);

                    if (configs.IsDeveloperMode.Value)
                    {
                        Chat.AddMessage("Spawn Credit: " + self.monsterCredit + "; " + "Rate: " + (creditMultiplier * 100) + "%");
                    }                 
                }               

                orig(self, deltaTime);
            };

            // Handler: Monster loot rewards
            On.RoR2.DeathRewards.OnKilledServer += (orig, self, damageReport) =>
            {
                if (!IsModuleEnabled())
                {
                    orig.Invoke(self, damageReport);
                    return;
                }

                CharacterBody attackerBody = damageReport.attackerBody;
                CharacterBody victimBody = damageReport.victimBody;

                if (attackerBody && victimBody && !victimBody.isPlayerControlled)
                {
                    DropHandler.Instance().DropLoot(victimBody, attackerBody);
                }

                orig.Invoke(self, damageReport);
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
                Chat.AddMessage("Welcome to Sacrifice Remix! I'd love to hear your feedback on Discord: drizzlx#8615");
            }            
        }

        private bool IsModuleEnabled()
        {
            return Configurations.Instance().IsModuleEnabled.Value;
        }

        [ConCommand(commandName = "sr_reload", flags = ConVarFlags.None, helpText = "SacrificeRemix: Reloads the configuration file.")]
        private static void CCReloadConfig(ConCommandArgs args)
        {
            Configurations.Instance().Reload();
            Console.print("SacrificeRemix.cfg has been reloaded!");
        }
    }
}
