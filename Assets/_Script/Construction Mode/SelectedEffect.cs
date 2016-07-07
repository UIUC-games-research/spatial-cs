using UnityEngine;
using System.Collections;

public class SelectedEffect : MonoBehaviour {

	float delay = 0.5f;
	float timer = 0f;
	public RaycastHit hitInfo;      // Set by SelectPart when this script is applied.
	GameObject instance;

	void Start ()
	{
		SpawnGhost();
	}
	
	void Update ()
	{
		/*
		timer += Time.deltaTime;
		if (timer > delay)
		{
			timer = 0f;
			SpawnGhost();
		}
		*/
	}

	public void SpawnGhost()
	{
		Debug.Log("SPAWN");
		instance = Instantiate(gameObject);
		Destroy(instance.GetComponent<SelectedEffect>());
		instance.transform.position = transform.position;
		instance.transform.localScale = 11 * transform.localScale;
		instance.transform.rotation = transform.rotation;
		instance.transform.SetParent(transform.parent);

		SelectedGhost ghost = instance.AddComponent<SelectedGhost>();
		ghost.hitInfo = hitInfo;
	}

	void OnDestroy()
	{
		Destroy(instance);
	}
}
