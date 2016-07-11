using UnityEngine;
using System.Collections;

public class SelectedGhost : MonoBehaviour {

	float timer = 0f;
	float destroyAt = 1f;
	public RaycastHit hitInfo;      // Set on creation by SelectedEffect.
	public GameObject selected;		// As above.
	Mesh mesh;
	float curDistance;
	float nexDistance;

	void Start ()
	{
		mesh = hitInfo.transform.GetComponent<MeshFilter>().mesh;
		curDistance = Vector3.Distance(transform.position, transform.parent.position);
		nexDistance = Vector3.Distance(transform.position + (0.01f * hitInfo.normal), transform.parent.position);
		//Debug.DrawLine(transform.position, transform.position + (5000f * hitInfo.normal), Color.green, 3f);



		// Make a back face.
		/*
		GameObject instance = new GameObject();
		instance.transform.parent = transform;
		instance.transform.localPosition = Vector3.zero;
		instance.transform.localScale = Vector3.one;
		instance.layer = 2;

		// Flip
		Vector3 rotation = instance.transform.localEulerAngles;
		rotation.x += 180;
		instance.transform.localEulerAngles = rotation;

		// Add mesh.
		MeshFilter meshf = instance.AddComponent<MeshFilter>();
		meshf.mesh = GetComponent<MeshFilter>().mesh;
		MeshRenderer meshr = instance.AddComponent<MeshRenderer>();
		meshr.material = Resources.Load("Opacity") as Material;
		*/
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

		transform.position += (0.08f * hitInfo.normal);


		/*
		// This sometimes goes the wrong way.
		// I'm assuming selections always face outwards from the center of the object.
		// Check to see if the normal is going toward the center. If it is, switch it.

		// Get distances.

		if (curDistance < nexDistance)
		{
			Debug.Log("Moving against normal");
			transform.position += (0.05f * hitInfo.normal);
		}
		else
		{
			Debug.Log("Moving with normal");
			transform.position += (0.05f * hitInfo.normal);
		}
		*/
	}
}
