// Nick Olenz

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Responsible for keeping the pane it's applied to updated with recipe names,
// and also the right pane which shows recipe status.
public class RecipePopulator : MonoBehaviour
{
	// References.
	public GameObject rightPane;    // Set in inspector.
	public Button constructButton;	// Set in inspector.

	// Internal Variables.
	List<GameObject> recipesInList = new List<GameObject>();
	public List<GameObject> iconsInList = new List<GameObject>();
	GameObject recipeBase;
	public GameObject tileBase;

	void Awake ()
	{
		recipeBase = Resources.Load<GameObject>("Prefabs/RecipeText");
		tileBase = Resources.Load<GameObject>("Prefabs/ItemTile");
		Repopulate();
	}

	// Repopulates the recipe list.
	public void Repopulate()
	{
		// Destroy everything currently in the list.
		foreach (GameObject ii in recipesInList)
		{
			Destroy(ii);
		}
		// Get rid of the icons on the right pane, too.
		foreach (GameObject ii in iconsInList)
		{
			Destroy(ii);
		}
		// Blank out the Construct button.
		constructButton.interactable = false;



		// Actually repopulate.
		foreach (Recipe ii in RecipesDB.unlockedRecipes)
		{
			// Create the object.
			GameObject instance = Instantiate(recipeBase);


			// Set parent and internals.
			RecipeButtonBridge instBridge = instance.GetComponent<RecipeButtonBridge>();
			instBridge.myRecipe = ii;
			instBridge.recPop = this;
			instance.transform.SetParent(this.transform, false);
			instance.GetComponentInChildren<Text>().text = ii.recipeName;

			// Setup button activation.
			// This part has been added to its own script, RecipeButtonBridge.
			// This had to happen because these listeners are not persistent, and
			// are overwritten on the next run through the loop...
			// IE each button needs its own listener script.

			// Add to object list.
			recipesInList.Add(instance);
		}
	}
}
