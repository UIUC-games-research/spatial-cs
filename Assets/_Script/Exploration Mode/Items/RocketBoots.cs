using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RocketBoots : MonoBehaviour
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


	void Awake ()
	{
		playerBody = GetComponent<Rigidbody>();
		uiElementStatic = uiElement;

		Debug.Log((int)'’');
		Debug.Log((int)'\'');


		// Initially disabled. ’
		if (!bootsActive)
			uiElement.gameObject.SetActive(false);
	}

	void Start ()
	{
		// Check tokens to see if boots are active.
		// Save is loaded somewhere else in an Awake() function.
		if (ConversationTrigger.GetToken("gear_rocketboots"))
			ActivateBoots();
	}
	
	void FixedUpdate ()
	{
		if (bootsActive)
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
}
