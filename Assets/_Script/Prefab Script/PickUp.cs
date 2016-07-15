using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
	// Respawn settings.
	[Header("Basic Variables")]
	public bool respawn = true;
    public float respawnTime = 5.0f;

	// Pickup-type Settings.
	public string pickupName = "";	// Give it a name, consider this a type.
	public string pickupDesc = "";  // This is mostly an internal tag. Doesn't really get used.

	// SPECIAL
	// Only necessary for clue:
	[Header("Special Variables")]
	public bool isClue = false;
	public Sprite clueSprite;

	[Tooltip("If the token specified here exists, this pickup will not spawn.")]
	public string deleteToken = "";

	void Start()
	{
		// check deleteToken.
		if (deleteToken != "" && ConversationTrigger.GetToken(deleteToken))
			Destroy(gameObject);

		// We need to rename all the bits under the model of the pickup, if there are any.
		// The reason for this is names will conflict with names inside construction mode, and
		// construction mode uses a ton of GameObject.Find... Weird things start to happen!
		//TODO better fix than this maybe? I've been putting this one off for a while.
		foreach (Transform ii in GetComponentsInChildren<Transform>())
		{
			ii.name = "NAME CHANGED TO PREVENT BUGS";
		}
	}

    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player")
        {
			// Special case for batteries.
			if (pickupName == "Battery")
			{
				BatterySystem.AddPower(2);
				RespawnBattery();
			}
			else if (isClue)
			{
				CluePopulator.AddClue(pickupName, clueSprite);
				ConversationTrigger.AddToken("clue_" + pickupName);
				Destroy(gameObject);
			}
			else
			{
				InventoryController.Add(this, 1);
				if (respawn)
				{
					ObjectRespawner.Instance().RespawnOBJ(this.gameObject, respawnTime);
				}
				else
				{
					// Object still needs to exist for the icon to work.
					// Silly, but let's just shove it into a corner and forget about it.
					// Also parents to the scene manager object so it rejects deletion as much as possible.
					transform.position = new Vector3(-1000f, -1000f, -1000f);
					LoadUtils.IconParenter(this.gameObject);
				}
			}
        }
    }


	// Batteries will find a new position when picked up. Battery markers don't actually spawn anything,
	// they just mark positions. You have to have batteries in your scene for battery markers to matter.
	void RespawnBattery()
	{
		GameObject[] allMarkers = GameObject.FindGameObjectsWithTag("BatteryMarker");
		int randomIdx = Random.Range(0, allMarkers.Length);
		transform.position = allMarkers[randomIdx].transform.position;
	}


}
