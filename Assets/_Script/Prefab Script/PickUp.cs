using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public AudioSource Pickup;
	public AudioClip pickupSound;
	public enum PickupType { Item, Battery, Clue };

	[Header("Basic Variables")]
	public PickupType type = PickupType.Item;
	// Pickup-type Settings.
	public string pickupName = "";	// Give it a name, consider this a type. IE should be unique.
	public string pickupDesc = "";  // This is mostly an internal tag. Doesn't really get used.

	[Tooltip("This MUST BE SET for an item to save. It should look like PartPickups/RocketBoots/Pickup_Boots_Body or similar.")]
	public string prefabPath = "";

	// SPECIAL
	// Only necessary for clue:
	[Header("Special Variables")]
	public Sprite clueSprite;

	[Tooltip("If the token specified here exists, this pickup will not spawn (scene load only.)")]
	public string deleteToken = "";
	[Tooltip("If true, this pickup will automatically be unique, meaning it will not spawn again when reloading the scene. Does not work with batteries!")]
	public bool autoDelete = false;


	void Start()
	{
		// check deleteToken.
		if ((deleteToken != "" && ConversationTrigger.GetToken(deleteToken)) ||
			(autoDelete && ConversationTrigger.GetToken("autodelete_" + pickupName)))
		{
			Destroy(gameObject);
		}

		// We need to rename all the bits under the model of the pickup, if there are any.
		// The reason for this is names will conflict with names inside construction mode, and
		// construction mode uses a ton of GameObject.Find... Weird things start to happen!
		//TODO better fix than this maybe? I've been putting this one off for a while.
		foreach (Transform ii in GetComponentsInChildren<Transform>())
		{
			//ii.name = "NAME CHANGED TO PREVENT BUGS";
			ii.name += "_fix";
		}
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player")
        {
            Pickup.clip = pickupSound;
            Pickup.Play();
			SimpleData.WriteDataPoint("Pickup_Item", "", "", "", "", pickupName);
			//SimpleData.WriteStringToFile("pickups.txt", Time.time + ",PICKUP," + pickupName);
			switch (type)
			{
				case PickupType.Item:
					// Add the item and update the tokens.
					InventoryController.Add(this, 1);
					InventoryController.ConvertInventoryToTokens();

					// Poke the build button so it can check if it needs to update.
					BuildButton.CheckRecipes();

					if (pickupName.Contains("Rocket"))
					{
						ConversationTrigger.AddToken("picked_up_a_boots_piece");
					}
					if (pickupName.Contains("Sledge"))
					{
						ConversationTrigger.AddToken("picked_up_a_sledge_piece");
					}
					if (pickupName.Contains("Key1"))
					{
						ConversationTrigger.AddToken("picked_up_a_key1_piece");
					}
					if (pickupName.Contains("FFA"))
					{
						ConversationTrigger.AddToken("picked_up_a_ffa_piece");
					}

					// Object still needs to exist for the icon to work.
					// Silly, but let's just shove it into a corner and forget about it.
					// Also parents to the scene manager object so it rejects deletion as much as possible.
					transform.position = new Vector3(-1000f, -1000f, -1000f);
					LoadUtils.IconParenter(this.gameObject);
					break;

				case PickupType.Battery:
					BatterySystem.AddPower(2);
					BatterySystem.PowerToTokens();
					ConversationTrigger.AddToken("picked_up_a_battery");
					RespawnBattery();
					break;

				case PickupType.Clue:
					CluePopulator.AddClue(pickupName, clueSprite);
					ConversationTrigger.AddToken("clue_" + pickupName);
					ConversationTrigger.AddToken("picked_up_a_clue");
					Destroy(gameObject);
					break;
			}
			if (autoDelete)
			{
				ConversationTrigger.AddToken("autodelete_" + pickupName);
			}
        }
    }


	// Batteries will find a new position when picked up. Battery markers don't actually spawn anything,
	// they just mark positions. You have to have batteries in your scene for battery markers to matter.
	void RespawnBattery()
	{
		GameObject[] allMarkers = GameObject.FindGameObjectsWithTag("BatteryMarker");
		int randomIdx = Random.Range(0, allMarkers.Length);
		while (transform.position == allMarkers[randomIdx].transform.position)
		{
			randomIdx = Random.Range(0, allMarkers.Length);
			Debug.Log("Not letting battery spawn on same spot!");
		}	
		transform.position = allMarkers[randomIdx].transform.position;
	}


}
