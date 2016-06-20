using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerCharacter {
	const string enterConstructionModeHotkey = "c";
	const string fireCatapultHotkey = "x";
	const string toggleBladeHotkey = "v";
	public bool bladeEnabled;
	public bool catapultEnabled;
	public bool rocketJumpEnabled;
	List<Item> inventory = new List<Item>();
	[CanBeNull] public Recipe recipeInMind;
	bool CanEnterConstructionMode {get {return recipeInMind != null && recipeInMind.CanBeDoneWith(inventory);}}
	//public for tests.
	public IEnumerable<Type> InventoryItemTypes {get {return inventory.Select(item => item.GetType()).Distinct();}}
	/// <summary> Will show counts for all items that the player has, and all items that the player needs for their current recipe. </summary>
	IEnumerable<ItemTypeDisplay> ActiveItemTypeDisplays {
		get {return ActiveRecipeRelevantItemTypeDisplays.Cast<ItemTypeDisplay>().Concat(ActiveRecipeIrrelevantItemTypeDisplays.Cast<ItemTypeDisplay>());}
	}
	IEnumerable<ItemTypeDisplay.RecipeRelevant> ActiveRecipeRelevantItemTypeDisplays {
		get {
			if (recipeInMind == null) return Enumerable.Empty<ItemTypeDisplay.RecipeRelevant>();
			return recipeInMind.ItemTypesNeeded.Select(
				type => new ItemTypeDisplay.RecipeRelevant(type, inventory.Count(type), recipeInMind.AmountNeededOf(type)));
		}
	}
	//public for tests.
	public IEnumerable<ItemTypeDisplay.RecipeIrrelevant> ActiveRecipeIrrelevantItemTypeDisplays {
		get {
			return InventoryItemTypes.Where(type => recipeInMind == null || !recipeInMind.NeedsAnyOf(type)).Select(
					type => new ItemTypeDisplay.RecipeIrrelevant(type, inventory.Count(type)));
		}
	}
	public float JumpForce {get {return rocketJumpEnabled ? 250 : 50;}}
	public void DrawHeadsUpDisplay (HeadsUpDisplayState state) {
		if (state.inventoryShown) DrawInventory();
		if (CanEnterConstructionMode)
			DrawEnterConstructionModePrompt();
	}
	public void AcceptInput () {
		if (CanEnterConstructionMode && Input.GetKeyDown(enterConstructionModeHotkey)) EnterConstructionMode();
		if (catapultEnabled && Input.GetKeyDown(fireCatapultHotkey)) FireCatapult();
		if (bladeEnabled && Input.GetKeyDown(toggleBladeHotkey)) ToggleBladeActive();
	}
	static void FireCatapult () {
		var bullet = ObjectPooler.Instance().GetPooledObject(1);
		bullet.SetActive(true);
	}
	static void ToggleBladeActive () {
		var blade = Reference.Instance().blade;
		blade.SetActive(!blade.activeSelf);
	}
	static void EnterConstructionMode () {
		MonoBehaviour.print("Not yet supported.");
		//todo
	}
	void DrawInventory () {
		var leftmostPoint = 100;
		var itemIconMargin = 20;
		ActiveItemTypeDisplays.ForEach((index, itemDisplay) => {
			var offsetFromleft = leftmostPoint + index * (ItemTypeDisplay.width + itemIconMargin);
			var offsetFromTop = Screen.height - itemIconMargin - ItemTypeDisplay.height;
			itemDisplay.Draw(offsetFromleft, offsetFromTop);
		});
	}
	static void DrawEnterConstructionModePrompt () {GUI.Label(new Rect(10, 10, 200, 200), "PRESS \"" + enterConstructionModeHotkey.ToUpper() + "\" TO ENTER CONSTRUCTION MODE");}
	public void Collect (Item item) {inventory.Add(item);}
}

public class HeadsUpDisplayState {
	public bool inventoryShown;
}
