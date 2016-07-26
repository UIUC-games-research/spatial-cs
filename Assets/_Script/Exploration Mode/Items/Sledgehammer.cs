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
			BreakFX(other.gameObject);
			Destroy(other.gameObject);
		}
	}

	void BreakFX(GameObject toMimic)
	{
		// Get the Renderer for the object we're breaking so we can mimic its material.
		MeshRenderer meshr = toMimic.GetComponent<MeshRenderer>();
		if (meshr == null)
		{
			Debug.Log("Didn't find a mesh on top level of object attempting to be broken.");
			return;
		}

		GameObject spawned = Resources.Load<GameObject>("Prefabs/DebrisBase");
		for (int i = 0; i < 20; i++)
		{
			GameObject instance = LoadUtils.InstantiateParenter(Instantiate(spawned));
			Rigidbody rb = instance.GetComponent<Rigidbody>();
			MeshRenderer mr = instance.GetComponent<MeshRenderer>();
			mr.material = meshr.material;
			Vector3 newPos = transform.position;
			newPos.y += 1f;
			instance.transform.position = newPos;
			instance.transform.localScale = new Vector3(Random.Range(0.5f, 4f), Random.Range(0.5f, 4f), Random.Range(0.5f, 4f));
			rb.AddTorque(Random.Range(-10f, 10f), Random.Range(0f, 15f), Random.Range(-10f, 10f), ForceMode.Impulse);
			rb.AddForce(Random.Range(-10f, 10f), Random.Range(0f, 15f), Random.Range(-10f, 10f), ForceMode.Impulse);
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
