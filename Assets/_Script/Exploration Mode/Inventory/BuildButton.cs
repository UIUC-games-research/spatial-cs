using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BuildButton : MonoBehaviour
{
	static Button build;
	static string whatToBuild = "";
	public GameObject indicator;
	static GameObject indicator_s;

	InventoryController invController;

	void Start ()
	{
		invController = transform.parent.parent.GetComponent<InventoryController>();
		indicator_s = indicator; // Need to ignore static limitations...
		build = GetComponent<Button>();
		build.interactable = false;
		indicator.SetActive(false);

		build.onClick.AddListener(() =>
		{	
			// Save player info before entering.
			InventoryController.levelName = SceneManager.GetActiveScene().name;

			// Record data.
			SimpleData.WriteDataPoint("Constructing_Item", "", "", "", "", whatToBuild);
			//SimpleData.WriteStringToFile("ModeSwitches.txt", Time.time + ",MODESWITCH_TO," + whatToBuild);

			// Enter.

			// Special cases for tutorial progress.
			if (whatToBuild == "tutorial1")
			{
				if (ConversationTrigger.GetToken("done_with_tutorial_2"))
				{
					LoadUtils.LoadScene("rocketBoots");
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
				LoadUtils.LoadScene(whatToBuild);
			}

			invController.CloseInventory();
		});
	}

	public static void CheckRecipes()
	{
		// Loop through all known recipes.
		build.interactable = false;
		indicator_s.SetActive(false);
		foreach (Recipe ii in RecipesDB.unlockedRecipes)
		{
			// Loop through all of the recipe's components.
			bool thisRecipeIsGood = true;
			foreach (InvItem jj in ii.components)
			{
				// Decide whether to make Construct button selectable.
				if (InventoryController.GetQuantity(jj.itemName) < jj.quantity)
				{
					thisRecipeIsGood = false;
					break;
				}
			}
			// Good recipe? Set it!
			if (thisRecipeIsGood)
			{
				build.interactable = true;
				indicator_s.SetActive(true);
				whatToBuild = ii.recipeDesc;
				break;
			}
		}
	}
}
