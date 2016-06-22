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
        if (other.gameObject.CompareTag("Player"))
        {
			InventoryController.Add(this, 1);
			if (respawn)
                ObjectRespawner.Instance().RespawnOBJ(this.gameObject, respawnTime);
        }
    }



}
