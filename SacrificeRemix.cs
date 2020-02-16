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
        private static DropHandler DropHandler;
        private static Interactables Interactables;
        private float monsterCreditInterval = 20;
        private float monsterCreditTimer = 0;

        // Called when loaded by BepInEx.
        private void Awake()
        {
            // Load configs first
            var srConfig = new Configurations();

            // Init dependencies                        
            DropHandler = new DropHandler();
            Interactables = new Interactables();            

            // Handler: Monster spawn rate
            On.RoR2.CombatDirector.Simulate += (orig, self, deltaTime) =>
            {

                monsterCreditTimer -= deltaTime;

                if (monsterCreditTimer < 0)
                {
                    self.monsterCredit *= Configurations.MonsterSpawnDifficulty.Value;
                    monsterCreditTimer = monsterCreditInterval;

                    //Chat.AddMessage("Credit: " + self.monsterCredit);
                }
                
                //self.minSeriesSpawnInterval = 0.1f;
                //self.maxSeriesSpawnInterval = 1;                
                //self.minRerollSpawnInterval = 0.1f;
                //self.maxRerollSpawnInterval = 1;

                orig(self, deltaTime);
            };

            // Handler: Monster loot rewards
            On.RoR2.DeathRewards.OnKilledServer += (orig, self, damageReport) =>
            {
                CharacterBody attackerBody = damageReport.attackerBody;
                CharacterBody victimBody = damageReport.victimBody;

                if (attackerBody && victimBody && !victimBody.isPlayerControlled)
                {
                    DropHandler.DropLoot(victimBody, attackerBody);
                }

                orig.Invoke(self, damageReport);
            };

            // Handler: Interactables
            On.RoR2.ClassicStageInfo.GenerateDirectorCardWeightedSelection += (orig, self, categorySelection) =>
            {
                if (!Interactables.IsInteractableCategorySelection(categorySelection))
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
                            directorCard.spawnCard.directorCreditCost = Mathf.RoundToInt((float)directorCard.cost * Configurations.InteractableCostMultiplier.Value);

                            weightedSelection.AddChoice(directorCard, (float)directorCard.selectionWeight / num * category.selectionWeight);
                        }
                    }
                }

                return weightedSelection;
            };

            // Handler: Interactables spawn multiplier
            On.RoR2.SceneDirector.PopulateScene += (orig, self) =>
            {
                int num = Reflection.GetFieldValue<int>(self, "interactableCredit");
                num = Mathf.RoundToInt((float)num * Configurations.InteractableSpawnMultiplier.Value);

                Reflection.SetFieldValue<int>(self, "interactableCredit", num);
                orig.Invoke(self);
            };
        }

        // Called at the first frame of the game.
        private void Start()
        {
            Chat.AddMessage("Welcome to Sacrifice Remix! Don't forget to smoke up.");
        }
    }
}
