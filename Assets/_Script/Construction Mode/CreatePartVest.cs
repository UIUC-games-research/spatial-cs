using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartVest : MonoBehaviour {
	
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
		startObject = GameObject.Find ("vest_base_complete");

		//CHANGE these lines so they refer to each black part on your starting part
		GameObject rightStrapBackStrapAttach = startObject.transform.FindChild("right_strap_back_strap_attach").gameObject;
		GameObject vestBaseLeftStrapBottomAttach = startObject.transform.FindChild("vest_base_left_strap_bottom_attach").gameObject;
		GameObject vestBaseLeftStrapTopAttach = startObject.transform.FindChild("vest_base_left_strap_top_attach").gameObject;
		GameObject VestBaseVestDiamondAttach = startObject.transform.FindChild("vest_base_vest_diamond_attach").gameObject;

		//to avoid errors when selectedObject starts as startObject
		//CHANGE these lines to match above
		rightStrapBackStrapAttach.GetComponent<FuseBehavior>().isFused = true;
		vestBaseLeftStrapBottomAttach.GetComponent<FuseBehavior>().isFused = true;
		vestBaseLeftStrapTopAttach.GetComponent<FuseBehavior>().isFused = true;
		VestBaseVestDiamondAttach.GetComponent<FuseBehavior>().isFused = true;

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
	public FuseAttributes leftStrapFuse() {
		GameObject vest = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();	
		
		Vector3 vestPos = vest.transform.position;
		Vector3 fuseLocation = new Vector3 (vestPos.x + 9, vestPos.y , vestPos.z - 19);
		fuseLocations.Add ("back_strap_short_back_strap_long_attach", fuseLocation);
		fuseLocations.Add("vest_base_left_strap_bottom_attach", fuseLocation);
		fuseLocations.Add("vest_base_left_strap_top_attach", fuseLocation);

		fuseRotations.Add ("back_strap_short_back_strap_long_attach", fuseRotation);
		fuseRotations.Add("vest_base_left_strap_bottom_attach", fuseRotation);
		fuseRotations.Add("vest_base_left_strap_top_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("back_strap_short_back_strap_long_attach", acceptableRotations);
		fusePositions.Add ("vest_base_left_strap_bottom_attach", acceptableRotations);
		fusePositions.Add ("vest_base_left_strap_top_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes backStrapFuses() {
		GameObject vest = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 vestPos = vest.transform.position;
		Vector3 fuseLocation = new Vector3 (vestPos.x+13, vestPos.y, vestPos.z);
		fuseLocations.Add ("right_strap_back_strap_attach", fuseLocation);
		fuseLocations.Add ("left_strap_back_strap_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("right_strap_back_strap_attach", fuseRotation);
		fuseRotations.Add ("left_strap_back_strap_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (0,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1,acceptableRotation2};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("right_strap_back_strap_attach", acceptableRotations);
		fusePositions.Add ("left_strap_back_strap_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes leftVestOvalFuses() {
		GameObject vest = startObject;

		Vector3 vestPos = vest.transform.position;
		Vector3 fuseLocation = new Vector3 (vestPos.x-6.5f, vestPos.y, vestPos.z-13.3f);

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


		fuseLocations.Add("vest_diamond_left_vest_oval_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("vest_diamond_left_vest_oval_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (0,270,90);
		Quaternion acceptableRotation2 = Quaternion.Euler (0,270,270);


		Quaternion[] acceptableRotations = {acceptableRotation1,acceptableRotation2};
		
		fusePositions.Add ("vest_diamond_left_vest_oval_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;
		
	}
	
	public FuseAttributes vestDiamondFuses() {
		GameObject vest = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 vestPos = vest.transform.position;
		Vector3 fuseLocation = new Vector3 (vestPos.x - 3, vestPos.y , vestPos.z);


		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));

	//	fuseLocations.Add ("left_vest_oval_vest_diamond_attach", fuseLocation);
	//	fuseLocations.Add ("right_vest_oval_vest_diamond_attach", fuseLocation);
		fuseLocations.Add ("vest_base_vest_diamond_attach", fuseLocation);
	//	fuseLocations.Add ("vest_oval_vest_diamond_attach", fuseLocation);


	//	fuseRotations.Add ("left_vest_oval_vest_diamond_attach", fuseRotation);
	//	fuseRotations.Add ("right_vest_oval_vest_diamond_attach", fuseRotation);
		fuseRotations.Add ("vest_base_vest_diamond_attach", fuseRotation);
	//	fuseRotations.Add ("vest_oval_vest_diamond_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (315,0,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (315,180,0);


		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
	//	fusePositions.Add ("left_vest_oval_vest_diamond_attach", acceptableRotations);
	//	fusePositions.Add ("right_vest_oval_vest_diamond_attach", acceptableRotations);
		fusePositions.Add ("vest_base_vest_diamond_attach", acceptableRotations);
	//	fusePositions.Add ("vest_oval_vest_diamond_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes vestOvalFuses() {
		GameObject vest = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


		Vector3 vestPos = vest.transform.position;
		Vector3 fuseLocation = new Vector3 (vestPos.x-7, vestPos.y , vestPos.z);


		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("vest_diamond_vest_oval_attach", fuseLocation);


		fuseRotations.Add ("vest_diamond_vest_oval_attach", fuseRotation);


		Quaternion acceptableRotation1 = Quaternion.Euler (0,270,90);
		Quaternion acceptableRotation2 = Quaternion.Euler (0,270,270);
		Quaternion[] acceptableRotations = {acceptableRotation1,acceptableRotation2};
		fusePositions.Add ("vest_diamond_vest_oval_attach", acceptableRotations);

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

	public void createLeftStrap(){
		if(!partCreated[1]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (90,0,90);
			GameObject newLeftStrap = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform backStraLongBackStrapShortAttach = newLeftStrap.transform.FindChild("back_strap_long_back_strap_short_attach");
			Transform leftStrapVestBaseBottomAttach = newLeftStrap.transform.FindChild("left_strap_vest_base_bottom_attach");
			Transform leftStrapVestBaseTopAttach = newLeftStrap.transform.FindChild("left_strap_vest_base_top_attach");



			FuseAttributes fuseAtts = leftStrapFuse ();
			
			backStraLongBackStrapShortAttach.gameObject.AddComponent<FuseBehavior>();
			backStraLongBackStrapShortAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backStraLongBackStrapShortAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftStrap"));

			leftStrapVestBaseBottomAttach.gameObject.AddComponent<FuseBehavior>();
			leftStrapVestBaseBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftStrapVestBaseBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftStrap"));

			leftStrapVestBaseTopAttach.gameObject.AddComponent<FuseBehavior>();
			leftStrapVestBaseTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftStrapVestBaseTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftStrap"));

			instantiated[1] = newLeftStrap;
			partCreated[1] = true;
			selectionManager.newPartCreated("left_strapPrefab(Clone)");

			enableManipulationButtons(newLeftStrap);
			
			
		}
	}
	
	public void createBackStrap() {
		if(!partCreated[0]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,270,90);
			GameObject newBack = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform backStrapShortBackStrapLongAttach = newBack.transform.FindChild("back_strap_short_back_strap_long_attach");
			Transform backStrapRightStrapAttach = newBack.transform.FindChild("back_strap_right_strap_attach");

			FuseAttributes fuseAtts = backStrapFuses ();
			
			backStrapShortBackStrapLongAttach.gameObject.AddComponent<FuseBehavior>();
			backStrapShortBackStrapLongAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backStrapShortBackStrapLongAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackStrap"));
			
			backStrapRightStrapAttach.gameObject.AddComponent<FuseBehavior>();
			backStrapRightStrapAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backStrapRightStrapAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackStrap"));

			instantiated[0] = newBack;
			partCreated[0] = true;
			selectionManager.newPartCreated("back_strapPrefab(Clone)");

			enableManipulationButtons(newBack);
			
			
		}
	}
	
	public void createLeftVestOval() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,270,0);
			GameObject newBackSlope = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform leftVestOvalVestDiamondAttach = newBackSlope.transform.FindChild("left_vest_oval_vest_diamond_attach");

			//fixes off center rotation problem
			//strutTopBodyAttach.transform.localPosition = new Vector3(0, 0, 0);
			//strutTopGeneratorAttach.transform.localPosition = new Vector3(0.08f, 0, -0.72f);
			//strutTopPointyAttach.transform.localPosition = new Vector3(0, 0, 0);
			
			FuseAttributes fuseAtts = leftVestOvalFuses ();
			
			leftVestOvalVestDiamondAttach.gameObject.AddComponent<FuseBehavior>();
			leftVestOvalVestDiamondAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftVestOvalVestDiamondAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftOval"));


			instantiated[2] = newBackSlope;	
			partCreated[2] = true;
			selectionManager.newPartCreated("left_vest_ovalPrefab(Clone)");

			enableManipulationButtons(newBackSlope);
			
			
		}
	}
	
	public void createVestDiamond() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);
			GameObject newLeftCover = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform vestDiamondLeftVestOvalAttach = newLeftCover.transform.FindChild("vest_diamond_left_vest_oval_attach");
			Transform vestDiamondRightVestOvalAttach = newLeftCover.transform.FindChild("vest_diamond_right_vest_oval_attach");
			Transform VestDiamondVestBaseAttach = newLeftCover.transform.FindChild("vest_diamond_vest_base_attach");
			Transform VestDiamondVestOvalAttach = newLeftCover.transform.FindChild("vest_diamond_vest_oval_attach");



			FuseAttributes fuseAtts = vestDiamondFuses();
			
			vestDiamondLeftVestOvalAttach.gameObject.AddComponent<FuseBehavior>();
			vestDiamondLeftVestOvalAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			vestDiamondLeftVestOvalAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Diamond"));

			vestDiamondRightVestOvalAttach.gameObject.AddComponent<FuseBehavior>();
			vestDiamondRightVestOvalAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			vestDiamondRightVestOvalAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Diamond"));

			VestDiamondVestBaseAttach.gameObject.AddComponent<FuseBehavior>();
			VestDiamondVestBaseAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			VestDiamondVestBaseAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Diamond"));

			VestDiamondVestOvalAttach.gameObject.AddComponent<FuseBehavior>();
			VestDiamondVestOvalAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			VestDiamondVestOvalAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Diamond"));

			instantiated[4] = newLeftCover;
			partCreated[4] = true;
			selectionManager.newPartCreated("vest_diamondPrefab(Clone)");

			enableManipulationButtons(newLeftCover);
			
			
		}
	}
	
	public void createVestOval() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (180,0,0);		
			GameObject newRightCover = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform vestOvalVestDiamondAttach = newRightCover.transform.FindChild("vest_oval_vest_diamond_attach");

			FuseAttributes fuseAtts = vestOvalFuses();
			
			vestOvalVestDiamondAttach.gameObject.AddComponent<FuseBehavior>();
			vestOvalVestDiamondAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			vestOvalVestDiamondAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Oval"));


			instantiated[3] = newRightCover;
			partCreated[3] = true;
			selectionManager.newPartCreated("vest_ovalPrefab(Clone)");

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
