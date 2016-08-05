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
	public static bool currentlyEnabled = false;
	public static string currentConversationName = "";
	public static bool currentEscRule = true;

	void Start ()
	{
		// Grab references.
		thisObject = gameObject;
		textBox = GetComponentInChildren<ScrollingText>();
		if (GameObject.FindGameObjectWithTag("Player") != null)
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
		}
		starterName = starterNameRef;
		nameBox = nameBoxRef;

		// Set initial values of sensitivity temps.
		if (player != null)
		{
			sensitivityXTemp = player.mouseLook.XSensitivity;
			sensitivityYTemp = player.mouseLook.YSensitivity;
		}

		// Disable to start with.
		FakeActive(gameObject, false);
	}

	void OnEnable()
	{
		// Fresh state every time this object is activated.
		Start();
	}
	
	
	void Update ()
	{
		// Ability to cancel a conversation.
		if (Input.GetKeyDown(KeyCode.Escape) && currentEscRule)
		{
			Disable();
		}

		if (Input.GetKeyDown(KeyCode.I) && currentConversationName == "tokenTest")
		{
			ConversationTrigger.AddToken("hardInstantTest");
		}
	}


	// Disable the text box by applying the nowhere conversation and pushing it really far away.
	// Also clears the name of whoever started the conversation.
	public static void Disable()
	{
		textBox.ApplyConversation(ConversationsDB.convos["nowhere"]);
		currentConversationName = "nowhere";
		SetStarterName("");
		FakeActive(thisObject, false);
		currentlyEnabled = false;
		LockMouse();
	}

	// Just like the previous function, only it will not set the conversation to nowhere.
	// This is useful for when nowhere already IS the conversation, being set from a choice.
	// It allows the box to close safely.
	public static void SoftDisable()
	{
		currentConversationName = "nowhere";
		FakeActive(thisObject, false);
		currentlyEnabled = false;
		LockMouse();
		SetStarterName("");
	}

	// Enable the text box with a specific conversation loaded.
	public static void Enable(string conversationName)
	{
		textBox.ApplyConversation(ConversationsDB.convos[conversationName]);
		currentConversationName = conversationName;
		if (conversationName == "nowhere")
		{
			SoftDisable();
			return;
		}
		FakeActive(thisObject, true);
		currentlyEnabled = true;
		AllowMouse();
	}

	// Enable the text box with a specific conversation loaded.
	public static void Enable(string conversationName, string conversationStarter)
	{
		textBox.ApplyConversation(ConversationsDB.convos[conversationName]);
		currentConversationName = conversationName;
		if (conversationName == "nowhere")
		{
			SoftDisable();
			return;
		}
		SetStarterName(conversationStarter);
		FakeActive(thisObject, true);
		currentlyEnabled = true;
		AllowMouse();
	}

	// Enable the text box, supplying a trigger. This is generally better when possible, since it will set name / escape rule.
	public static void Enable(ConversationTrigger trigger)
	{
		// Make sure we didn't lose our ref somehow...
		if (textBox == null || textBox.isActiveAndEnabled == false)
		{
			textBox = thisObject.GetComponentInChildren<ScrollingText>();
			textBox.enabled = true;
		}

		// Make sure the dictionary is prepped if a "bad" key is given.
		if (!ConversationsDB.convos.ContainsKey(trigger.conversationName))
		{
			ConversationsDB.LoadConversationsFromFiles();
		}
		textBox.ApplyConversation(ConversationsDB.convos[trigger.conversationName]);
		currentConversationName = trigger.conversationName;
		currentEscRule = trigger.allowEscape;
		if (trigger.conversationName == "nowhere")
		{
			SoftDisable();
			return;
		}
		SetStarterName(trigger.nameOfStarter);
		FakeActive(thisObject, true);
		currentlyEnabled = true;
		AllowMouse();

		// Oneshot destroys the trigger and marks it with a token so it never comes back again. Ever.
		// You'll have to delete the save file to have it trigger again.
		if (trigger.oneShot)
		{
			Destroy(trigger);
			ConversationTrigger.AddToken("oneShot_" + trigger.conversationName);
		}
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
		if (player != null)
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
	}

	// Re-enables all aspects of the player controller.
	static void LockMouse()
	{
		if (player != null)
		{
			player.mouseLook.SetCursorLock(true);
			player.mouseLook.XSensitivity = sensitivityXTemp;
			player.mouseLook.YSensitivity = sensitivityYTemp;

			UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController.allowMovement = true;
		}
	}
}
