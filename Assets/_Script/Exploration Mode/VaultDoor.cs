using UnityEngine;
using System.Collections;

public class VaultDoor : MonoBehaviour
{
	public string tokenToCheck = "";
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && checkToken())
		{
			StartCoroutine(rotate(90f));
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" && checkToken())
		{
			StartCoroutine(rotate(-90f));
		}
	}
	bool checkToken()
	{
		return ConversationTrigger.GetToken(tokenToCheck);
	}
	IEnumerator rotate(float amount)
	{
		for (int i = 0; i < 60; i++)
		{
			transform.Rotate(0f, amount / 60f, 0f, Space.World);
			yield return null;
		}
	}
}
