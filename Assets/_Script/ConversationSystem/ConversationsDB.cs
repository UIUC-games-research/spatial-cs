using UnityEngine;
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
		}

	};
}
