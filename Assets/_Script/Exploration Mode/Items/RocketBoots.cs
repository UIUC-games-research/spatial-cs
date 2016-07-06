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


	void Start ()
	{
		playerBody = GetComponent<Rigidbody>();
		uiElementStatic = uiElement;

		// Initially disabled.
		if (!bootsActive)
			uiElement.gameObject.SetActive(false);
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

	public static void ActivateBoots()
	{
		bootsActive = true;
		uiElementStatic.gameObject.SetActive(true);
	}
}
