using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    private static ObjectPooler current;


    public List<GameObject> poolObjectList;
    public List<int> poolAmount;
    public bool willGrow = true;

    List<List<GameObject>> mainPoolList = new List<List<GameObject>>();

    public static ObjectPooler Instance()
    {
        if (!current)
        {
            if (!current)
            {
                current = FindObjectOfType(typeof(ObjectPooler)) as ObjectPooler;
                if (!current)
                    Debug.LogError("There needs to be one active script, and there isn't any to be found.");
            }

        }
        return current;
    }

    // Use this for initialization
    void Awake()
    {
        current = this;
        for (int i = 0; i < poolObjectList.Count; i++)
        {
            List<GameObject> t = InitiatePool(poolObjectList[i], poolAmount[i]);
            mainPoolList.Add(t);
        }
    }

    void Start()
    {


    }

    public GameObject GetPooledObject(int poolIndex)
    {
       
        return InactiveSearch(mainPoolList[poolIndex], poolObjectList[poolIndex]);
    }


    private GameObject InactiveSearch(List<GameObject> targetPool, GameObject targetObject)
    {
        for (int i = 0; i < targetPool.Count; i++)
        {
            if (!targetPool[i].activeInHierarchy)
            {
                //targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(targetObject);
           // obj.SetActive(true);
            targetPool.Add(obj);
            return obj;
        }
        return null;
    }

    private List<GameObject> InitiatePool(GameObject targetObject, int pooledAmount)
    {
        List<GameObject> targetPool = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(targetObject);
            obj.SetActive(false);
            targetPool.Add(obj);
        }
        return targetPool;
    }
}
