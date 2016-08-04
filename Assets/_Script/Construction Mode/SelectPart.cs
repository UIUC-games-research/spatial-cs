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


			}
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

}
