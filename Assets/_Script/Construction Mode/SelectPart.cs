using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class SelectPart : MonoBehaviour {
	// face of active part to fuse
	private GameObject selectedObject;
	private GameObject prevSelectedObject;
	private GameObject activePart;
	private CameraControl cameraControls;

	private Texture unhighTexture;
	private Texture highTexture;

	//face of part to fuse to
	private GameObject selectedFuseTo;
	private GameObject prevSelectedFuseTo;

	private Color prevColorSelectedFuseTo;

	//tutorial variables
	public bool tutorialOn;
	public string mode;

	public Button connectButton;
	//background music: (4), (11), (34)

	// When an active part is replaced with another, currently
	// selected part becomes null?

	void Awake() {

		//selectedObject = GameObject.Find ("Sole_Heel_Top_Attach");
		selectedObject = null;
		activePart = null;

		prevSelectedObject = selectedObject;
		print ("Initially Selected Object: " + selectedObject);

		selectedFuseTo = null;
		//highTexture = selectedFuseTo.GetComponent<SelectBehavior>().highTex;
		//selectedFuseTo.renderer.material.mainTexture = highTexture;

		prevSelectedFuseTo = selectedFuseTo;
		print ("Initially Selected FuseTo: " + selectedFuseTo);


	}
		
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
			{
				GameObject objectToSelect = hitInfo.transform.gameObject;
			//	print ("Currently selected object: " + selectedObject);
			//	print ("Active part: " + activePart);
				Transform objectParent = objectToSelect.transform.parent;

				if(objectParent != null && 
				   objectToSelect.GetComponent<SelectBehavior>() != null && 
				   objectToSelect.transform.parent.gameObject.GetComponent<IsFused>().isFused) {
					//fused part
					globalHitInfo = hitInfo;
					// unhighlight previously selected fused part
					if(prevSelectedFuseTo != null) {
						unhighTexture = prevSelectedFuseTo.GetComponent<SelectBehavior>().unhighTex;
						prevSelectedFuseTo.GetComponent<Renderer>().material.mainTexture = unhighTexture;

						//! CODE FOR REMOVING Marker FROM PREVIOUS PART. prevSelectedFuseTo
						Destroy(prevSelectedFuseTo.GetComponent<SelectedEffect>());
						
					}
					
					selectedFuseTo = objectToSelect;
					print("Currently Selected FuseTo: " + selectedFuseTo);
					highTexture = selectedFuseTo.GetComponent<SelectBehavior>().highTex;
					selectedFuseTo.GetComponent<Renderer>().material.mainTexture = highTexture;

					//! CODE FOR ADDING MARKER TO SELECTED PART. selectedFuseTo
					
					if (GetComponent<SelectedEffect>() == null)
					{
						SelectedEffect sel = selectedFuseTo.AddComponent<SelectedEffect>();
						sel.hitInfo = hitInfo;
						sel.selected = selectedFuseTo;
					}
					

					prevSelectedFuseTo = selectedFuseTo;


				} else if (objectToSelect.transform.parent != null && objectToSelect.GetComponent<SelectBehavior>() != null){
					//active part

					// unhighlight previously selected active part
					if(prevSelectedObject != null) {
						Texture regTex = prevSelectedObject.GetComponent<SelectBehavior>().unhighTex;
						prevSelectedObject.GetComponent<Renderer>().material.mainTexture = regTex;

						//! CODE FOR REMOVING MARKER FROM PREVIOUS PART. prevSelectedObject
						Destroy(prevSelectedObject.GetComponent<SelectedEffect>());
						
					}

					selectedObject = hitInfo.transform.gameObject;
					//highlight selected object
					Texture highlightedTex = selectedObject.GetComponent<SelectBehavior>().highTex;
					if(highlightedTex) {
						selectedObject.GetComponent<Renderer>().material.mainTexture =  highlightedTex;
					} else {
						print ("Unable to load texture");
					}
					print("Currently Selected Object: " + selectedObject);

					//! CODE FOR ADDING MARKER TO SELECTED PART. selectedObject
					
					if (GetComponent<SelectedEffect>() == null)
					{
						SelectedEffect sel = selectedObject.AddComponent<SelectedEffect>();
						sel.hitInfo = hitInfo;
						sel.selected = selectedObject;
					}
					

					prevSelectedObject = selectedObject;
					//print ("prevSelected: " + prevSelectedObject.name);

				}
				if(!tutorialOn && selectedObject != null && selectedFuseTo != null) {
					connectButton.interactable = true;
				} else {
					connectButton.interactable = false;
				}

				//! PART MOVEMENT.
				// We ALWAYS need the hitinfo of selecting a part on the static object.
				// To do this, globalHitInfo is set whenever we click on a fused part.
				if (selectedFuseTo != null && selectedObject != null && objectToSelect.GetComponent<SelectBehavior>() != null)
				{
					SelectedEffect fuseToFX = selectedFuseTo.GetComponent<SelectedEffect>();
					if (fuseToFX != null)
					{
						// Move it to the position of the fused object, offset by a multiple of the normal, 
						// offset again by the scaled local positional difference of the connection face and the parent object.

						//! DUE TO THE CRAZINESS: All parts with non-boxy attachment points will require box colliders roughly positioned at their center.'
						if (selectedObject.GetComponent<BoxCollider>() == null)
						{
							BoxCollider boxy = selectedObject.AddComponent<BoxCollider>();
							boxy.size = Vector3.zero;
						}
						if (selectedFuseTo.GetComponent<BoxCollider>() == null)
						{
							BoxCollider boxy = selectedFuseTo.AddComponent<BoxCollider>();
							boxy.size = Vector3.zero;
						}

						// The actual location of the selected fuse marker... Wow.
						Vector3 properFuseToPos = selectedFuseTo.transform.position + (Quaternion.Euler(selectedFuseTo.transform.eulerAngles) * (selectedFuseTo.transform.parent.localScale.x * (selectedFuseTo.GetComponent<BoxCollider>().center)));
						// The actual offset of the object face from the object parent... Also wow.
						Vector3 properOffset = Quaternion.Euler(selectedObject.transform.parent.localEulerAngles) * (selectedObject.transform.parent.localScale.x * (selectedObject.transform.localPosition + Quaternion.Euler(selectedObject.transform.localEulerAngles) * (selectedObject.GetComponent<BoxCollider>().center)));

						//Debug.DrawLine(selectedObject.transform.parent.position, selectedObject.transform.parent.position + properOffset, Color.red, 25f, false);
						//Debug.DrawLine(selectedFuseTo.transform.parent.position, properFuseToPos, Color.red, 25f, false);

						StartCoroutine(SweepPosition(selectedObject.transform.parent.gameObject, properFuseToPos - properOffset + (15 * globalHitInfo.normal), 20));					
					}
					else
					{
						Debug.LogError("Uh oh, no fuse fx for some reason!");
					}
				}
				//! END PART MOVEMENT
			}

		}
	}

	// This function is called by RotationGizmo.cs to make sure the alignment of the faces persists even when rotating the object.
	// It's also the most ridiculous three lines of code this world has ever seen.
	RaycastHit globalHitInfo;
	public void AdjustPartAlignment(float x, float y, float z)
	{
		if (selectedFuseTo != null && selectedObject != null)
		{
			Vector3 properFuseToPos = selectedFuseTo.transform.position + (Quaternion.Euler(selectedFuseTo.transform.eulerAngles) * (selectedFuseTo.transform.parent.localScale.x * (selectedFuseTo.GetComponent<BoxCollider>().center)));
			Vector3 properOffset = Quaternion.Euler(x, y, z) * Quaternion.Euler(selectedObject.transform.parent.localEulerAngles) * (selectedObject.transform.parent.localScale.x * (selectedObject.transform.localPosition + Quaternion.Euler(selectedObject.transform.localEulerAngles) * (selectedObject.GetComponent<BoxCollider>().center)));
			StartCoroutine(SweepPosition(selectedObject.transform.parent.gameObject, properFuseToPos - properOffset + (10 * globalHitInfo.normal), 30));
		}
	}

	public void setTutorialOn(bool isOn) {
		tutorialOn = isOn;
	}


	public GameObject getSelectedObject() {
		return selectedObject;
	}

	public GameObject getSelectedFuseTo() {
		return selectedFuseTo;
	}

	public GameObject getActivePart() {
		return activePart;
	}

	public void resetSelectedObject() {
		//! CODE FOR REMOVING GHOSTS ON CONNECT.
		Destroy(selectedObject.GetComponent<SelectedEffect>());

		prevSelectedObject = selectedObject;
		selectedObject = null;
		Texture unhighlightedTex = prevSelectedObject.GetComponent<SelectBehavior>().unhighTex;
		prevSelectedObject.GetComponent<Renderer>().material.mainTexture = unhighlightedTex;
	}
	
	public void resetSelectedFuseTo() {
		//! CODE FOR REMOVING GHOSTS ON CONNECT.
		Destroy(selectedFuseTo.GetComponent<SelectedEffect>());

		prevSelectedFuseTo = selectedFuseTo;
		selectedFuseTo = null;
		Texture unhighlightedTex = prevSelectedFuseTo.GetComponent<SelectBehavior>().unhighTex;
		prevSelectedFuseTo.GetComponent<Renderer>().material.mainTexture = unhighlightedTex;
	}

	//destroys active part so it can be replaced with a new part - use when a part button is clicked
	public void destroySelectedObject() {
		Destroy (selectedObject.transform.parent.gameObject);
		prevSelectedObject = null;
		selectedObject = null;
	}



	//selects on command - use when new object is created
	public void selectObject(GameObject newSelection) {
		prevSelectedObject = newSelection;
		selectedObject = newSelection;

		if (selectedObject.GetComponent<SelectBehavior>() == null) {
			//BOO stupid priority problem - this is an ugly workaround
			selectedObject.AddComponent<SelectBehavior>();
			Texture unhighTexture = selectedObject.GetComponent<Renderer>().material.mainTexture;
			selectedObject.GetComponent<SelectBehavior>().unhighTex = unhighTexture;
			Texture highTexture = (Texture)Resources.Load (unhighTexture.name + "_h");
			selectedObject.GetComponent<SelectBehavior>().highTex = highTexture;

		} 
		Texture highlightedTex = selectedObject.GetComponent<SelectBehavior>().highTex;
		selectedObject.GetComponent<Renderer>().material.mainTexture = highlightedTex;


	}

	public void selectFuseTo(GameObject newSelection) {
		prevSelectedFuseTo = newSelection;
		selectedFuseTo = newSelection;

		if (selectedFuseTo.GetComponent<SelectBehavior>() == null) {
			//BOO stupid priority problem - this is an ugly workaround
			selectedFuseTo.AddComponent<SelectBehavior>();
			Texture unhighTexture = selectedFuseTo.GetComponent<Renderer>().material.mainTexture;
			selectedFuseTo.GetComponent<SelectBehavior>().unhighTex = unhighTexture;
			Texture highTexture = (Texture)Resources.Load (unhighTexture.name + "_h");
			selectedFuseTo.GetComponent<SelectBehavior>().highTex = highTexture;

		} 
		Texture highlightedTex = selectedFuseTo.GetComponent<SelectBehavior>().highTex;
		selectedFuseTo.GetComponent<Renderer>().material.mainTexture = highlightedTex;

	}

	public void newPartCreated(string part) {
		if(selectedObject != null) {
			GameObject parent = selectedObject.transform.parent.gameObject;
			
			if(!parent.GetComponent<IsFused>().isFused) {
				destroySelectedObject();
			}
		} 
		activePart = GameObject.Find (part);

	}


	IEnumerator SweepPosition(GameObject toSweep, Vector3 targetPos, int frames)
	{
		// Interpolate.
		Vector3 initialPos = toSweep.transform.position;
		float iteration = 1 / (float)frames;
		for (float i = 0.0f; i < 1; i += iteration)
		{
			toSweep.transform.position = Vector3.Lerp(initialPos, targetPos, i);
			yield return null;
		}

		// Ensure it ends in the right place no matter what.
		yield return null;
		toSweep.transform.position = targetPos;
	}

}
