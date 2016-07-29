using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartHull : MonoBehaviour {
	
	private GameObject[] instantiated;
	public GameObject[] parts;
	private bool[] partCreated;
	private Vector3 createLoc;
	public GameObject eventSystem;
	private SelectPart selectionManager;
	public int NUM_PARTS;
	private GameObject startObject;

	public GameObject rotateYButton;
	public GameObject rotateXButton;
	public GameObject rotateZButton;
	public RotationGizmo rotateGizmo;

	// Use this for initialization
	void Awake () {
		//number of parts to fuse
		partCreated = new bool[NUM_PARTS];
		instantiated = new GameObject[NUM_PARTS];
		for(int i = 0; i < NUM_PARTS; i++) {
			partCreated[i] = false;
		}
		for(int i = 0; i < NUM_PARTS; i++) {
			instantiated[i] = null;
		}
		createLoc = new Vector3(-40, 25, 80);
		selectionManager = eventSystem.GetComponent<SelectPart>();

		//CHANGE this string to the name of your starting part
		startObject = GameObject.Find ("bridgeWhole");

		//CHANGE these lines so they refer to each black part on your starting part
		GameObject bridgeBackAttach = startObject.transform.FindChild("bridge_back_attach").gameObject;
		GameObject bridgeBridgeCoverLeftAttach = startObject.transform.FindChild("bridge_bridge_cover_left_attach").gameObject;
		GameObject bridgeBridgeCoverRightAttach = startObject.transform.FindChild("bridge_bridge_cover_right_attach").gameObject;


		//to avoid errors when selectedObject starts as startObject
		//CHANGE these lines to match above
		bridgeBackAttach.GetComponent<FuseBehavior>().isFused = true;
		bridgeBridgeCoverLeftAttach.GetComponent<FuseBehavior>().isFused = true;
		bridgeBridgeCoverRightAttach.GetComponent<FuseBehavior>().isFused = true;
		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();
	}
	
	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at object from the front)

	//CHANGE these next 5 methods so that they refer to the 5 prefabs you made. This requires you to 
	// change most of the variables and strings in each method. For now, set the fuseLocation to the 
	// location of whatever part you're going to attach it to, set the fuseRotation to the location 
	// (0,0,0), and make acceptableRotations contain only one rotation: Quaternion.Euler (0,0,0). Later,
	// you will come back and change fuseLocation, fuseRotation, and acceptableRotations after testing.
	
	//returns list of objects bridge cover can fuse to
	public FuseAttributes bridgeCoverFuses() {
		GameObject bridge = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 bridgePos = bridge.transform.position;
		Vector3 fuseLocation = new Vector3 (bridgePos.x, bridgePos.y + 8, bridgePos.z);
		fuseLocations.Add ("bridge_bridge_cover_left_attach", fuseLocation);
		fuseRotations.Add("bridge_bridge_cover_left_attach", fuseRotation);
		fuseLocations.Add ("bridge_bridge_cover_right_attach", fuseLocation);
		fuseRotations.Add("bridge_bridge_cover_right_attach", fuseRotation);
		
		Quaternion acceptableRotation1 = Quaternion.Euler (0,90,90);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("bridge_bridge_cover_left_attach", acceptableRotations);
		fusePositions.Add ("bridge_bridge_cover_right_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes backFuses() {
		GameObject bridge = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 bridgePos = bridge.transform.position;
		Vector3 fuseLocation = new Vector3 (bridgePos.x, bridgePos.y + 1, bridgePos.z - 30);
		fuseLocations.Add ("bridge_back_attach", fuseLocation);
		fuseLocations.Add ("left_cover_back_attach", fuseLocation);
		fuseLocations.Add ("right_cover_back_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		fuseRotations.Add ("bridge_back_attach", fuseRotation);
		fuseRotations.Add ("left_cover_back_attach", fuseRotation);
		fuseRotations.Add ("right_cover_back_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,90,90);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("bridge_back_attach", acceptableRotations);
		fusePositions.Add ("left_cover_back_attach", acceptableRotations);
		fusePositions.Add ("right_cover_back_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes backSlopeFuses() {
		GameObject bridgeCover = GameObject.Find ("bridge_coverPrefab(Clone)");
		Vector3 fuseLocation = new Vector3(0,0,0);

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if (bridgeCover != null) {
			Vector3 bridgeCoverPos = bridgeCover.transform.position;
			fuseLocation = new Vector3 (bridgeCoverPos.x, bridgeCoverPos.y, bridgeCoverPos.z - 25);
		}

		fuseLocations.Add("bridge_cover_back_slope_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		fuseRotations.Add ("bridge_cover_back_slope_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (0,90,90);
		Quaternion[] acceptableRotations = {acceptableRotation1};

		fusePositions.Add ("bridge_cover_back_slope_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;
		
	}
	
	public FuseAttributes leftCoverFuses() {
		GameObject back = GameObject.Find("backPrefab(Clone)");
		GameObject backSlope = GameObject.Find("backSlopePrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);

		if(back != null) {
			Vector3 backPos = back.transform.position;
			fuseLocation = new Vector3 (backPos.x - 13, backPos.y + 6, backPos.z);
		} else if (backSlope != null) {
			Vector3 backSlopePos = backSlope.transform.position;
			fuseLocation = new Vector3 (backSlopePos.x, backSlopePos.y, backSlopePos.z);
		}

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("back_left_cover_attach", fuseLocation);
		fuseLocations.Add ("back_slope_left_cover_attach", fuseLocation);
		fuseRotations.Add ("back_left_cover_attach", fuseRotation);
		fuseRotations.Add ("back_slope_left_cover_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,90);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
		fusePositions.Add ("back_left_cover_attach", acceptableRotations);
		fusePositions.Add ("back_slope_left_cover_attach", acceptableRotations);
		
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes rightCoverFuses() {
		GameObject back = GameObject.Find("backPrefab(Clone)");
		GameObject backSlope = GameObject.Find("back_slopePrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);

		if(back != null) {
			Vector3 backPos = back.transform.position;
			fuseLocation = new Vector3(backPos.x + 11.5f, backPos.y + 6, backPos.z);
		} else if (backSlope != null) {
			Vector3 backSlopePos = backSlope.transform.position;
			fuseLocation = new Vector3 (backSlopePos.x + 11.5f, backSlopePos.y - 1, backSlopePos.z - 5);
		}

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		
		fuseLocations.Add ("back_right_cover_attach", fuseLocation);
		fuseLocations.Add ("back_slope_right_cover_attach", fuseLocation);
		fuseRotations.Add ("back_right_cover_attach", fuseRotation);
		fuseRotations.Add ("back_slope_right_cover_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,270);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
		fusePositions.Add ("back_right_cover_attach", acceptableRotations);
		fusePositions.Add ("back_slope_right_cover_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
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

	
	public void enableManipulationButtons(GameObject toRotate) {
		rotateYButton.transform.GetComponent<Button>().interactable = true;
		rotateXButton.transform.GetComponent<Button>().interactable = true;
		rotateZButton.transform.GetComponent<Button>().interactable = true;
		
		rotateYButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		rotateXButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		rotateZButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
	}
	
	//CHANGE these next 5 methods so that they refer to the 5 prefabs you made. This requires you to 
	// change most of the variables and strings in each method.

	public void createBridgeCover() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = new Quaternion();
			GameObject newBridgeCover = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform bridgeCoverBridgeLeftAttach = newBridgeCover.transform.FindChild("bridge_cover_bridge_left_attach");
			Transform bridgeCoverBridgeRightAttach = newBridgeCover.transform.FindChild("bridge_cover_bridge_right_attach");
			Transform bridgeCoverBackSlopeAttach = newBridgeCover.transform.FindChild("bridge_cover_back_slope_attach");

			FuseAttributes fuseAtts = bridgeCoverFuses ();
			
			bridgeCoverBridgeLeftAttach.gameObject.AddComponent<FuseBehavior>();
			bridgeCoverBridgeLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bridgeCoverBridgeLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BridgeCover"));

			bridgeCoverBridgeRightAttach.gameObject.AddComponent<FuseBehavior>();
			bridgeCoverBridgeRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bridgeCoverBridgeRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BridgeCover"));

			bridgeCoverBackSlopeAttach.gameObject.AddComponent<FuseBehavior>();
			bridgeCoverBackSlopeAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bridgeCoverBackSlopeAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BridgeCover"));

			instantiated[0] = newBridgeCover;
			partCreated[0] = true;
			selectionManager.newPartCreated("bridgeCoverPrefab(Clone)");

			enableManipulationButtons(newBridgeCover);
			
			
		}
	}
	
	public void createBack() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);
			GameObject newBack = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform backLeftCoverAttach = newBack.transform.FindChild("back_left_cover_attach");
			Transform backRightCoverAttach = newBack.transform.FindChild("back_right_cover_attach");
			Transform backBridgeAttach = newBack.transform.FindChild("back_bridge_attach");

			FuseAttributes fuseAtts = backFuses ();
			
			backLeftCoverAttach.gameObject.AddComponent<FuseBehavior>();
			backLeftCoverAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backLeftCoverAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Back"));
			
			backRightCoverAttach.gameObject.AddComponent<FuseBehavior>();
			backRightCoverAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backRightCoverAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Back"));

			backBridgeAttach.gameObject.AddComponent<FuseBehavior>();
			backBridgeAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backBridgeAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Back"));

			instantiated[1] = newBack;
			partCreated[1] = true;
			selectionManager.newPartCreated("backPrefab(Clone)");

			enableManipulationButtons(newBack);
			
			
		}
	}
	
	public void createBackSlope() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,180,0);
			GameObject newBackSlope = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform backSlopeBridgeCoverAttach = newBackSlope.transform.FindChild("back_slope_bridge_cover_attach");
			Transform backSlopeLeftCoverAttach = newBackSlope.transform.FindChild("back_slope_left_cover_attach");
			Transform backSlopeRightCoverAttach = newBackSlope.transform.FindChild("back_slope_right_cover_attach");

			//fixes off center rotation problem
			//strutTopBodyAttach.transform.localPosition = new Vector3(0, 0, 0);
			//strutTopGeneratorAttach.transform.localPosition = new Vector3(0.08f, 0, -0.72f);
			//strutTopPointyAttach.transform.localPosition = new Vector3(0, 0, 0);
			
			FuseAttributes fuseAtts = backSlopeFuses ();
			
			backSlopeBridgeCoverAttach.gameObject.AddComponent<FuseBehavior>();
			backSlopeBridgeCoverAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backSlopeBridgeCoverAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackSlope"));

			backSlopeLeftCoverAttach.gameObject.AddComponent<FuseBehavior>();
			backSlopeLeftCoverAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backSlopeLeftCoverAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackSlope"));
		
			backSlopeRightCoverAttach.gameObject.AddComponent<FuseBehavior>();
			backSlopeRightCoverAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backSlopeRightCoverAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackSlope"));

			instantiated[2] = newBackSlope;	
			partCreated[2] = true;
			selectionManager.newPartCreated("backSlopePrefab(Clone)");

			enableManipulationButtons(newBackSlope);
			
			
		}
	}
	
	public void createLeftCover() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newLeftCover = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform leftCoverBackSlopeAttach = newLeftCover.transform.FindChild("left_cover_back_slope_attach");
			Transform leftCoverBackAttach = newLeftCover.transform.FindChild("left_cover_back_attach");
			
			FuseAttributes fuseAtts = leftCoverFuses();
			
			leftCoverBackSlopeAttach.gameObject.AddComponent<FuseBehavior>();
			leftCoverBackSlopeAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftCoverBackSlopeAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftCover"));

			leftCoverBackAttach.gameObject.AddComponent<FuseBehavior>();
			leftCoverBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftCoverBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftCover"));

			instantiated[3] = newLeftCover;
			partCreated[3] = true;
			selectionManager.newPartCreated("left_coverPrefab(Clone)");

			enableManipulationButtons(newLeftCover);
			
			
		}
	}
	
	public void createRightCover() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,90);		
			GameObject newRightCover = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform rightCoverBackAttach = newRightCover.transform.FindChild("right_cover_back_attach");
			Transform rightCoverBackSlopeAttach = newRightCover.transform.FindChild("right_cover_back_slope_attach");

			FuseAttributes fuseAtts = rightCoverFuses();
			
			rightCoverBackAttach.gameObject.AddComponent<FuseBehavior>();
			rightCoverBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightCoverBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightCover"));

			rightCoverBackSlopeAttach.gameObject.AddComponent<FuseBehavior>();
			rightCoverBackSlopeAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightCoverBackSlopeAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightCover"));

			instantiated[4] = newRightCover;
			partCreated[4] = true;
			selectionManager.newPartCreated("right_coverPrefab(Clone)");

			enableManipulationButtons(newRightCover);
			
			
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
