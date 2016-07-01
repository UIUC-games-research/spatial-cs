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
	Sprite questionMark;

	void Awake ()
	{
		questionMark = Resources.Load<Sprite>("UI_Elements/QuestionMark");
	}

	void Start ()
	{
		GetComponent<Button>().onClick.AddListener(() =>    // Adds an event to the button
		{
			//Debug.Log("Recipe Button Pressed: " + myRecipe.recipeName);

			// Destroy everything on the right.
			foreach (GameObject jj in recPop.iconsInList)
			{
				Destroy(jj);
			}

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
					recPop.constructButton.interactable = true;
				else
					recPop.constructButton.interactable = false;

				// Add to object list.
				recPop.iconsInList.Add(instanceTile);
			}

			recPop.constructButton.onClick.AddListener(() =>
			{
				// Save player info before entering.
				InventoryController.levelName = SceneManager.GetActiveScene().name;

				// TODO remove items from player's inventory... When should this happen?
				// Talk to Helen about disallowing jumping back and forth between scenes. Would be 1000x easier.
				// Just have recipes require some amount of energy to start, and you have to finish them before leaving
				// the mode. Or something like that.

				// Enter.
				SceneManager.LoadScene(myRecipe.recipeDesc);
			});
		});
	}
}
