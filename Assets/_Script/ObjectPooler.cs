using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
	static ObjectPooler current;
	List<List<GameObject>> mainPoolList = new List<List<GameObject>>();
	public List<int> poolAmount;
	public List<GameObject> poolObjectList;
	public bool willGrow = true;
	public static ObjectPooler Instance () {
		if (!current) {
			if (!current) {
				current = FindObjectOfType(typeof(ObjectPooler)) as ObjectPooler;
				if (!current)
					Debug.LogError("There needs to be one active script, and there isn't any to be found.");
			}
		}
		return current;
	}

	// Use this for initialization
	void Awake () {
		current = this;
		for (var i = 0; i < poolObjectList.Count; i++) {
			var t = InitiatePool(poolObjectList[i], poolAmount[i]);
			mainPoolList.Add(t);
		}
	}
	void Start () {}
	public GameObject GetPooledObject (int poolIndex) {return InactiveSearch(mainPoolList[poolIndex], poolObjectList[poolIndex]);}
	GameObject InactiveSearch (List<GameObject> targetPool, GameObject targetObject) {
		for (var i = 0; i < targetPool.Count; i++) {
			if (!targetPool[i].activeInHierarchy) {
				//targetPool[i].SetActive(true);
				return targetPool[i];
			}
		}
		if (willGrow) {
			var obj = Instantiate(targetObject);
			// obj.SetActive(true);
			targetPool.Add(obj);
			return obj;
		}
		return null;
	}
	List<GameObject> InitiatePool (GameObject targetObject, int pooledAmount) {
		var targetPool = new List<GameObject>();
		for (var i = 0; i < pooledAmount; i++) {
			var obj = Instantiate(targetObject);
			obj.SetActive(false);
			targetPool.Add(obj);
		}
		return targetPool;
	}
}
