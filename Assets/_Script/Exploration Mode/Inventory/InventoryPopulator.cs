// Nick Olenz

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


// Populates the frame this script is applied to with every item in the inventory list.
public class InventoryPopulator : MonoBehaviour
{
	// Internal Variables.
	List<GameObject> itemsInList = new List<GameObject>();
	GameObject tileBase;


	void Awake ()
	{
		tileBase = Resources.Load<GameObject>("Prefabs/ItemTile");
		Repopulate();
	}

	// Scans the list of items, adds icons for each one.
	public void Repopulate()
	{
		// Destroy everything currently in the list.
		foreach (GameObject ii in itemsInList)
		{
			Destroy(ii);
		}

		// Actually repopulate.
		foreach (KeyValuePair<string, InvItem> ii in InventoryController.items)
		//foreach (InvItem ii in InventoryController.items)
		{
			// Create the object.
			GameObject instance = Instantiate(tileBase);

			// Set parent and internals.
			instance.transform.SetParent(this.transform, false);
			instance.GetComponentInChildren<Text>().text = ii.Value.quantity.ToString();

			// Create Icon
			AddIcon(ii.Value.pickup, instance.transform);

			// Add to object list.
			itemsInList.Add(instance);
		}
	}

	public static void AddIcon(GameObject iconBase, Transform parent)
	{
		// Create Icon and clean it up... a lot.
		// Icon only works currently in Camera Space, so I've added a
		// dedicated UI camera.
		GameObject icon = Instantiate(iconBase);
		icon.transform.SetParent(parent);
		icon.transform.localPosition = new Vector3(0f, 4f, -10f);		// Arbitrary.
		icon.transform.localScale = icon.transform.localScale / 2;      // Arbitrary.
		foreach (Transform ii in icon.GetComponentsInChildren<Transform>())
		{
			//icon.layer = 5;     // UI layer.
			ii.gameObject.layer = 5;
		}
		Destroy(icon.GetComponent<PickUp>());   // Don't want to be able to actually collect it out of the menu.
		Destroy(icon.GetComponent<ParticleSystem>());	// Or else the inventory becomes a rave party.
		icon.SetActive(true);		// Often spawns in disabled, for some unknown reason.
	}
}
