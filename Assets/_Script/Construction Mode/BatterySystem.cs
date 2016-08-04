using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BatterySystem : MonoBehaviour
{
	// Reference.
	Text indicator;

	// counter for battery power.
	static int batteryPower = 10;

	public static int GetPower()
	{
		return batteryPower;
	}

	public static int AddPower(int power)
	{
		batteryPower += power;
		return batteryPower;
	}

	public static int SubPower(int power)
	{
		batteryPower -= power;
		if (batteryPower < 0)
			batteryPower = 0;
		return batteryPower;
	}

	// Called when battery is picked up.
	public static void PowerToTokens()
	{
		// Remove battery token. (anything containing "battery|")
		List<string> toRemove = new List<string>(); // Need a list because you cannot modify a hashset while iterating over it.
		foreach (string ii in ConversationTrigger.tokens)
		{
			if (ii.Contains("battery|"))
				toRemove.Add(ii);
		}
		foreach (string ii in toRemove)
		{
			ConversationTrigger.RemoveToken(ii, false);
		}

		// Add the new token.
		ConversationTrigger.AddToken("battery|" + batteryPower);
	}

	// Called by load.
	public static void TokensToPower()
	{
		// Find the battery token, split and set power.
		string batString = "battery|10";
		foreach (string ii in ConversationTrigger.tokens)
		{
			if (ii.Contains("battery|"))
				batString = ii;
		}
		string[] separated = batString.Split(new char[] { '|' });
		batteryPower = int.Parse(separated[1]);
	}

	// May as well use this script for indicators, too!
	void Start ()
	{
		indicator = GetComponent<Text>();
	}

	void FixedUpdate ()
	{
		indicator.text = batteryPower.ToString();
	}
}
