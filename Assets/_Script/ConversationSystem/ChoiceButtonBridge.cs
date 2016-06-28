using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChoiceButtonBridge : MonoBehaviour
{
	public string choiceText;
	public string choicePointer;

	void Start()
	{
		GetComponentInChildren<Text>().text = choiceText;
		GetComponent<Button>().onClick.AddListener(() =>    // Adds an event to the button
		{
			ConversationController.Enable(choicePointer);
		});
	}
}
