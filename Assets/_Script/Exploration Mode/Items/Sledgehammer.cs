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

	void Start ()
	{
		sledgehammerStatic = sledgehammer;

		// Check token and activate if unlocked.
		if (ConversationTrigger.GetToken("gear_sledgehammer"))
		{
			ActivateSledgehammer();
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
		// DEBUG.
		if (Input.GetKeyDown(KeyCode.H))
		{
			ActivateSledgehammer();
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
		}
	}
}
