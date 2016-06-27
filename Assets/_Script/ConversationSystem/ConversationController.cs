using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConversationController : MonoBehaviour
{
	// There should only ever be one of these in a scene, so lots of static methods!
	// References
	static GameObject thisObject;
	static ScrollingText textBox;
	static Text starterName;
	static Image nameBox;
	
	void Start ()
	{
		// Grab references.
		thisObject = gameObject;
		textBox = GetComponentInChildren<ScrollingText>();
		starterName = GetComponentInChildren<Text>();
		nameBox = GetComponentsInChildren<Image>()[1];

		// Disable to start with.
		FakeActive(gameObject, false);
	}
	
	
	void Update ()
	{
		// Ability to cancel a conversation.
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Disable();
		}
	}


	// Disable the text box by applying the nowhere conversation and pushing it really far away.
	// Also clears the name of whoever started the conversation.
	public static void Disable()
	{
		textBox.ApplyConversation(ConversationsDB.convos["nowhere"]);
		SetStarterName("");
		FakeActive(thisObject, false);
	}

	// Enable the text box with a specific conversation loaded.
	public static void Enable(string conversationName)
	{
		textBox.ApplyConversation(ConversationsDB.convos[conversationName]);
		SetStarterName("");
		FakeActive(thisObject, true);
	}

	// Enable the text box, supplying a trigger. This is generally better, since it will also get name information.
	public static void Enable(ConversationTrigger trigger)
	{
		textBox.ApplyConversation(ConversationsDB.convos[trigger.conversationName]);
		SetStarterName(trigger.nameOfStarter);
		FakeActive(thisObject, true);
	}

	// Set the name box to whoever started this conversation.
	static void SetStarterName(string name)
	{
		starterName.text = name;
		if (name != "")
			FakeActive(nameBox.gameObject, true);
		else
			FakeActive(nameBox.gameObject, false);
	}

	static void FakeActive(GameObject go, bool active)
	{
		Vector3 pos = go.transform.localPosition;
		if (!active)
		{
			pos.z = 80000;
			go.transform.localPosition = pos;
		}
		else
		{
			pos.z = 0;
			go.transform.localPosition = pos;
		}
	}
}
