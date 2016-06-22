using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Construction : MonoBehaviour {
	public Button RotateButton;
	// Use this for initialization
	void Start () {
		Debug.Log ("I am alive");
	}

	// Update is called once per frame
	void Update () {
		if (RotateButton) {
			StartCoroutine ("RotateRight");
		}
	}

	public void rotateCube() {
		Debug.Log ("Clicked");
		RotateRight();
	}

	IEnumerator RotateRight()
	{
		Quaternion oldRotation = transform.rotation;
		transform.Rotate(0,45,0);
		Quaternion newRotation = transform.rotation;
		
		for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime)
		{
			transform.rotation = Quaternion.Slerp(oldRotation, newRotation, t);
			yield return null;
		}
		
		transform.rotation = newRotation; // To make it come out at exactly 90 degrees
		
	}
}
