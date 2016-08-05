// Nick Olenz

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

// This class unfortunately has to exist because AddListener is not persistent.
public class RecipeButtonBridge : MonoBehaviour
{
	// References.
	public Recipe myRecipe;	// Set on creation by RecipePopulator.
	public RecipePopulator recPop; // Same as above.
	InventoryController invController;
	Sprite questionMark;

	// Internal.
	List<bool> constructFlags = new List<bool>();	// Holds a list of booleans when deciding if construct button should enable.

	void Awake ()
	{
		questionMark = Resources.Load<Sprite>("UI_Elements/QuestionMark");

	}

	void Start ()
	{
		invController = transform.parent.parent.parent.GetComponent<InventoryController>();

		GetComponent<Button>().onClick.AddListener(() =>    // Adds an event to the button
		{
			//Debug.Log("Recipe Button Pressed: " + myRecipe.recipeName);

			// Destroy everything on the right.
			foreach (GameObject jj in recPop.iconsInList)
			{
				Destroy(jj);
			}
			// Clear the flag list.
			constructFlags.Clear();

			// Actually repopulate.
			foreach (InvItem jj in myRecipe.components)
			{
				// Create the object.
				GameObject instanceTile = Instantiate(recPop.tileBase);

				// Set parent and internals.
				instanceTile.transform.SetParent(recPop.rightPane.transform, false);
				instanceTile.GetComponentInChildren<Text>().text = InventoryController.GetQuantity(jj.itemName) + " / " + jj.quantity.ToString();


				// Find the correct icon from the inventory.
				if (InventoryController.items.ContainsKey(jj.itemName))
					InventoryPopulator.AddIcon(InventoryController.items[jj.itemName].pickup, instanceTile.transform);
				else
					instanceTile.GetComponentInChildren<Image>().sprite = questionMark;


				// Decide whether to make Construct button selectable.
				if (InventoryController.GetQuantity(jj.itemName) >= jj.quantity)
					constructFlags.Add(true);
				else
					constructFlags.Add(false);

				// Add to object list.
				recPop.iconsInList.Add(instanceTile);
			}

			// Check the flags for each item in the recipe to see if we have all the items.
			foreach (bool bb in constructFlags)
			{
				if (!bb)
				{
					recPop.constructButton.interactable = false;
					break;
				}
				else
				{
					recPop.constructButton.interactable = true;
				}
			}

			// Remove all other listeners so only the one we add in the next line will work.
			// This stops several scenes from loading at once, causing terrible things.
			recPop.constructButton.onClick.RemoveAllListeners();

			recPop.constructButton.onClick.AddListener(() =>
			{
				// Save player info before entering.
				InventoryController.levelName = SceneManager.GetActiveScene().name;

				// Close the inventory for safety on updates.
				//invController.CloseInventory();

				// Ensure cursor is active.
				//Cursor.visible = true;
				//Cursor.lockState = CursorLockMode.None;

				// Record data.
				SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO," + myRecipe.recipeDesc);

				// Enter.

				// Special cases for tutorial progress.
				if (myRecipe.recipeDesc == "tutorial1")
				{
					if (ConversationTrigger.GetToken("done_with_tutorial_2"))
					{
						LoadUtils.LoadScene("construction");
					}
					else if (ConversationTrigger.GetToken("done_with_tutorial_1"))
					{
						LoadUtils.LoadScene("tutorial2");
					}
					else
					{
						LoadUtils.LoadScene("tutorial1");
					}
				}
				else  // Normal function.
				{
					LoadUtils.LoadScene(myRecipe.recipeDesc);
				}

				invController.CloseInventory();
			});
		});
	}
}
