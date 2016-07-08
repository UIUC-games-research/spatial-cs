using UnityEngine;
using System.Collections;

public class SelectedEffect : MonoBehaviour {

	float delay = 0.15f;
	float timer = 0f;
	public RaycastHit hitInfo;      // Set by SelectPart when this script is applied.
	public GameObject selected;
	GameObject instance;

	void Start ()
	{
		//SpawnGhost();
	}
	
	void Update ()
	{
		
		timer += Time.deltaTime;
		if (timer > delay)
		{
			timer = 0f;
			SpawnGhost();
		}
		
	}

	public void SpawnGhost()
	{
		//Debug.Log("SPAWN");
		/*
		instance = Instantiate(gameObject);
		Destroy(instance.GetComponent<SelectedEffect>());
		instance.transform.position = transform.position;
		instance.transform.localScale = 10 * transform.localScale;
		instance.transform.rotation = transform.rotation;
		instance.transform.SetParent(transform.parent);
		*/

		// Transforms.
		instance = new GameObject();
		//instance = Instantiate(Resources.Load("MarkerSphere") as GameObject);
		
		instance.transform.position = transform.position;
		instance.transform.localScale = 10 * transform.localScale;
		instance.transform.rotation = transform.rotation;
		instance.transform.parent = transform.parent;
		instance.layer = 2;

		// Add mesh filter and renderer
		
		MeshFilter meshf = instance.AddComponent<MeshFilter>();
		meshf.mesh = GetComponent<MeshFilter>().mesh;
		MeshRenderer meshr = instance.AddComponent<MeshRenderer>();
		meshr.material = Resources.Load("Opacity") as Material;
		

		// Add ghost script.
		SelectedGhost ghost = instance.AddComponent<SelectedGhost>();
		ghost.hitInfo = hitInfo;
		ghost.selected = selected;
	}

	void OnDestroy()
	{
		Destroy(instance);
	}
}
