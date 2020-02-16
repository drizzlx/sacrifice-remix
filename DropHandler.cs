using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace SacrificeRemix
{    
    class DropHandler
    {        
        public void DropLoot(CharacterBody victimBody, CharacterBody attackerBody)
        {
            float chanceToDropItem;
            string victimType;

            if (victimBody.isElite)
            {
                victimType = "elite";
                chanceToDropItem = Configurations.EliteDropChance.Value;
            }
            else if (victimBody.isBoss)
            {
                victimType = "boss";
                chanceToDropItem = Configurations.BossDropChance.Value;
            }
            else
            {
                victimType = "normal";
                chanceToDropItem = Configurations.NormalDropChance.Value;
            }

            // Check if item should drop
            var canDropItem = getCloversRerollDrops() ? Util.CheckRoll(chanceToDropItem, attackerBody.master) : Util.CheckRoll(chanceToDropItem);
            
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

            // And then finally drop it infront of the player
            PickupDropletController.CreatePickupDroplet(RollItem(victimType, attackerBody.master), position, velocity);
        }        

        private PickupIndex RollItem(string victimType, CharacterMaster master)
        {
            List<PickupIndex> dropList;
            int itemIndex;
            // Percent chance to drop
            float whiteItemChance, greenItemChance, redItemChance;
            // Rarity roll
            bool greenItemRoll, redItemRoll;

            switch (victimType)
            {
                case "elite":
                    greenItemChance = Configurations.EliteGreenItemChance.Value;
                    redItemChance = Configurations.EliteRedItemChance.Value;
                    break;
                case "boss":
                    greenItemChance = Configurations.BossGreenItemChance.Value;
                    redItemChance = Configurations.BossRedItemChance.Value;
                    break;
                default:
                    greenItemChance = Configurations.NormalGreenItemChance.Value;
                    redItemChance = Configurations.NormalRedItemChance.Value;
                    break;
            }
            
            // Rarity roll
            greenItemRoll = getCloversRerollRarity() ? Util.CheckRoll(greenItemChance, master) : Util.CheckRoll(greenItemChance);
            redItemRoll = getCloversRerollRarity() ? Util.CheckRoll(redItemChance, master) : Util.CheckRoll(redItemChance);
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

        private bool getCloversRerollDrops()
        {
            return Configurations.CloversRerollDrops.Value;
        }

        private bool getCloversRerollRarity()
        {
            return Configurations.CloversRerollRarity.Value;
        }
    }
}
