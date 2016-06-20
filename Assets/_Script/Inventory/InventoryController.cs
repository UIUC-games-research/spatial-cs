// Nick Olenz

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class InventoryController : MonoBehaviour
{
	// Child references, set in inspector.
	public GameObject tabs; // The tabs parent.
	public Transform[] tabButtons;	// Every tab.
	public GameObject inv;	// The inventory parent.
	public GameObject rec;  // The recipes parent.
	public InventoryPopulator invPop;   // Where items are populated.
	public RecipePopulator recPop;		// Where recipes are populated.

	// Other references, grabbed on Start.
	UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController player;
	
	float tabButtonY;	// Default Y position of tab buttons, for tab popping.

	// Internal variables.
	bool menuOpen = false;  // Whether menu is open. Used for releasing mouse.
	float sensitivityXTemp = 0;	// Used to store sensitivity settings while menu is open.
	float sensitivityYTemp = 0; // "

	// Storage for all items / parts currently held by the player.
	//public static List<InvItem> items = new List<InvItem>();
	public static Dictionary<string, InvItem> items = new Dictionary<string, InvItem>();

	void Awake ()
	{
		// Initial recipes unlocked should be placed here.
		RecipesDB.unlockedRecipes.Add(RecipesDB.TestRecipe);
		RecipesDB.unlockedRecipes.Add(RecipesDB.TestRecipe2);
	}


	void Start ()
	{
		// Grabbing references.
		player = Reference.Instance().player;
		//tabButtonY = tabButtons[1].transform.localPosition.y;

		// Will be closed on run, so disable all menus.
		// Grab initial sensitivity settings first, though, or they will zero.
		sensitivityXTemp = player.mouseLook.XSensitivity;
		sensitivityYTemp = player.mouseLook.YSensitivity;

		// Also ensure everything is populated when initialized.
		// We do this literally everywhere because script run order is not
		// defined in Unity... Keeps things interesting, I guess.
		invPop.Repopulate();
		recPop.Repopulate();

		CloseInventory();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			if (menuOpen)
				CloseInventory();
			else
				OpenInventory();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
			CloseInventory();
	}

	// Defaults to "Inventory" tab.
	void OpenInventory()
	{
		// Internal Settings.
		menuOpen = true;

		// Player Settings.
		player.mouseLook.SetCursorLock(false);
		sensitivityXTemp = player.mouseLook.XSensitivity;
		sensitivityYTemp = player.mouseLook.YSensitivity;
		player.mouseLook.XSensitivity = 0;
		player.mouseLook.YSensitivity = 0;

		// Menus Active.
		FakeActive(tabs, true);
		SwitchToInv();
	}

	void CloseInventory()
	{
		// Internal Settings.
		menuOpen = false;

		// Player Settings.
		player.mouseLook.SetCursorLock(true);
		player.mouseLook.XSensitivity = sensitivityXTemp;
		player.mouseLook.YSensitivity = sensitivityYTemp;

		// Menus Inactive.
		FakeActive(tabs, false);
		FakeActive(inv, false);
		FakeActive(rec, false);
	}

	// Tab switching functions.
	public void SwitchToInv()
	{
		FakeActive(inv, true);
		FakeActive(rec, false);

		PopUpTab(0);

		// Ensure inventory pane is populated.
		invPop.Repopulate();
	}
	public void SwitchToRec()
	{
		FakeActive(rec, true);
		FakeActive(inv, false);

		PopUpTab(1);

		// Ensure recipes pane is populated.
		recPop.Repopulate();
	}

	public void PopUpTab(int idx)
	{
		
		// Reset all heights to normal.
		foreach (Transform ii in tabButtons)
		{
			ii.localPosition = new Vector3(ii.localPosition.x, 0, ii.localPosition.z);
		}
		
		// Pop up the one at idx.
		Vector3 pos = tabButtons[idx].localPosition;
		pos.y += 4;
		tabButtons[idx].localPosition = pos;
		
	}

	// Unity's SetActive function is more trouble than it's worth in situations
	// like menus where a lot can be going on. SetActive doesn't seem to happen
	// until the next frame.
	// This solves that problem by moving "inactive" objects behind you... 
	public void FakeActive(GameObject go, bool active)
	{
		Vector3 pos = go.transform.localPosition;
		if (!active)
		{
			pos.z = 800;
			go.transform.localPosition = pos;
		}
		else
		{
			pos.z = 0;
			go.transform.localPosition = pos;
		}
	}

////////////////////////////////////////////
// Here starts functions related to items //
////////////////////////////////////////////

	// Definitely wasting time with these functions, but
	// the simplicity may be worth it, considering there aren't supposed
	// to be many different types of parts.
	public static int GetQuantity(string itemName)
	{
		if (items.ContainsKey(itemName))
		{
			return items[itemName].quantity;
		}
		Debug.Log("Item not found: " + itemName);
		return 0;
	}

	// Add an amount of an item type to the list.
	// Assuming that items will only be added by pickups.
	public static int Add(PickUp pickup, int quantity)
	{
		// If item is already in list.
		if (items.ContainsKey(pickup.pickupName))
			return items[pickup.pickupName].Add(quantity);

		// If item is not already in list.
		InvItem added = new InvItem(pickup.pickupName, pickup.pickupDesc, quantity, pickup.gameObject);
		items.Add(pickup.pickupName, added);
		return quantity;
	}

	public static int Consume(string itemName, int quantity)
	{
		if (items.ContainsKey(itemName))
			return items[itemName].Consume(quantity);

		Debug.Log("Item not found: " + itemName);
		return 0;
	}
}

////////////////////////////////
// Here starts the item class //
////////////////////////////////

public class InvItem
{
	// Fields
	public string itemName;
	public string description;
	public int quantity;            // Number of this item currently held
	public GameObject pickup;		// The object of the pickup itself is stored, may be used as an icon.

	// Ctor
	public InvItem(string newName, string newDesc, int newQuantity, GameObject newPickup)
	{
		itemName = newName;
		description = newDesc;
		quantity = newQuantity;
		pickup = newPickup;
	}

	// Remove "amount" of this item type.
	// Returns what is left.
	public int Consume(int amount)
	{
		if (quantity > amount)
		{
			quantity -= amount;
		}
		else
		{
			Debug.LogError("Attempting to consume more items than player has. Consume() does not account for this! Quantity will be set to 0");
			quantity = 0;
		}
		return quantity;
	}

	// Add "amount" of this item type.
	// Returns new quantity.
	public int Add(int amount)
	{
		if (amount < 0)
		{
			Debug.LogError("A negative amount of an item is being added. Use Consume() instead.");
			return Consume(-1 * amount);
		}
		quantity += amount;
		return quantity;
	}
}


//////////////////////////////////
// Here starts the recipe class //
//////////////////////////////////

public class InvRecipe
{
	public string recipeName;       // Name of recipe.
	public string recipeDesc;		// Description of recipe.
	public List<InvItem> components = new List<InvItem>();	// List of InvItems which make up the recipe.

	public InvRecipe(string newName, string newDesc, string[] itemNames, int[] itemQuantities)
	{
		recipeName = newName;
		recipeDesc = newDesc;
		if (itemNames.Length != itemQuantities.Length)
			Debug.LogError("Names and Quantities of items in recipe do not match in length. Recipe name: " + newName);

		for (int ii = 0; ii < itemNames.Length; ii++)
		{
			components.Add(new InvItem(itemNames[ii], "", itemQuantities[ii], null));
		}
	}
}
