using UnityEngine;
using System.Collections;

public class Sledgehammer : ItemBase
{
	// References
	public GameObject sledgehammer;

	// Internal
	static bool sledgeActive = false;
	static GameObject sledgehammerStatic;

	bool selected = false;
	bool swinging = false;

	void Start ()
	{
		sledgehammerStatic = sledgehammer;

		// Check token and activate if unlocked, but deselected.
		if (ConversationTrigger.GetToken("gear_sledgehammer"))
		{
			ActivateSledgehammer();
			Deselect();
		}
		else
		{
			// Disable if not unlocked.
			sledgehammerStatic.SetActive(false);
		}
	}

	// Restart when re-enabled. Fixes saving bugs.
	void OnEnable()
	{
		Start();
	}

	void Update ()
	{
		if (sledgeActive && selected)
		{
			// Press left mouse to swing.
			if (Input.GetMouseButtonDown(0))
			{
				StartCoroutine(Swing());
			}
		}
	}

	IEnumerator Swing()
	{
		swinging = false;
		for (int i = 0; i < 10; i++)
		{
			sledgehammer.transform.Rotate(90f / 10f, 0f, 0f, Space.Self);
			if (i > 5)
				swinging = true;
			else
				swinging = false;
			yield return null;
		}
		swinging = false;
		for (int i = 0; i < 15; i++)
		{
			sledgehammer.transform.Rotate(-90f / 15f, 0f, 0f, Space.Self);
			yield return null;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (swinging && other.tag == "Breakable")
		{
			Destroy(other.gameObject);
		}
	}

	public static void ActivateSledgehammer()
	{
		sledgeActive = true;
		sledgehammerStatic.SetActive(true);
		ConversationTrigger.AddToken("gear_sledgehammer");
	}

	// Sometimes, things don't happen in time. Specifically, this static reference conversion.
	void GetRef()
	{
		if (sledgehammerStatic == null)
			sledgehammerStatic = sledgehammer;
	}

	public override void Deselect()
	{
		if (sledgeActive)
		{
			GetRef();
			// Hide the sledge when deselected, and set a flag.
			sledgehammerStatic.gameObject.SetActive(false);
			selected = false;
			swinging = false;
		}
	}
	public override void Select()
	{
		if (sledgeActive)
		{
			GetRef();
			// Re-show the sledgewhen selected, and set a flag.
			sledgehammerStatic.gameObject.SetActive(true);
			selected = true;
			swinging = false;
		}
	}
}
