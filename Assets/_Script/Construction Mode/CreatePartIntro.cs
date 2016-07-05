using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartIntro : MonoBehaviour {
	
	private GameObject[] instantiated;
	public GameObject[] parts;
	private bool[] partCreated;
	private Vector3 createLoc;
	public GameObject eventSystem;
	private CameraControl cameraControls;
	private SelectPart selectionManager;
	public int NUM_PARTS;
	private GameObject startObject;
	private string password = "e7O9aT";

	public GameObject rotateLeftButton;
	public GameObject rotateForwardButton;
	public GameObject rotateAcrossButton;

	public GameObject topButton;
	public GameObject midButton;

//	public GameObject rotateUpButton;

	//interface explanation variables
	private bool tutorialOn;

	public Text allRight;
	public Text butFirst;
	public Text letsStart;
	public Text buildThis;

	public Text adjustCameraAngles;
	public Text clickDown;
	public Text clickUp;
	public Text createPartsHere;
	public Text clickMid;

	public Text nowYoure;
	public Text areasWhere;
	public Text forExample;
	public Text clickBlack;

	public Text nowYou;
	public CanvasGroup rotatePanelGroup;
	public CanvasGroup bottomPanelGroup;
	public CanvasGroup buildThisPanel;
	public CanvasGroup adjustCameraAnglesPanel;
	public CanvasGroup selectPartsPanel;
	public CanvasGroup clickMidPanel;
	public CanvasGroup clickBlackPanel;
	public CanvasGroup findOtherBlackRegionPanel;
	public CanvasGroup useConnectButtonPanel;

	public Camera mainCamera;
	public GameObject completeIntro;
	public Image finishedImage;

	// Use this for initialization
	void Awake () {
		tutorialOn = true;
		//number of parts to fuse
		partCreated = new bool[NUM_PARTS];
		instantiated = new GameObject[NUM_PARTS];
		cameraControls = GameObject.Find ("Main Camera").GetComponent<CameraControl>();
		for(int i = 0; i < NUM_PARTS; i++) {
			partCreated[i] = false;
		}
		for(int i = 0; i < NUM_PARTS; i++) {
			instantiated[i] = null;
		}
		createLoc = new Vector3(-60, 25, 100);
		selectionManager = eventSystem.GetComponent<SelectPart>();
		startObject = GameObject.Find ("intro_bottom");
		//to avoid errors when selectedObject starts as rocket_boots_start
		startObject.GetComponent<IsFused>().isFused = true;

		//get better starting angle for intro tutorial
		mainCamera.GetComponent<CameraControl>().rotateUp();
		StartCoroutine(tutorialCreateTop ());
	}

	IEnumerator tutorialCreateTop() {
		//tutorial
		//Build This!
		//select top button
		//show rotation to find black region
		//show clicking on black regions
		//show rotation to align parts
		//show rotating camera
		//use connect button!
		print ("tutorial starting!");
		yield return new WaitForSeconds(1);
		allRight.enabled = true;
		yield return new WaitForSeconds(3);
		allRight.enabled = false;
		butFirst.enabled = true;
		yield return new WaitForSeconds(3);
		butFirst.enabled = false;
		letsStart.enabled = true;
		yield return new WaitForSeconds(3);
		letsStart.enabled = false;
		buildThis.enabled = true;
		buildThisPanel.alpha = 1;
		yield return new WaitForSeconds(3);
		buildThis.enabled = false;
		buildThisPanel.alpha = 0;
		adjustCameraAngles.enabled = true;
		adjustCameraAnglesPanel.alpha = 1;
		yield return new WaitForSeconds(4);
		adjustCameraAngles.enabled = false;
		adjustCameraAnglesPanel.alpha = 0;
		createPartsHere.enabled = true;
		selectPartsPanel.alpha = 1;
		yield return new WaitForSeconds(4);
		createPartsHere.enabled = false;
		selectPartsPanel.alpha = 0;
		resetCamera();
		clickMid.enabled = true;
		clickMidPanel.alpha = 1;
		GameObject.Find ("bottom_attach").GetComponent<BoxCollider>().enabled = true;
		midButton.GetComponent<Button>().interactable = true;
	}

	private void resetCamera() {
		int numClicksHigh = cameraControls.getNumClicksHigh();
		while(numClicksHigh < 1) {
			cameraControls.rotateUp ();
			numClicksHigh = cameraControls.getNumClicksHigh();
		}
	}


	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at boot from the front)

	//returns list of objects body can fuse to
	public FuseAttributes midFuses() {
		Vector3 startObjPos = startObject.transform.position;
		Vector3 fuseLocation = new Vector3 (startObjPos.x, startObjPos.y + 20, startObjPos.z);
		//Vector3 fuseLocation = new Vector3 (0, 0, 0);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Quaternion acceptableRotation1 = Quaternion.Euler (new Vector3(270,0,0));
		Quaternion acceptableRotation2 = Quaternion.Euler (new Vector3(270,180,0));

		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		fuseLocations.Add("bottom_attach", fuseLocation);
		fuseRotations.Add("bottom_attach", fuseRotation);
		fusePositions.Add ("bottom_attach", acceptableRotations);
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}


	public FuseAttributes topFuses() {
		GameObject mid = GameObject.Find ("introMidPrefab(Clone)");
		Vector3 startObjPos = startObject.transform.position;
		Vector3 fuseLocation = new Vector3 (startObjPos.x, startObjPos.y + 39.5f, startObjPos.z);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
	
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		if(mid != null) {

			Quaternion acceptableRotation1 = Quaternion.Euler (new Vector3(270,0,0));
			Quaternion acceptableRotation2 = Quaternion.Euler (new Vector3(270,180,0));
			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
			fuseLocations.Add("mid_attach", fuseLocation);
			fuseRotations.Add("mid_attach", fuseRotation);
			fusePositions.Add ("mid_attach", acceptableRotations);
			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		}
			
		return newAttributes;
		
	}



	//when a new part is created, clear partsCreated
	public void clearPartsCreated() {
		for(int i = 0; i < partCreated.Length; i++) {
			partCreated[i] = false;
		}
		for(int i = 0; i < instantiated.Length; i++) {
			if(instantiated[i] != null && !instantiated[i].GetComponent<IsFused>().isFused) {
				Destroy(instantiated[i]);
			}
		}
	}

	public void createDirections(GameObject part) {
		Quaternion defaultRotation = new Quaternion();

		//need this in to set priority
		print ("Initialization complete!");
	}

	public void enableManipulationButtons(GameObject toRotate) {
		rotateLeftButton.transform.GetComponent<Button>().interactable = true;
		rotateForwardButton.transform.GetComponent<Button>().interactable = true;
		rotateAcrossButton.transform.GetComponent<Button>().interactable = true;
		
		rotateLeftButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		rotateForwardButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		rotateAcrossButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);

	}


	public void createMid() {
		createPartsHere.enabled = false;
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = new Quaternion();
			GameObject newMid = LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation));

			//fixes off center rotation problem
			Transform midChild = newMid.transform.FindChild("mid");
			Transform  midBottomAttachChild = newMid.transform.FindChild("mid_bottom_attach");
			Transform  midAttachChild = newMid.transform.FindChild("mid_attach");

			midChild.transform.localPosition = new Vector3(0,0,0);
			midBottomAttachChild.transform.localPosition = new Vector3(0,0.2f,0);
			midAttachChild.transform.localPosition = new Vector3(0,2.05f,0);

			FuseAttributes fuseAtts = midFuses ();

			midBottomAttachChild.gameObject.AddComponent<FuseBehavior>();
			midBottomAttachChild.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			midBottomAttachChild.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Mid"));

			midAttachChild.gameObject.AddComponent<FuseBehavior>();
			midAttachChild.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			midAttachChild.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Mid"));

			instantiated[0] = newMid;
			partCreated[0] = true;
			selectionManager.newPartCreated("introMidPrefab(Clone)");
			createDirections(parts[0]);

			if(!tutorialOn) {
				enableManipulationButtons(newMid);
			} else {
				StartCoroutine(clickBlackTutorial());

				rotateLeftButton.GetComponent<Button>().interactable = false;
				rotateAcrossButton.GetComponent<Button>().interactable = false;
				rotateForwardButton.GetComponent<Button>().interactable = false;
				rotateForwardButton.transform.GetComponent<RotateButton>().setObjectToRotate(newMid);
				tutorialOn = false;
			}


		}
	}

	IEnumerator clickBlackTutorial() {
		clickMid.enabled = false;
		clickMidPanel.alpha = 0;

		clickBlackPanel.alpha = 1;
		nowYoure.enabled  = true;
		yield return new WaitForSeconds(4);
		nowYoure.enabled = false;
		areasWhere.enabled = true;
		yield return new WaitForSeconds(4);
		areasWhere.enabled = false;
		forExample.enabled = true;
		yield return new WaitForSeconds(4);
		forExample.enabled = false;
		clickBlack.enabled = true;
	}
	
	public void createTop() {
		nowYou.enabled = false;
		findOtherBlackRegionPanel.alpha = 0;
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newTop = LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation));

			//fixes off center rotation problem
			Transform topChild = newTop.transform.FindChild("top");
			Transform  topAttachChild = newTop.transform.FindChild("mid_top_attach");
			
			topChild.transform.localPosition = new Vector3(0,0,0);
			topAttachChild.transform.localPosition = new Vector3(0,0.13f,0);

			FuseAttributes fuseAtts = topFuses ();

			topAttachChild.gameObject.AddComponent<FuseBehavior>();
			topAttachChild.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			topAttachChild.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Top"));

			instantiated[1] = newTop;
			partCreated[1] = true;
			selectionManager.newPartCreated("introTopPrefab(Clone)");
			createDirections(parts[1]);

			enableManipulationButtons(newTop);





		}
	}


	//checks to see if an object has been fused already
	public bool alreadyFused(string part) {
		GameObject partInstance = GameObject.Find(part);
		if(partInstance != null && !partInstance.GetComponent<FuseBehavior>().fused ()) {
			return false;
		} else {
			return true;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
