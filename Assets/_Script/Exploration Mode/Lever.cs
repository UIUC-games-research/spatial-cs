using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
	public GameObject[] doors;  // Set in inspector. All doors this switch opens.

	public Collider thisCollider;   // Set in inspector.
	public GameObject leverObject;  // Set in inspector.

	public GameObject player;
	bool leverState = false;
	bool rotating = false;

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			Debug.DrawLine(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).origin + (100 * Camera.main.ScreenPointToRay(Input.mousePosition).direction), Color.green, 10f);
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
			{
				Debug.Log(hitInfo.collider.gameObject.name);
				if (hitInfo.collider == thisCollider && Vector3.Distance(player.transform.position, transform.position) < 4f)
				{
					Debug.Log("LEVER PULLED");
					if (!rotating)
						StartCoroutine(SwitchLever());
				}
			}
		}
	}

	IEnumerator SwitchLever()
	{
		// Rotate the object.
		rotating = true;
		leverState = !leverState;
		for (int ii = 0; ii < 10; ii++)
		{
			if (leverState)
				leverObject.transform.Rotate(-9f, 0f, 0f);
			else
				leverObject.transform.Rotate(9f, 0f, 0f);

			yield return null;
		}
		// Deal with the doors.
		for (int ii = 0; ii < 10; ii++)
		{
			foreach (GameObject gg in doors)
			{
				if (leverState)
					gg.transform.Translate(0f, 0.55f, 0f);
				else
					gg.transform.Translate(0f, -0.55f, 0f);
			}
			yield return null;
		}

		rotating = false;
	}
}
