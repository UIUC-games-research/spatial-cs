using UnityEngine;
using System.Collections;

public class SelectedGhost : MonoBehaviour {

	float timer = 0f;
	float destroyAt = 1f;
	public RaycastHit hitInfo;      // Set on creation by SelectedEffect.
	Vector3 normal;

	void Start ()
	{
		normal = hitInfo.normal;
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
	}
}
