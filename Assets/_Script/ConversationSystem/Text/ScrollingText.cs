using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class ScrollingText : MonoBehaviour
{
	// Intrinsic Variables
	Text textBase;
	SpriteText textController;
	Image[] letters;
	bool initializing = true;
	bool scrolling = true;
	bool conversationMode = false;
	bool nextLine = false;
	int conversationIdx = 0;

	float scrollTimer = 0f;
	public int numScrolled = 0;

	// Public Variables
	public float letterDelay = 0.1f; // The time between letters appearing.
	public string[] conversation;


	void Start ()
	{
		// Get and ensure the existence of the SpriteText controller.
		textController = GetComponent<SpriteText>();
		textBase = GetComponent<Text>();
		if (textController == null || textBase == null)
		{
			Debug.LogError("No sprite text / text component found. Deleting this script.");
			Destroy(this);
		}

		PrepConversation();

		// If the conversation array has any elements, treat this as a conversation with multiple lines and parse any tags it has.
		// If we don't parse here, everything breaks due to the fact that Unity's text requires us to refresh twice.
		if (conversation.Length != 0)
		{
			conversationMode = true;
			ParseTags();
			textBase.text = conversation[0];		
		}

		// This Start function effectively continues in PrepNextLine to ensure functions are called AFTER the text has been created.
		// Race conditions, basically. Who knew? Also apparently LateUpdate doesn't work how I thought it did.
	}

	void Update()
	{
		// DEBUG functionality
		if (Input.GetKeyDown(KeyCode.R))
		{
			//ApplyConversation(new string[] { "Test1", "Test2", "Test3" });
			ApplyConversation(new string[] { "THE SAME", "THE SAME", "THE SAME" });
		}

		// Skipping line scrolling or advancing a conversation.
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (scrolling)
			{
				EnableLetters();        // Enable everything, thereby skipping the scrolling effect.
				numScrolled = letters.Length;
			}
			else if (conversationMode && conversationIdx < conversation.Length - 1)
			{
				//Debug.Log("Going to next line");
				conversationIdx++;                              // Go to the next line.
				textBase.text = conversation[conversationIdx];  // Set that line.
				scrollTimer = 0f;                               // Reset scroll timer to allow another line to scroll in.
				numScrolled = 0;                                // Reset numScrolled for fresh state.

				nextLine = true;                                // This pokes LateUpdate, asking it to re-grab the text images.
			}
			else
			{
				// If we reach this case, the conversation is over and we can disable the text.
				DisableLetters();
			}

		}
		// Update Timer
		scrollTimer += Time.deltaTime;

		// Scrolling text effect.
		if (scrollTimer >= (numScrolled + 1) * letterDelay && numScrolled < letters.Length && !initializing && !nextLine)
		{
			//Debug.Log("Scrolling");
			scrolling = true;

			if (letters[numScrolled] != null)
				letters[numScrolled].enabled = true;

			numScrolled++;
			// TODO sound effect here.
		}
		// Keep track of whether text is currently scrolling or not.
		if (!initializing && numScrolled >= letters.Length)
		{
			scrolling = false;
		}
	}

	// This function is called by SpriteText when it is done creating its line.
	// It initiates the scrolling process.
	public void PrepNextLine (List<GameObject> letterObjects)
	{
		if (nextLine || initializing)
		{
			letters = new Image[letterObjects.Count];
			for (int i = 0; i < letterObjects.Count; i++)
			{
				letters[i] = letterObjects[i].GetComponentInChildren<Image>();
			}
			
			//Debug.Log(letters.Length);
			DisableLetters();

			nextLine = false;
			initializing = false;
		}
	}

	// Disable all letters.
	void DisableLetters ()
	{
		foreach (Image i in letters)
		{
			if (i != null)
			{
				i.enabled = false;
			}
		}
	}

	// Enable all letters.
	void  EnableLetters ()
	{
		foreach (Image i in letters)
		{
			if (i != null)
				i.enabled = true;
		}
	}

	// Parse all ease of use tags into their ascii garbage equivalents.
	void ParseTags ()
	{
		for (int i = 0; i < conversation.Length; i++)
		{
			while (conversation[i].Contains("[color]"))
			{
				StringBuilder tempText = new StringBuilder(conversation[i]);
				int idxStart = conversation[i].IndexOf("[color]");
				int numToRemove = 6;
				tempText.Remove(idxStart, numToRemove);
				tempText[idxStart] = '┤';
				conversation[i] = tempText.ToString();
			}
			while (conversation[i].Contains("[wave]"))
			{
				StringBuilder tempText = new StringBuilder(conversation[i]);
				int idxStart = conversation[i].IndexOf("[wave]");
				int numToRemove = 5;
				tempText.Remove(idxStart, numToRemove);
				tempText[idxStart] = '╡';
				conversation[i] = tempText.ToString();
			}
			while (conversation[i].Contains("[shake]"))
			{
				StringBuilder tempText = new StringBuilder(conversation[i]);
				int idxStart = conversation[i].IndexOf("[shake]");
				int numToRemove = 6;
				tempText.Remove(idxStart, numToRemove);
				tempText[idxStart] = '╢';
				conversation[i] = tempText.ToString();
			}
			while (conversation[i].Contains("[rando]"))
			{
				StringBuilder tempText = new StringBuilder(conversation[i]);
				int idxStart = conversation[i].IndexOf("[rando]");
				int numToRemove = 6;
				tempText.Remove(idxStart, numToRemove);
				tempText[idxStart] = '╖';
				conversation[i] = tempText.ToString();
			}
			while (conversation[i].Contains("[rbowwave]"))
			{
				StringBuilder tempText = new StringBuilder(conversation[i]);
				int idxStart = conversation[i].IndexOf("[rbowwave]");
				int numToRemove = 9;
				tempText.Remove(idxStart, numToRemove);
				tempText[idxStart] = '╕';
				conversation[i] = tempText.ToString();
			}
		}
	}

	// When two lines of the exact same text are next to each other, the system freezes 
	// since it can't detect the no-op of setting text to itself. This is a hack to prevent that.
	void PrepConversation()
	{
		if (conversation.Length >= 2)
		{
			for (int i = 0; i < conversation.Length - 1; i++)
			{
				if (conversation[i] == conversation[i + 1])
				{
					conversation[i + 1] += " ";
				}
			}
		}
	}

	// A function so other scripts can provide a conversation to play.
	public void ApplyConversation(string[] newConversation)
	{
		// Silly glitch again with no-op text assignment. See above function.
		if (newConversation[0] == textBase.text)
			newConversation[0] += " ";

		conversation = newConversation;
		Reset();
	}

	// Resets the conversation to the very beginning.
	// Also called by ApplyConversation to make the conversation it plays start from the beginning.
	public void Reset()
	{
		// Reset all internal variables to their initial values.
		initializing = true;
		scrolling = true;
		conversationMode = false;
		nextLine = false;
		conversationIdx = 0;
		scrollTimer = 0f;
		numScrolled = 0;

		// Then just kick the start function again.
		Start();
}
}
