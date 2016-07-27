using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class RocketBoots : ItemBase
{
	// References
	Rigidbody playerBody;
	public Slider uiElement;

	// Internal
	static bool bootsActive = false;
	static Slider uiElementStatic;
	public float charge = 100f;
	public float power = 150f;
	public float drain = 1.5f;
	public float recharge = 1f;

	bool selected = false;

	void Awake ()
	{
		playerBody = GetComponent<Rigidbody>();
		uiElementStatic = uiElement;
	}

	void Start ()
	{
		// Check tokens to see if boots are active.
		// Save is loaded somewhere else in an Awake() function.
		if (ConversationTrigger.GetToken("gear_rocketboots"))
		{
			ActivateBoots();

			// This is the first item you get, and it will be the one that always starts active when you load a game.
			// Therefore, we need this line here.
			ItemManager.SelectGear(0);
		}
		else
		{
			uiElement.gameObject.SetActive(false);
		}
	}

	// Restart when re-enabled. Fixes saving bugs.
	void OnEnable()
	{
		Start();
	}
	
	void FixedUpdate ()
	{
		if (bootsActive && selected)
		{

			if (Input.GetKey(KeyCode.Space) && charge > 0)
			{
				if (charge > 10)
				{
					playerBody.AddForce(power * Vector3.up);
				}
				charge -= drain;
			}
			else if (charge < 100)
			{
				charge += recharge;
			}
			uiElement.value = charge;

		}
	}

	// When activating the boots, we add a token as a form of saving progress.
	// We check for this token at start, which will check the loaded options,
	// and enable the rocket boots if we've unlocked them before.
	public static void ActivateBoots()
	{
		bootsActive = true;
		uiElementStatic.gameObject.SetActive(true);
		ConversationTrigger.AddToken("gear_rocketboots");
	}

	public static bool GetBootsActive()
	{
		return bootsActive;
	}

	// Sometimes, things don't happen in time. Specifically, this static reference conversion.
	void GetRef()
	{
		if (uiElementStatic == null)
			uiElementStatic = uiElement;
	}

	public override void Deselect()
	{
		if (bootsActive)
		{
			GetRef();
			// Hide the UI element when deselected, and set a flag.
			uiElementStatic.gameObject.SetActive(false);
			selected = false;
		}
	}
	public override void Select()
	{
		if (bootsActive)
		{
			GetRef();
			// Re-show the UI element when selected, and set a flag.
			uiElementStatic.gameObject.SetActive(true);
			selected = true;
		}
	}
}
