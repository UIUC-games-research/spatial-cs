using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationTrigger : MonoBehaviour
{
	[Header("Global Settings")]
	[Tooltip("The name that appears in the top bar. Bar will not exist if left blank.")]
	public string nameOfStarter;
	[Tooltip("Internal name / file name of conversation to play")]
	public string conversationName;
	public bool allowEscape = true; // If true, you can press Esc to quit out of this conversation.
	[Tooltip("Conversation can only be started once if true. Best used with disallowed escape.")]
	public bool oneShot = false;	// If true, this trigger will automatically destroy itself once it completes a conversation.
	public enum TriggerType { SimpleTrigger, ButtonTrigger, Instant, HardInstant };
	public TriggerType trigger;

	[Header("Button Trigger Settings")]
	[Tooltip("Only used for Button Trigger type.")]
	public KeyCode keyRequired;

	[Header("Token Requirements")]
	public string[] tokenWhitelist;	// Tokens which the player must have for this conversation to trigger.
	public string[] tokenBlacklist; // Tokens which the player must not have for this conversation to trigger.

	// The trigger class makes the most sense to be the one to hold the Token set, so here it is.
	public static HashSet<string> tokens = new HashSet<string>{""};

	// Internal variables
	// For Instant trigger.
	float timer = 0f;

	void Start ()
	{
		if (oneShot && GetToken("oneShot_" + conversationName))
		{
			Destroy(this);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && trigger == TriggerType.SimpleTrigger && CheckTokens())
		{
			ConversationController.Enable(this);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.tag == "Player" && trigger == TriggerType.ButtonTrigger && Input.GetKey(keyRequired) && CheckTokens())
		{
			ConversationController.Enable(this);
		}
	}

	void FixedUpdate()
	{
		if (trigger == TriggerType.Instant || trigger == TriggerType.HardInstant)
		{
			timer += Time.deltaTime;
			if (timer > 0.25f && CheckTokens() && (!ConversationController.currentlyEnabled || trigger == TriggerType.HardInstant))
			{
				timer = 0f;
				Debug.Log("Token already exists in dict: " + conversationName);

				if (trigger == TriggerType.HardInstant)
				{
					tokens.Add("HardInstant_" + conversationName);
				}
				ConversationController.Enable(this);
			}
			else if (timer > 0.5f)
			{
				timer = 0f;
			}
		}
	}

	bool CheckTokens()
	{
		foreach (string ii in tokenWhitelist)
		{
			if (!tokens.Contains(ii))
				return false;
		}

		foreach (string ii in tokenBlacklist)
			if (tokens.Contains(ii))
				return false;

		// Stop HardInstant conversations from overwriting themselves.
		if (trigger == TriggerType.HardInstant && ConversationController.currentlyEnabled)
		{
			return !tokens.Contains("HardInstant_" + conversationName);
		}

		return true;
	}

	// Use this to add a token.
	public static void AddToken(string token)
	{
		tokens.Add(token);
		SaveController.Save();
	}
	public static void AddToken(string token, bool save)
	{
		tokens.Add(token);
		if (save)
			SaveController.Save();
	}

	public static void RemoveToken(string token)
	{
		if (tokens.Contains(token))
		{
			tokens.Remove(token);
			SaveController.Save();
		}
	}
	public static void RemoveToken(string token, bool save)
	{
		if (tokens.Contains(token))
		{
			tokens.Remove(token);
			if (save)
				SaveController.Save();
		}
	}

	// Returns true if player has token.
	public static bool GetToken(string token)
	{
		return tokens.Contains(token);
	}
}
