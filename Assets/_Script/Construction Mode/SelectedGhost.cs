using UnityEngine;
using System.Collections;

public class SelectedGhost : MonoBehaviour {

	float timer = 0f;
	float destroyAt = 1f;
	public RaycastHit hitInfo;		// Set on creation by SelectedEffect.

	void Start ()
	{
		// Clean up the ghost.
		SelectBehavior sel = GetComponent<SelectBehavior>();
		FuseBehavior fus = GetComponent<FuseBehavior>();
		SelectedEffect eff = GetComponent<SelectedEffect>();

		Destroy(sel);
		Destroy(fus);
		Destroy(eff);

		// Move up a little.
		transform.position += (hitInfo.normal);
	}
	
	void Update ()
	{
		/*
		// Move and delete after some time.
		timer += Time.deltaTime;
		if (timer > destroyAt)
		{
			Debug.Log("Destroying");
			Destroy(this.gameObject);
		}
		transform.position += (0.1f * hitInfo.normal);
		*/
	}
}
