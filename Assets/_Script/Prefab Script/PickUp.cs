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
				transform.position = new Vector3(-1000f, -1000f, -1000f);
			}

        }
    }



}
