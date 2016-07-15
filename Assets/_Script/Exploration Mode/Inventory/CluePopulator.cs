using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CluePopulator : MonoBehaviour
{
	// References.
	public Image bigImage;

	// Internal Variables.
	List<GameObject> cluesInList = new List<GameObject>();
	GameObject clueBase;

	// Clue storage
	public static Dictionary<string, Sprite> clues = new Dictionary<string, Sprite>();

	void Awake()
	{
		clueBase = Resources.Load<GameObject>("Prefabs/ClueTile");
		Repopulate();
	}

	// Scans the list of items, adds icons for each one.
	public void Repopulate()
	{
		// Clear out the big image.
		bigImage.color = new Color(1f, 1f, 1f, 0f);

		// Destroy everything currently in the list.
		foreach (GameObject ii in cluesInList)
		{
			Destroy(ii);
		}

		// Actually repopulate.
		foreach (KeyValuePair<string, Sprite> ii in clues)
		//foreach (InvItem ii in InventoryController.items)
		{
			// Create the object.
			GameObject instance = Instantiate(clueBase);
			ClueButtonBridge cbb = instance.GetComponent<ClueButtonBridge>();
			cbb.bigImage = bigImage;
			cbb.clueSprite = ii.Value;

			// Set parent and internals.
			instance.transform.SetParent(this.transform, false);
			instance.GetComponent<Image>().sprite = ii.Value;

			// Add to object list.
			cluesInList.Add(instance);
		}
	}

	public static void AddClue(string clueName, Sprite clueSprite)
	{
		if (!clues.ContainsKey(clueName))
			clues.Add(clueName, clueSprite);
	}
}
