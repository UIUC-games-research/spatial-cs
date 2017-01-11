using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RotationGizmo : MonoBehaviour
{
	public GameObject mainCamera;
	public GameObject toRotate;
	public bool tutorialOn;
	public SelectPart adjuster;
	public GameObject batteryIndicator;

	public GameObject xGizmo;
	public GameObject yGizmo;
	public GameObject zGizmo;
	public int xRots = 0;
	public int yRots = 0;
	public int zRots = 0;

	bool rotating = false;

	void Start ()
	{
		//adjuster = EventSystem.current.gameObject.GetComponent<SelectPart>();
		//adjuster = GameObject.Find("EventSystem").GetComponent<SelectPart>();
		Disable();
		batteryIndicator = GameObject.Find("BatteryIndicator");
	}

	void OnEnable()
	{
		// Reset rotation count.
		xRots = 0;
		yRots = 0;
		zRots = 0;
	}
	
	void Update ()
	{
		// Restarting game while in construction mode.
		// DEMO MODE ONLY.
		if (Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.R))
		{
			InventoryController.RestartGame();
		}


		// LookAts.
		Vector3 lookToward = mainCamera.transform.position;

		// X
		xGizmo.transform.LookAt(new Vector3(xGizmo.transform.position.x, lookToward.y, lookToward.z));
		Vector3 xTemp = xGizmo.transform.localEulerAngles;
		xTemp.z = 90;
		xGizmo.transform.localEulerAngles = xTemp;

		// Y
		yGizmo.transform.LookAt(new Vector3(lookToward.x, xGizmo.transform.position.y, lookToward.z));

		// Z
		zGizmo.transform.LookAt(new Vector3(lookToward.x, lookToward.y, xGizmo.transform.position.z));
		Vector3 zTemp = zGizmo.transform.localEulerAngles;
		zTemp.z = 90;
		zGizmo.transform.localEulerAngles = zTemp;

		// Object Itself
		if (toRotate != null)
			transform.position = toRotate.transform.position;


		// Highlight raycasts.
		RaycastHit mouseOver = new RaycastHit();
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseOver))
		{
			switch (mouseOver.transform.name)
			{
				case "XUp":
					Highlighter.Highlight(xGizmo);
					Highlighter.Unhighlight(yGizmo);
					Highlighter.Unhighlight(zGizmo);
					break;

				case "XDown":
					Highlighter.Highlight(xGizmo);
					Highlighter.Unhighlight(yGizmo);
					Highlighter.Unhighlight(zGizmo);
					break;

				case "YLeft":
					Highlighter.Highlight(yGizmo);
					Highlighter.Unhighlight(xGizmo);
					Highlighter.Unhighlight(zGizmo);
					break;

				case "YRight":
					Highlighter.Highlight(yGizmo);
					Highlighter.Unhighlight(xGizmo);
					Highlighter.Unhighlight(zGizmo);
					break;

				case "ZUp":
					Highlighter.Highlight(zGizmo);
					Highlighter.Unhighlight(yGizmo);
					Highlighter.Unhighlight(xGizmo);
					break;

				case "ZDown":
					Highlighter.Highlight(zGizmo);
					Highlighter.Unhighlight(yGizmo);
					Highlighter.Unhighlight(xGizmo);
					break;

				default:
					Highlighter.Unhighlight(xGizmo);
					Highlighter.Unhighlight(yGizmo);
					Highlighter.Unhighlight(zGizmo);
					break;
			}
		}

		// Raycasts.
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
			{
				//Debug.Log(hitInfo.transform.name);
				switch(hitInfo.transform.name)
				{
					case "XUp":
						if (!CheckBattery())
							break;
						xRots++;
						if (Mathf.Approximately(xGizmo.transform.localEulerAngles.y, 180f))
							StartCoroutine(Rotate(90f, 0f, 0f));
						else
							StartCoroutine(Rotate(-90f, 0f, 0f));
						break;

					case "XDown":
						if (!CheckBattery())
							break;
						xRots++;
						if (Mathf.Approximately(xGizmo.transform.localEulerAngles.y, 180f))
							StartCoroutine(Rotate(-90f, 0f, 0f));
						else
							StartCoroutine(Rotate(90f, 0f, 0f));
						break;

					case "YLeft":
						if (!CheckBattery())
							break;
						yRots++;
						StartCoroutine(Rotate(0f, 90f, 0f));
						break;

					case "YRight":
						if (!CheckBattery())
							break;
						yRots++;
						StartCoroutine(Rotate(0f, -90f, 0f));
						break;

					case "ZUp":
						if (!CheckBattery())
							break;
						zRots++;
						if (Mathf.Approximately(zGizmo.transform.localEulerAngles.y, 270f))
							StartCoroutine(Rotate(0f, 0f, -90f));
						else
							StartCoroutine(Rotate(0f, 0f, 90f));
						break;

					case "ZDown":
						if (!CheckBattery())
							break;
						zRots++;
						if (Mathf.Approximately(zGizmo.transform.localEulerAngles.y, 270f))
							StartCoroutine(Rotate(0f, 0f, 90f));
						else
							StartCoroutine(Rotate(0f, 0f, -90f));
						break;

					default:
						break;
				}
			}
		}

	}

	IEnumerator Rotate(float x, float y, float z)
	{
		// Adjustment of alignment.
		adjuster.AdjustPartAlignment(x, y, z);

		// Integration for battery power.
		if(!tutorialOn && !rotating) {
			BatterySystem.SubPower(1);
		}

		// Can only start rotating if not already rotating. Prevents bugs with part movement.
		if (!rotating)
		{
			// Set rotate flag and start rotating.
			// Flag is reset every frame to ensure the check only runs at the end of all queued rotations.
			rotating = true;
			for (int i = 0; i < 30; i++)
			{
				toRotate.transform.Rotate(x / 30f, y / 30f, z / 30f, Space.World);
				rotating = true;
				yield return null;
			}
			rotating = false;
			yield return null;  // Wait a frame to see if another active rotation resets this flag to true.
			if (!rotating)
				StartCoroutine(CheckRotation());
		}
	}

	IEnumerator CheckRotation()
	{
		yield return null;
		
		Vector3 rot = toRotate.transform.eulerAngles;

		// X Rounding
		if (rot.x != 0 && rot.x % 90 < 45)
		{
			rot.x -= (rot.x % 90);
		}
		else if (rot.x != 0 && rot.x % 90 >= 45)
		{
			rot.x += (90 - (rot.x % 90));
		}

		// Y Rounding
		if (rot.y != 0 && rot.y % 90 < 45)
		{
			rot.y -= (rot.y % 90);
		}
		else if (rot.y != 0 && rot.y % 90 >= 45)
		{
			rot.y += (90 - (rot.y % 90));
		}

		// Z Rounding
		if (rot.z != 0 && rot.z % 90 < 45)
		{
			rot.z -= (rot.z % 90);
		}
		else if (rot.z != 0 && rot.z % 90 >= 45)
		{
			rot.z += (90 - (rot.z % 90));
		}

		rot.x = Mathf.RoundToInt(rot.x);
		rot.y = Mathf.RoundToInt(rot.y);
		rot.z = Mathf.RoundToInt(rot.z);

		if (rot.x == 360)
			rot.x = 0;
		if (rot.y == 360)
			rot.y = 0;
		if (rot.z == 360)
			rot.z = 0;

		//Debug.Log(rot);

		toRotate.transform.eulerAngles = rot;
		rotating = false;
	}

	//Warning - will probably break on X and Z rotations if used outside of tutorial
	public void runManualRotation(GameObject objectToRotate, float x, float y, float z) {
		toRotate = objectToRotate;
		StartCoroutine(Rotate(x, y, z));
	}

	public void Disable()
	{
		toRotate = null;
		transform.position = new Vector3(-1000f, -1000f, -1000f);
	}

	public GameObject Enable(GameObject objectToRotate)
	{
		//Debug.Log("Enabled the rotation gizmo.");
		toRotate = objectToRotate;
		transform.position = toRotate.transform.position;
		return objectToRotate;
	}

	// Checks battery, returns whether this rotation can happen. If it cannot, shows error.
	bool CheckBattery()
	{
		// Check if we're in the standard game mode and have no power.
		if (BatterySystem.GetPower() == 0 && !tutorialOn && !FuseEvent.runningJustConstructionMode)
		{
			StartCoroutine(FlashBattery());
			return false;
		}
		else
		{
			return true;
		}
	}

	bool flashing = false;
	IEnumerator FlashBattery()
	{
		if (flashing)
		{
			yield break;
		}
		flashing = true;

		batteryIndicator.transform.localScale = 2f * batteryIndicator.transform.localScale;

		for (int ii = 0; ii < 8; ii++)
		{
			batteryIndicator.transform.Rotate(0f, 90f, 0f);

			// Wait a few frames.
			for (int jj = 0; jj < 8; jj++)
			{
				yield return null;
			}
		}

		batteryIndicator.transform.localScale = 0.5f * batteryIndicator.transform.localScale;
		flashing = false;
	}




}
