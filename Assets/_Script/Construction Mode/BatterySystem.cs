using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BatterySystem : MonoBehaviour
{
	// Reference.
	Text indicator;

	// counter for battery power.
	static int batteryPower = 15;

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
