using UnityEngine;
using System.Collections;

public class ObjectRespawner : MonoBehaviour {
    //Returns static instance
    private static ObjectRespawner current;
    public ObjectScatterer objScatterer;
    public static ObjectRespawner Instance()
    {
        if (!current)
        {
            if (!current)
            {
                current = FindObjectOfType(typeof(ObjectRespawner)) as ObjectRespawner;
                if (!current)
                    Debug.LogError("There needs to be one active script, and there isn't any to be found.");
            }

        }
        return current;

    }

    public void RespawnOBJ(GameObject obj, float respawnTime)
    {
        StartCoroutine(Respawn(obj, respawnTime));
    }

    IEnumerator Respawn(GameObject obj, float second)
    {
        obj.gameObject.SetActive(false);
        yield return new WaitForSeconds(second);
        obj.gameObject.transform.position = objScatterer.ReturnRandomPosition();
        obj.gameObject.SetActive(true);
    }
}
