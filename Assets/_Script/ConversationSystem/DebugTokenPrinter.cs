using UnityEngine;
using System.Collections;

public class DebugTokenPrinter : MonoBehaviour
{
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			foreach (string ii in ConversationTrigger.tokens)
			{
				Debug.Log(ii);
			}
			Debug.Log(ConversationTrigger.GetToken("dreshaReadyToFlashPyr"));
		}
	}
}
