using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationsDB : MonoBehaviour
{
	// All conversations are stored in a dictionary, with a string name to look them up by.
	public static Dictionary<string, string[]> convos = new Dictionary<string, string[]>
	{		
		// This nowhere conversation comes in handy for choices, if you want one of the choices to end the conversation.
		// Also, don't delete it because parts of the system use it.
		{
			"nowhere",
			new string [0]
		},

		// Here is a generic test conversation. It loops itself (single choice, points to its own name.)
		// Also shows off a few of the fun effects.
		{
			"testConversation",
			new string []
			{
				"Hello! this is a new conversation.",
				"The syntax is a little strange, but it honestly looks better than I had expected.",
				"There are a bunch of tags you can use in conversations, some of them are more customizeable than others.",
				"[wave]This is a fun one.",
				"[shake]You can manually close tags[shake] by typing them again.",
				"[rbowwave]This one is my favorite.",
				"When you want to make a choice, insert the [color]900[CHOOSE][color] tag as the [color]900only thing[color] on a line, everything afterwards will be parsed as a choice",
				"[CHOOSE]",
				"RANDOM ANSWER|testConversation"
			}
		},

		// A test of choices.
		{
			"choiceTest",
			new string []
			{
				"This is a test of the choice system.",
				"[CHOOSE]",
				"Okay|nowhere",
				"What?|choiceTest"
			}
		},

		// A test of choice tokens.
		{
			"tokenTest",
			new string []
			{
				"This is a token test. Respond with yes and then talk to the nearby platform.",
				"You can also press 'I' now to test the Hard Instant trigger.",
				"[CHOOSE]",
				"Yes|nowhere|tokenTestToken",
				"No|nowhere"
			}
		},

		// For when you do not have the "tokenTestToken"
		{
			"tokenTestNegative",
			new string []
			{
				"You have either not talked to the nearby platform, or responded no to it."
			}
		},

		// For when you have the "tokenTestToken"
		{
			"tokenTestPositive",
			new string []
			{
				"You have talked to the platform near this one, and responded yes!"
			}
		},

		// Test conversation for an Instant trigger.
		{
			"instantTest",
			new string []
			{
				"This is an instant conversation.",
				"It happens automatically, but only when not already in a conversation.",
				"[CHOOSE]",
				"nowhere|nowhere|instantTestDone"
			}
		},

		// Test conversation for a Hard Instant trigger.
		{
			"hardInstantTest",
			new string []
			{
				"This is a hard instant conversation.",
				"It will happen as soon as its conditons are met, no matter what.",
				"This also means it will override any current conversation you are in.",
				"[CHOOSE]",
				"nowhere|nowhere|hardInstantTestDone"
			}
		}

	};


	// Alternatively, load them in from text files.
	// This function is automatically called by the SaveController.Load function.
	static TextAsset[] conversationFiles = Resources.LoadAll<TextAsset>("Conversations");
	public static void LoadConversationsFromFiles()
	{
		foreach (TextAsset ii in conversationFiles)
		{
			if (!convos.ContainsKey(ii.name))
				convos.Add(ii.name, ii.text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));
			else
				Debug.LogError("Attempting to add a key which already exists. This is usually a bad thing.");
		}
	}
}
