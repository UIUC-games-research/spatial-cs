using UnityEngine;
using System.Collections;

public class LoadTrigger : MonoBehaviour
{
	public string levelName;
	public Vector3 spawnPosition;
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			LoadUtils.LoadNewExplorationLevel(levelName, spawnPosition);
		}
	}
}
