using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This ended up being slightly more hardcoded than I originally wanted,
// but I don't think it's worth the potential mess to make it super nice.

// Notes: Items have a few state variables. there's what I'm calling ACTIVE and there's also SELECTED.
// ACTIVE is like unlocked. An active item is one which has been built.
// SELECTED is the item which is currently being used, of your ACTIVE items.
public class ItemManager : MonoBehaviour
{
	// References.
	static GameObject player;

	// Gear scripts. Add more as needed.
	static RocketBoots boots;
	static Sledgehammer hammer;

	// List for scripts, helps with ordering. Add to as needed.
	static List<ItemBase> gear = new List<ItemBase>();
	static int currentIdx = 0;



	static void Refresh()
	{
		// Get Player.
		player = GameObject.FindGameObjectWithTag("Player");

		// Get every item manually. Add more as needed.
		boots = player.GetComponent<RocketBoots>();
		hammer = player.GetComponent<Sledgehammer>();

		// Clear the list and rebuild because it's the safest solution I can think of.
		// Again. Add more items as needed.
		gear.Clear();
		if (ConversationTrigger.GetToken("gear_rocketboots"))
			gear.Add(boots);
		if (ConversationTrigger.GetToken("gear_sledgehammer"))
			gear.Add(hammer);
	}



	public static void SelectGear(int idx)
	{
		// Make sure it's refreshed.
		Refresh();
		
		// Disable all gear for safety.
		foreach (ItemBase ii in gear)
		{
			ii.Deselect();
		}

		// Select the one at idx and update marker.
		gear[idx].Select();
		currentIdx = idx;

	}
	public static void NextGear()
	{
		if (gear.Count == 0)
			return;

		SelectGear((currentIdx + 1) % gear.Count);
	}
	public static void PrevGear()
	{
		if (gear.Count == 0)
			return;

		int nextIndex = currentIdx - 1;
		if (nextIndex < 0)
			nextIndex = gear.Count - 1;

		SelectGear(nextIndex);
	}
	

	// While the overwhelming majority of this script is static, it makes the most sense to have
	// the controls be in here as well, so I'm adding an Update function and applying this script to the player.
	void Update()
	{
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
			NextGear();
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
			PrevGear();
	}
}
