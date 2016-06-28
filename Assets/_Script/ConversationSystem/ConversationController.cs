using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConversationController : MonoBehaviour
{
	// There should only ever be one of these in a scene, so lots of static methods!
	// References
	static GameObject thisObject;
	static ScrollingText textBox;
	static UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController player;

	static Text starterName;
	static Image nameBox;
	static Image choiceContainer;

	// Inspector-set references. Made static in start.
	public Text starterNameRef;
	public Image nameBoxRef;

	// Internal variables.
	static float sensitivityXTemp = 0; // Used to store sensitivity settings while conversation is open.
	static float sensitivityYTemp = 0;

	void Start ()
	{
		// Grab references.
		thisObject = gameObject;
		textBox = GetComponentInChildren<ScrollingText>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
		starterName = starterNameRef;
		nameBox = nameBoxRef;

		// Set initial values of sensitivity temps.
		sensitivityXTemp = player.mouseLook.XSensitivity;
		sensitivityYTemp = player.mouseLook.YSensitivity;

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
		LockMouse();
	}

	// Just like the previous function, only it will not set the conversation to nowhere.
	// This is useful for when nowhere already IS the conversation, being set from a choice.
	// It allows the box to close safely.
	public static void SoftDisable()
	{
		FakeActive(thisObject, false);
		LockMouse();
		SetStarterName("");
	}

	// Enable the text box with a specific conversation loaded.
	public static void Enable(string conversationName)
	{
		textBox.ApplyConversation(ConversationsDB.convos[conversationName]);
		if (conversationName == "nowhere")
		{
			SoftDisable();
			return;
		}
		FakeActive(thisObject, true);
		AllowMouse();
	}

	// Enable the text box with a specific conversation loaded.
	public static void Enable(string conversationName, string conversationStarter)
	{
		textBox.ApplyConversation(ConversationsDB.convos[conversationName]);
		if (conversationName == "nowhere")
		{
			SoftDisable();
			return;
		}
		SetStarterName(conversationStarter);
		FakeActive(thisObject, true);
		AllowMouse();
	}

	// Enable the text box, supplying a trigger. This is generally better, since it will also get name information.
	public static void Enable(ConversationTrigger trigger)
	{
		textBox.ApplyConversation(ConversationsDB.convos[trigger.conversationName]);
		if (trigger.conversationName == "nowhere")
		{
			SoftDisable();
			return;
		}
		SetStarterName(trigger.nameOfStarter);
		FakeActive(thisObject, true);
		AllowMouse();
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

	// Allows you to move your mouse around the screen to select a choice or something.
	// Also disables player movement.
	static void AllowMouse()
	{
		player.mouseLook.SetCursorLock(false);
		if (player.mouseLook.XSensitivity != 0)
		{
			sensitivityXTemp = player.mouseLook.XSensitivity;
			sensitivityYTemp = player.mouseLook.YSensitivity;
		}
		player.mouseLook.XSensitivity = 0;
		player.mouseLook.YSensitivity = 0;

		UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController.allowMovement = false;
	}

	// Re-enables all aspects of the player controller.
	static void LockMouse()
	{
		player.mouseLook.SetCursorLock(true);
		player.mouseLook.XSensitivity = sensitivityXTemp;
		player.mouseLook.YSensitivity = sensitivityYTemp;

		UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController.allowMovement = true;
	}
}
