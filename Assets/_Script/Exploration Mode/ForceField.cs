using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour
{
	Collider coll;
	MeshRenderer render;

	void Start ()
	{
		coll = GetComponent<Collider>();
		render = GetComponent<MeshRenderer>();
	}

	public string tokenToCheck = "";
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && checkToken())
		{
			coll.enabled = false;
			render.enabled = false;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player" && checkToken())
		{
			coll.enabled = true;
			render.enabled = true;
		}
	}
	bool checkToken()
	{
		return ConversationTrigger.GetToken(tokenToCheck);
	}
}
