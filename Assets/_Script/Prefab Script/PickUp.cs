using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public bool respawn = true;
    public float respawnTime = 5.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameController.Instance().Score();
            if (respawn)
                ObjectRespawner.Instance().RespawnOBJ(this.gameObject, respawnTime);
        }
    }



}
