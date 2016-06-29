using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationTrigger : MonoBehaviour
{
	[Header("Global Settings")]
	public string nameOfStarter;
	public string conversationName;
	public bool allowEscape = true;	// If true, you cannot press Esc to quit out of this conversation.
	public enum TriggerType { SimpleTrigger, ButtonTrigger, Instant, HardInstant };
	public TriggerType trigger;

	[Header("Button Trigger Settings")]
	public KeyCode keyRequired;

	[Header("Token Requirements")]
	public string[] tokenWhitelist;	// Tokens which the player must have for this conversation to trigger.
	public string[] tokenBlacklist; // Tokens which the player must not have for this conversation to trigger.

	// The trigger class makes the most sense to be the one to hold the Token set, so here it is.
	public static HashSet<string> tokens = new HashSet<string>{""};

	// Internal variables
	// For Instant trigger.
	float timer = 0f;

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
}
