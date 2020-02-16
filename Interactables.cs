using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace SacrificeRemix
{
    class Interactables
    {
        private readonly string[] Categories;
		private static List<InteractableConfiguration> Configs;

		public Interactables()
        {
			this.Categories = new string[]
			{
				"Chests",
				"Barrels",
				"Shrines",
				"Drones",
				"Misc",
				"Rare",
				"Duplicator"
			};

			Configs = new List<InteractableConfiguration>
			{
				new InteractableConfiguration("Chest1", Configurations.ChestSmallChance.Value),
				new InteractableConfiguration("Chest2", Configurations.ChestLargeChance.Value),
				new InteractableConfiguration("GoldChest", Configurations.ChestGoldChance.Value),
				new InteractableConfiguration("LunarChest", Configurations.ChestLunarChance.Value),
				new InteractableConfiguration("Barrel1", Configurations.BarrelChance.Value),
				new InteractableConfiguration("EquipmentBarrel", Configurations.EquipmentBarrelChance.Value),
				new InteractableConfiguration("Duplicator", Configurations.DuplicatorSmallChance.Value),
				new InteractableConfiguration("DuplicatorLarge", Configurations.DuplicatorLargeChance.Value),
				new InteractableConfiguration("DuplicatorMilitary", Configurations.DuplicatorMilitaryChance.Value),
				new InteractableConfiguration("ShrineGoldshoresAccess", Configurations.ShrineGoldshoresAccessChance.Value),
				new InteractableConfiguration("RadarTower", Configurations.RadarTowerChance.Value),
				new InteractableConfiguration("Chest1Stealthed", Configurations.ChestStealthedChance.Value),
				new InteractableConfiguration("Drone1Broken", Configurations.BrokenDrone1Chance.Value),
				new InteractableConfiguration("Drone2Broken", Configurations.BrokenDrone2Chance.Value),
				new InteractableConfiguration("MegaDroneBroken", Configurations.BrokenMegaDroneChance.Value),
				new InteractableConfiguration("MissileDroneBroken", Configurations.BrokenMissileDroneChance.Value),
				new InteractableConfiguration("EquipmentDroneBroken", Configurations.BrokenEquipmentDroneChance.Value),
				new InteractableConfiguration("FlameDroneBroken", Configurations.BrokenFlameDroneChance.Value),
				new InteractableConfiguration("Turret1Broken", Configurations.BrokenTurretChance.Value),
				new InteractableConfiguration("ShrineBlood", Configurations.ShrineBloodChance.Value),
				new InteractableConfiguration("ShrineBoss", Configurations.ShrineBossChance.Value),
				new InteractableConfiguration("ShrineCombat", Configurations.ShrineCombatChance.Value),
				new InteractableConfiguration("ShrineChance", Configurations.ShrineChanceChance.Value),
				new InteractableConfiguration("ShrineRestack", Configurations.ShrineRestackChance.Value),
				new InteractableConfiguration("ShrineHealing", Configurations.ShrineHealingChance.Value),
				new InteractableConfiguration("TripleShop", Configurations.TripleShopSmallChance.Value),
				new InteractableConfiguration("TripleShopLarge", Configurations.TripleShopLargeChance.Value),
				new InteractableConfiguration("CategoryChestDamage", Configurations.ChestDamageChance.Value),
				new InteractableConfiguration("CategoryChestHealing", Configurations.ChestHealingChance.Value),
				new InteractableConfiguration("CategoryChestUtility", Configurations.ChestUtilityChance.Value)
			};
		}
		public bool IsInteractableCategorySelection(DirectorCardCategorySelection categorySelection)
		{
			foreach (DirectorCardCategorySelection.Category category in categorySelection.categories)
			{
				foreach (string value in this.Categories)
				{					
					if (category.name.Equals(value))
					{
						return true;
					}
				}
			}

			return false;
		}

		public static bool ApplyConfigModifiers(DirectorCard card)
		{
			foreach (InteractableConfiguration interactableConfig in Configs)
			{				
				if (card.spawnCard.prefab.name == interactableConfig.Name)
				{					
					if (interactableConfig.SpawnWeightModifier <= 0f)
					{
						return false;
					}

					card.selectionWeight = Mathf.RoundToInt((float)card.selectionWeight * interactableConfig.SpawnWeightModifier);

					break;
				}
			}

			return true;
		}
	}

	public class InteractableConfiguration
	{
		public string Name;
		public float SpawnWeightModifier;

		public InteractableConfiguration(string name, float spawnWeightModifier)
		{
			this.Name = name;
			this.SpawnWeightModifier = spawnWeightModifier;
		}
	}
}
