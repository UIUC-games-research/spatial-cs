using UnityEngine;
using System.Collections;

public class KeyToggle : MonoBehaviour
{
	// This script allows you to setup a gameobject to disable or enable while pressing a key.
	public bool toggle = false;
	public bool startDisabled = true;
	public KeyCode keyToPress;
	public GameObject toDisable;
	

	// Use this for initialization
	void Start()
	{
		toDisable.SetActive(!startDisabled);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(keyToPress))
		{
			if (toDisable.activeSelf)
				toDisable.SetActive(false);
			else
				toDisable.SetActive(true);
		}
		if (Input.GetKeyUp(keyToPress))
		{
			if (!toggle)
			{
				toDisable.SetActive(!startDisabled);
			}
		}
	}
}
