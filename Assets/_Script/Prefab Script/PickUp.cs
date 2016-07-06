using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
	// Respawn settings.
    public bool respawn = true;
    public float respawnTime = 5.0f;

	// Pickup-type Settings.
	public string pickupName = "";	// Give it a name, consider this a type.
	public string pickupDesc = "";	// Give it a description.

	void Start()
	{
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
        //if (other.gameObject.CompareTag("Player"))
		if (other.tag == "Player")
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
