using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveNameSetter : MonoBehaviour
{
	Text field;
	void Start ()
	{
		field = GetComponent<Text>();
	}

	void Update ()
	{
		// Just keep the string in SaveController updated.
		SaveController.filename = field.text;
	}
}
