using UnityEngine;
using System.Collections;

public class RotateHandler : MonoBehaviour {

	private GameObject rotated;

	// Use this for initialization
	void Start () {
		rotated = GetComponent<SelectPart>().getSelectedObject();
	}

	public IEnumerator rotateHelperX() {
//		bool rotating = false;
//		int rotateSpeed = 45;
//		Quaternion tmp;
//		if (!rotating) {
//			rotating = true;
//			float curRot =  0.0f;
//			float startRot = rotated.transform.localRotation.x;
//			print ("starting rotation: " + startRot);

//			while (curRot < 90.0f) {
//				curRot += rotateSpeed * Time.deltaTime;
//				tmp = rotated.transform.localRotation;
//				tmp.x = startRot + curRot;
//				rotated.transform.localRotation = tmp;
//				yield return new WaitForSeconds(0.001f);
//			}
			//tmp = rotated.transform.localRotation;
			//tmp.x = Mathf.Round(startRot + 90f);
			//print ("ending rotation: " + tmp.x);

			//rotated.transform.localRotation = tmp;
			//print ("rotated.transform.localRotation: " + rotated.transform.rotation.x);
//			rotating = false;
//		}
		Quaternion oldRotation = rotated.transform.rotation;
		rotated.transform.Rotate(90,0,0);
		Quaternion newRotation = rotated.transform.rotation;
		
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime)
		{
			rotated.transform.rotation = Quaternion.Slerp(oldRotation, newRotation, t);
			yield return new WaitForSeconds(0.01f);
		}
		for(int i = 0; i < 90; i++) {
			rotated.transform.Rotate(Time.deltaTime * 90.0f,0,0);
			print (Time.deltaTime * 90.0f);
			yield return new WaitForSeconds(0.01f);
		}
	}

	public IEnumerator rotateHelperY() {
		for(int i = 0; i < 100; i++) {
			rotated.transform.Rotate (new Vector3(0, 45, 0) * Time.deltaTime * 2);
			yield return new WaitForSeconds(0.01f);
		}
	}
	
	public IEnumerator rotateHelperZ() {
		for(int i = 0; i < 75; i++) {
			rotated.transform.Rotate (new Vector3(0, 0, 45) * Time.deltaTime * 2);
			yield return new WaitForSeconds(0.01f);
		}
	}
	
	public void rotateX() {
		rotated = GetComponent<SelectPart>().getSelectedObject().transform.parent.gameObject;

		rotated.transform.Rotate(90,0,0);

	}
	
	public void rotateY() {
		rotated = GetComponent<SelectPart>().getSelectedObject().transform.parent.gameObject;

		rotated.transform.Rotate(0,90,0);

	}
	
	public void rotateZ() {
		rotated = GetComponent<SelectPart>().getSelectedObject().transform.parent.gameObject;

		rotated.transform.Rotate(0,0,90);

	}
	
	// Update is called once per frame
	void Update () {
	}
}
