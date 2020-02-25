using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SacrificeRemix
{    
    class DropHandler
    {
        private static DropHandler instance;
        private readonly Configurations configs = Configurations.Instance();

        public void DropLoot(DamageReport damageReport)
        {         
            // If attacker is minion then get the owner
            CharacterBody attackerBody = damageReport.attackerOwnerMaster ? damageReport.attackerOwnerMaster.GetBody() : damageReport.attackerBody;
            CharacterBody victimBody = damageReport.victimBody;

            // Only drop loot when a player kills an enemy
            if (!(attackerBody && victimBody 
                && attackerBody.isPlayerControlled && damageReport.attackerTeamIndex != damageReport.victimTeamIndex))
            {
                return;
            }

            // Handle summoner rewards
            var summonDroneChance = configs.SummonDroneChance.Value;

            if (summonDroneChance > 0 && Util.CheckRoll(summonDroneChance))
            {
                SummonDrone(attackerBody);
            }

            // Handle item drops
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

            // Should item drop
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

            // Drop it near the player
            Util.PlaySound("Play_UI_chest_unlock", attackerBody.gameObject);
            PickupDropletController.CreatePickupDroplet(RollItem(victimType, attackerBody.master), position, velocity);            
        }

        // TODO
        private Vector3 RandomPointOnSphereEdge(float radius)
        {
            var vector = Random.insideUnitSphere.normalized * radius;               
            
            return vector;
        }

        private PickupIndex RollItem(string victimType, CharacterMaster master)
        {
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

        public CharacterMaster SummonDrone(CharacterBody attackerBody)
        {
            var transform = attackerBody.transform;

            // Spawn drone above player position
            Vector3 position = transform.position + Vector3.up * 3f;

            Util.PlaySound("Play_drone_repair", attackerBody.gameObject);

            CharacterMaster characterMaster = SummonMaster(
                Resources.Load<GameObject>("Prefabs/CharacterMasters/DroneBackupMaster"), position, transform.rotation, attackerBody);
            
            if (characterMaster)
            {
                MinionOwnership ownership = characterMaster.GetComponent<MinionOwnership>();

                // Attach to player
                if (ownership)
                {
                    ownership.SetOwner(attackerBody.master);
                }

                // Random item count
                var playerItemCount = attackerBody.inventory.GetTotalItemCountOfTier(ItemTier.Tier1) 
                    + attackerBody.inventory.GetTotalItemCountOfTier(ItemTier.Tier2) 
                    + attackerBody.inventory.GetTotalItemCountOfTier(ItemTier.Tier3);

                var minRandomItemCount = Mathf.CeilToInt(playerItemCount * 0.2f);
                var maxRandomItemCount = Mathf.CeilToInt(playerItemCount * 0.5f);
                var randomItemCount = Random.Range(minRandomItemCount, maxRandomItemCount);  
                
                if (randomItemCount < 1 && Random.Range(0, 2) == 1)
                {
                    randomItemCount = 1;
                }
                                
                characterMaster.inventory.GiveRandomItems(randomItemCount);
                
                // Drone lifetime
                characterMaster.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 25f + UnityEngine.Random.Range(0f, 3f);
            }            

            return characterMaster;
        }

        private CharacterMaster SummonMaster(GameObject masterPrefab, Vector3 position, Quaternion rotation, CharacterBody player)
        {
            if (!NetworkServer.active)
            {                
                return null;                
            }

            return new MasterSummon
            {
                masterPrefab = masterPrefab,
                position = position,
                rotation = rotation,
                summonerBodyObject = null,
                ignoreTeamMemberLimit = false,
                teamIndexOverride = player.teamComponent.teamIndex
            }.Perform();
        }

        public static DropHandler Instance()
        {
            return instance ?? (instance = new DropHandler());
        }
    }
}
