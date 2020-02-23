using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace SacrificeRemix
{    
    class DropHandler
    {
        private static DropHandler instance;
        private readonly Configurations configs = Configurations.Instance();

        public void DropLoot(CharacterBody victimBody, CharacterBody attackerBody)
        {
            float chanceToDropItem;
            string victimType;           

            if (victimBody.isElite)
            {
                victimType = "elite";
                chanceToDropItem = configs.EliteDropChance.Value;
            }
            else if (victimBody.isBoss)
            {
                victimType = "boss";
                chanceToDropItem = configs.BossDropChance.Value;
            }
            else
            {
                victimType = "normal";
                chanceToDropItem = configs.NormalDropChance.Value;
            }

            // Check if item should drop
            var canDropItem = configs.CloversRerollDrops.Value ? Util.CheckRoll(chanceToDropItem, attackerBody.master) : Util.CheckRoll(chanceToDropItem);
            
            if (chanceToDropItem <= 0 || !canDropItem)
            {
                return;
            }                       

            // Get the attacker body to use transform object
            var transform = attackerBody.transform;

            // Spawn item above player position
            Vector3 position = transform.position + Vector3.up * 3f;

            // Randomize velocity so that drops will randomly shoot out in any direction
            // Randomize X axis (right/left)
            var dropRightVelocityOffset = Random.Range(-15f, 15f);
            // Randomize Y axis (up/down)
            var dropUpVelocityStrength = Random.Range(5f, 10f);
            // Randomize Z axis (forwards/backwards)
            // Prevent drop from falling directly on the player by having a minimum strength
            var dropForwardVelocityStrength = Random.Range(0, 2) == 0 ? Random.Range(5f, 15f) : Random.Range(-5f, -15f);

            // Add vectors to create the velocity
            Vector3 velocity =
                Vector3.right * dropRightVelocityOffset + // X axis
                Vector3.up * dropUpVelocityStrength + // Y axis
                transform.forward * dropForwardVelocityStrength // Z axis                    
            ;

            Util.PlaySound("Play_UI_item_land_tier3", attackerBody.masterObject);
            Util.PlaySound("Play_UI_item_land_tier3", attackerBody.gameObject);
            // And then finally drop it in front of the player
            PickupDropletController.CreatePickupDroplet(RollItem(victimType, attackerBody.master), position, velocity);
        }

        private Vector3 RandomPointOnSphereEdge(float radius)
        {
            var vector = Random.insideUnitSphere.normalized * radius;               
            
            return vector;
        }

        private PickupIndex RollItem(string victimType, CharacterMaster master)
        {
            var configs = Configurations.Instance();
            // Drops
            List<PickupIndex> dropList;
            int itemIndex;
            // Percent chance to drop
            float whiteItemChance, greenItemChance, redItemChance;
            // Rarity roll
            bool greenItemRoll, redItemRoll;

            switch (victimType)
            {
                case "elite":
                    greenItemChance = configs.EliteGreenItemChance.Value;
                    redItemChance = configs.EliteRedItemChance.Value;
                    break;
                case "boss":
                    greenItemChance = configs.BossGreenItemChance.Value;
                    redItemChance = configs.BossRedItemChance.Value;
                    break;
                default:
                    greenItemChance = configs.NormalGreenItemChance.Value;
                    redItemChance = configs.NormalRedItemChance.Value;
                    break;
            }
            
            // Rarity roll
            greenItemRoll = configs.CloversRerollRarity.Value ? Util.CheckRoll(greenItemChance, master) : Util.CheckRoll(greenItemChance);
            redItemRoll = configs.CloversRerollRarity.Value ? Util.CheckRoll(redItemChance, master) : Util.CheckRoll(redItemChance);
            // Common chance
            whiteItemChance = 100f - greenItemChance - redItemChance;

            // Red item
            if (redItemRoll)
            {
                dropList = Run.instance.availableTier3DropList;
            }
            // Green item
            else if (whiteItemChance <= 0 || greenItemRoll)
            {
                dropList = Run.instance.availableTier2DropList;
            }
            // White item
            else
            {
                dropList = Run.instance.availableTier1DropList;
            }

            itemIndex = Run.instance.treasureRng.RangeInt(0, dropList.Count);

            return dropList[itemIndex];
        }        

        public static DropHandler Instance()
        {
            return instance ?? (instance = new DropHandler());
        }
    }
}
