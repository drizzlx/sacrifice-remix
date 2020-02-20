using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace SacrificeRemix
{
    class Interactables
    {
		private static Interactables instance;
        private readonly string[] Categories;
		private static List<InteractableConfiguration> Configs;
		private readonly Configurations configs = Configurations.Instance();

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
				new InteractableConfiguration("Chest1", configs.ChestSmallChance.Value),
				new InteractableConfiguration("Chest2", configs.ChestLargeChance.Value),
				new InteractableConfiguration("GoldChest", configs.ChestGoldChance.Value),
				new InteractableConfiguration("LunarChest", configs.ChestLunarChance.Value),
				new InteractableConfiguration("Barrel1", configs.BarrelChance.Value),
				new InteractableConfiguration("EquipmentBarrel", configs.EquipmentBarrelChance.Value),
				new InteractableConfiguration("Duplicator", configs.DuplicatorSmallChance.Value),
				new InteractableConfiguration("DuplicatorLarge", configs.DuplicatorLargeChance.Value),
				new InteractableConfiguration("DuplicatorMilitary", configs.DuplicatorMilitaryChance.Value),
				new InteractableConfiguration("ShrineGoldshoresAccess", configs.ShrineGoldshoresAccessChance.Value),
				new InteractableConfiguration("RadarTower", configs.RadarTowerChance.Value),
				new InteractableConfiguration("Chest1Stealthed", configs.ChestStealthedChance.Value),
				new InteractableConfiguration("Drone1Broken", configs.BrokenDrone1Chance.Value),
				new InteractableConfiguration("Drone2Broken", configs.BrokenDrone2Chance.Value),
				new InteractableConfiguration("MegaDroneBroken", configs.BrokenMegaDroneChance.Value),
				new InteractableConfiguration("MissileDroneBroken", configs.BrokenMissileDroneChance.Value),
				new InteractableConfiguration("EquipmentDroneBroken", configs.BrokenEquipmentDroneChance.Value),
				new InteractableConfiguration("FlameDroneBroken", configs.BrokenFlameDroneChance.Value),
				new InteractableConfiguration("Turret1Broken", configs.BrokenTurretChance.Value),
				new InteractableConfiguration("ShrineBlood", configs.ShrineBloodChance.Value),
				new InteractableConfiguration("ShrineBoss", configs.ShrineBossChance.Value),
				new InteractableConfiguration("ShrineCombat", configs.ShrineCombatChance.Value),
				new InteractableConfiguration("ShrineChance", configs.ShrineChanceChance.Value),
				new InteractableConfiguration("ShrineRestack", configs.ShrineRestackChance.Value),
				new InteractableConfiguration("ShrineHealing", configs.ShrineHealingChance.Value),
				new InteractableConfiguration("TripleShop", configs.TripleShopSmallChance.Value),
				new InteractableConfiguration("TripleShopLarge", configs.TripleShopLargeChance.Value),
				new InteractableConfiguration("CategoryChestDamage", configs.ChestDamageChance.Value),
				new InteractableConfiguration("CategoryChestHealing", configs.ChestHealingChance.Value),
				new InteractableConfiguration("CategoryChestUtility", configs.ChestUtilityChance.Value)
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

		public static Interactables Instance()
		{
			return instance ?? (instance = new Interactables());
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
