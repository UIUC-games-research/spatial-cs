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
		curDistance = Vector3.Distance(transform.localPosition, selected.transform.localPosition);
		nexDistance = Vector3.Distance(transform.localPosition + (0.1f * hitInfo.normal), selected.transform.localPosition);
	}
	
	void Update ()
	{
		
		// Move and delete after some time.
		timer += Time.deltaTime;
		if (timer > destroyAt)
		{
			//Debug.Log("Destroying");
			Destroy(this.gameObject);
		}

		// This sometimes goes the wrong way.
		// I'm assuming selections always face outwards from the center of the object.
		// Check to see if the normal is going toward the center. If it is, switch it.

		// Get distances.
		
		if (curDistance > nexDistance)
		{
			Debug.Log("Moving against normal");
			transform.localPosition += (-0.01f * hitInfo.normal);
		}
		else
		{
			Debug.Log("Moving with normal");
			transform.localPosition += (0.01f * hitInfo.normal);
		}
		
	}
}
