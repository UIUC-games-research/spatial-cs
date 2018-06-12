using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectedGhost : MonoBehaviour {

	float timer = 0f;
	float destroyAt = 1f;
	private Vector3 normal;

	void Start ()
	{
	}

    public void setNormal(Vector3 normal)
    {
        this.normal = normal;
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
