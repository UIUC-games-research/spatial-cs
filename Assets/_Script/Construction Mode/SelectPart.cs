using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class SelectPart : MonoBehaviour {
	// face of active part to fuse
	private GameObject selectedObject;
	private GameObject prevSelectedObject;
	private GameObject activePart;
	private Dictionary<string, int> cameraAdjusts;
	private GameObject mainCamera;
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
	public Text getCorrectRotation;
	public Text clickBlack;
	public Text noticeHowThe;
	public Text nowWhatSide;
	public Text youNeedToFind;
	public Text youCanRotate;
	public Text rotateToFind;
	public Text clickOnMatching;
	public Text theTwoBlackAreas;
	public Text inOrderToAttach;
	public Text imagineYouAre;
	public Text whatDirectionMust;
	public Text lineUpBlackAreas;

	public GameObject rotateForwardButton;
	public GameObject connectButton;
	//background music: (4), (11), (34)
	public CanvasGroup clickBlackPanel;
	public CanvasGroup findOtherBlackRegionPanel;
	public CanvasGroup rotateToFindBlackPanel;

	private bool findBlackRegionDone;
	// When an active part is replaced with another, currently
	// selected part becomes null?

	void Awake() {
		cameraAdjusts = new Dictionary<string, int>();
		mainCamera = GameObject.Find ("Main Camera");
		//cameraControls = mainCamera.GetComponent<CameraControl>();
		findBlackRegionDone = false;
		if(mode.Equals ("intro")) {
			tutorialOn = true;
		} else if(mode.Equals ("boot")){
			tutorialOn = false;

		} else if(mode.Equals ("ebg")){
			tutorialOn = false;

		} else if (mode.Equals ("key1")) {
			tutorialOn = false;
		}
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

	public bool tutorialMidAndBottomSelected() {
		if(selectedObject != null && selectedFuseTo != null) {
			return selectedObject.Equals (GameObject.Find ("mid_bottom_attach")) 
				&& selectedFuseTo.Equals (GameObject.Find ("bottom_attach"));
		}
		return false;

	}
	


	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo))
			{
				GameObject objectToSelect = hitInfo.transform.gameObject;
				print ("Currently selected object: " + selectedObject);
				print ("Active part: " + activePart);
				print ("objectToSelect: " + objectToSelect + ", tutorial: " + tutorialOn);
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

					/*
					int numClicksHigh = cameraControls.getNumClicksHigh();
					if(cameraAdjusts.ContainsKey(selectedFuseTo.name)) {

						while(numClicksHigh != cameraAdjusts[selectedFuseTo.name]) {
							if(numClicksHigh < cameraAdjusts[selectedFuseTo.name]) {
								cameraControls.rotateUp ();
							} else {
								cameraControls.rotateDown ();
							}
							numClicksHigh = cameraControls.getNumClicksHigh();
						}
					}
					*/
					//tutorial
					if(tutorialOn && tutorialMidAndBottomSelected()) {
						StartCoroutine(findOtherBlackRegion());
						tutorialOn = false;
					} else if(tutorialOn && selectedFuseTo.name.Equals ("bottom_attach") && !findBlackRegionDone) {
						findBlackRegionDone = true;
						StartCoroutine (findBlackRegion());
					}
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

					//tutorial
					if(tutorialOn && tutorialMidAndBottomSelected()) {
						StartCoroutine(findOtherBlackRegion());
						tutorialOn = false;
					}
				}
				if(!tutorialMidAndBottomSelected() && selectedObject != null && selectedFuseTo != null) {
					connectButton.transform.GetComponent<Button>().interactable = true;
				} else if (tutorialMidAndBottomSelected () && selectedObject != null && selectedFuseTo != null) {
					// do nothing
				} else {
					connectButton.transform.GetComponent<Button>().interactable = false;
				}


			}
		}
	}

	IEnumerator findBlackRegion() {
		clickBlack.enabled = false;
		noticeHowThe.enabled = true;
		yield return new WaitForSeconds(4);
		noticeHowThe.enabled = false;

		nowWhatSide.enabled = true;
		yield return new WaitForSeconds(4);
		nowWhatSide.enabled = false;

		youNeedToFind.enabled = true;
		yield return new WaitForSeconds(4);
		youNeedToFind.enabled = false;
		clickBlackPanel.alpha = 0;

		findOtherBlackRegionPanel.alpha = 1;
		youCanRotate.enabled = true;
		yield return new WaitForSeconds(4);
		youCanRotate.enabled = false;

		rotateToFind.enabled = true;
		rotateForwardButton.GetComponent<Button>().interactable = true;
		rotateForwardButton.transform.GetComponent<RotateButton>().setObjectToRotate(GameObject.Find ("introMidPrefab(Clone)"));
	}

	IEnumerator findOtherBlackRegion() {
		clickOnMatching.enabled = false;
		
		theTwoBlackAreas.enabled = true;
		yield return new WaitForSeconds(4);
		theTwoBlackAreas.enabled = false;
		
		inOrderToAttach.enabled = true;
		yield return new WaitForSeconds(4);
		inOrderToAttach.enabled = false;
		
		imagineYouAre.enabled = true;
		yield return new WaitForSeconds(4);
		imagineYouAre.enabled = false;
		
		whatDirectionMust.enabled = true;
		yield return new WaitForSeconds(4);
		whatDirectionMust.enabled = false;
		
		lineUpBlackAreas.enabled = true;
		rotateForwardButton.GetComponent<Button>().interactable = true;
		rotateForwardButton.transform.GetComponent<RotateButton>().setObjectToRotate(GameObject.Find ("introMidPrefab(Clone)"));

	}

	public GameObject getSelectedObject() {
		return selectedObject;
	}

	public GameObject getSelectedFuseTo() {
		return selectedFuseTo;
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

		RaycastHit hitInfo = new RaycastHit();
		float xCoord = newSelection.transform.position.x;
		float yCoord = newSelection.transform.position.y;
		float zCoord = newSelection.transform.position.z;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(xCoord, yCoord, zCoord)), out hitInfo)) {
			if (selectedObject.GetComponent<SelectedEffect>() == null)
			{
				SelectedEffect sel = selectedObject.AddComponent<SelectedEffect>();
				sel.hitInfo = hitInfo;
				sel.selected = selectedObject;
			}
		}

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

		RaycastHit hitInfo = new RaycastHit();
		float xCoord = newSelection.transform.position.x;
		float yCoord = newSelection.transform.position.y;
		float zCoord = newSelection.transform.position.z;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(xCoord, yCoord, zCoord)), out hitInfo)) {
			if (selectedFuseTo.GetComponent<SelectedEffect>() == null)
			{
				SelectedEffect sel = selectedObject.AddComponent<SelectedEffect>();
				sel.hitInfo = hitInfo;
				sel.selected = selectedFuseTo;
			}
		}

	}

	public void newPartCreated(string part) {
		if(selectedObject != null) {
			GameObject parent = selectedObject.transform.parent.gameObject;
			
			if(!parent.GetComponent<IsFused>().isFused) {
				destroySelectedObject();
			}
		} 
		activePart = GameObject.Find (part);

		//print ("Destroying old direction images");
		//Destroy (GameObject.FindGameObjectWithTag("X"));
		//Destroy (GameObject.FindGameObjectWithTag("Y"));
		//Destroy (GameObject.FindGameObjectWithTag("Z"));
	}

}
