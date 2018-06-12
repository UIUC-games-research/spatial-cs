using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectedEffect : MonoBehaviour
{

	float delay = 0.15f;
	float timer = 0f;
	public RaycastResult hitInfo;      // Set by SelectPart when this script is applied.
    public RaycastHit hitUpdate;
	GameObject instance;
	GameObject hitCaster;

	void Start()
	{

		// Also add a hitcaster object to keep our normal updated.
		hitCaster = new GameObject();
		hitCaster.transform.position = transform.position;
		hitCaster.transform.localScale = transform.parent.localScale; //3f * transform.localScale;
		hitCaster.transform.rotation = transform.rotation;
		hitCaster.transform.parent = transform.parent;
		hitCaster.transform.position += (20f * hitInfo.worldNormal);
	}

	void FixedUpdate()
	{

		timer += Time.deltaTime;
		if (timer > delay)
		{
			timer = 0f;
			SpawnGhost();
		}

		// Update the normal with the hitCaster.
		Physics.Raycast(hitCaster.transform.position, transform.position - hitCaster.transform.position, out hitUpdate);
		//Debug.DrawRay(hitCaster.transform.position, 10 * (transform.position - hitCaster.transform.position), Color.cyan, 3f);

	}

	public void SpawnGhost()
	{
		// Transforms.
        // will adding the name parameter help unity distinguish fuseTo ghosts from AC ghosts? We'll see!
		instance = new GameObject(this.name + "_ghost");

		instance.transform.position = transform.position;
		instance.transform.localScale = /*10 */ transform.parent.localScale.x * transform.localScale;
		instance.transform.rotation = transform.rotation;
		//instance.transform.parent = transform.parent;
		LoadUtils.InstantiateParenter(instance);
		instance.layer = 2;

		// Add mesh filter and renderer
		MeshFilter meshf = instance.AddComponent<MeshFilter>();
		meshf.mesh = GetComponent<MeshFilter>().mesh;
		MeshRenderer meshr = instance.AddComponent<MeshRenderer>();
		meshr.material = Resources.Load("Opacity") as Material;


		// Add ghost script.
		SelectedGhost ghost = instance.AddComponent<SelectedGhost>();
		ghost.setNormal(hitUpdate.normal);
	}

	void OnDestroy()
	{
		Destroy(instance);
	}
}
