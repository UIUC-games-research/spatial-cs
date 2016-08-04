using UnityEngine;
using System.Collections;

public class LoadTrigger : MonoBehaviour
{
	public string levelName;
	public string tokenRequired = "";
	public Vector3 spawnPosition;
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (tokenRequired == "")
				LoadUtils.LoadNewExplorationLevel(levelName, spawnPosition);
			else if (ConversationTrigger.GetToken(tokenRequired))
			{
				LoadUtils.LoadNewExplorationLevel(levelName, spawnPosition);
			}
		}
	}
}
