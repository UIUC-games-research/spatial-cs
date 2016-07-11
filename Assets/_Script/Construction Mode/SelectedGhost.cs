using UnityEngine;
using System.Collections;

public class SelectedGhost : MonoBehaviour {

	float timer = 0f;
	float destroyAt = 1f;
	public RaycastHit hitInfo;      // Set on creation by SelectedEffect.
	Vector3 normal;
	public GameObject selected;		// As above.
	Mesh mesh;
	//Vector3 currRot = Vector3.zero;
	//Vector3 startRot = Vector3.zero;
	Quaternion currRot = Quaternion.identity;
	Quaternion startRot = Quaternion.identity;
	Vector3 initialFaceNormal;

	void Start ()
	{
		mesh = hitInfo.transform.GetComponent<MeshFilter>().mesh;
		normal = hitInfo.normal;
		currRot = transform.parent.rotation;
		startRot = transform.parent.rotation;
		initialFaceNormal = mesh.normals[0];

		FindHitFace();
	}
	
	void FixedUpdate ()
	{
		
		// Move and delete after some time.
		timer += Time.deltaTime;
		if (timer > destroyAt)
		{
			//Debug.Log("Destroying");
			Destroy(this.gameObject);
		}

		transform.position += (0.08f * normal);

		// Update normal direction
		currRot = transform.parent.localRotation;
		//Quaternion temp = currRot * Quaternion.Inverse(startRot);

		// Rotate.
		//normal = Quaternion.Euler(currRot.eulerAngles) * hitInfo.normal;
		//normal = Quaternion.Euler(-1 * startRot.eulerAngles) * normal;
		//normal = Quaternion.Euler(transform.parent.localEulerAngles) * initialFaceNormal;
		//normal = Quaternion.Euler(transform.localEulerAngles) * normal;
		//normal = initialFaceNormals
		Debug.DrawLine(transform.position, transform.position + (5f * normal), Color.green, 3f);
	}

	void FindHitFace()
	{
		//Vector3[] normals = mesh.normals;
		//Debug.Log("FACE: " + normals[0]);
		//Debug.Log("HIT: " + hitInfo.normal);
	}
}
