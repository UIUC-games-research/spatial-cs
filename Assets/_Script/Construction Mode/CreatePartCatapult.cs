using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartCatapult : MonoBehaviour {
	
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
		startObject = GameObject.Find ("platform_complete");
		
		//CHANGE these lines so they refer to each black part on your starting part
		GameObject platform_back_axle_bottom_attach = startObject.transform.FindChild("platform_back_axle_bottom_attach").gameObject;
		GameObject platform_back_axle_left_attach = startObject.transform.FindChild("platform_back_axle_left_attach").gameObject;
		GameObject platform_back_axle_right_attach = startObject.transform.FindChild("platform_back_axle_right_attach").gameObject;
		GameObject platform_front_axle_bottom_attach = startObject.transform.FindChild("platform_front_axle_bottom_attach").gameObject;
		GameObject platform_front_axle_left_attach = startObject.transform.FindChild("platform_front_axle_left_attach").gameObject;
		GameObject platform_front_axle_right_attach = startObject.transform.FindChild("platform_front_axle_right_attach").gameObject;
		GameObject platform_left_support_attach = startObject.transform.FindChild("platform_left_support_attach").gameObject;
		GameObject platform_right_support_attach = startObject.transform.FindChild("platform_right_support_attach").gameObject;
	
		
		//to avoid errors when selectedObject starts as startObject
		//CHANGE these lines to match above
		platform_back_axle_bottom_attach.GetComponent<FuseBehavior>().isFused = true;
		platform_back_axle_left_attach.GetComponent<FuseBehavior>().isFused = true;
		platform_back_axle_right_attach.GetComponent<FuseBehavior>().isFused = true;
		platform_front_axle_bottom_attach.GetComponent<FuseBehavior>().isFused = true;
		platform_front_axle_left_attach.GetComponent<FuseBehavior>().isFused = true;
		platform_front_axle_right_attach.GetComponent<FuseBehavior>().isFused = true;
		platform_left_support_attach.GetComponent<FuseBehavior>().isFused = true;
		platform_right_support_attach.GetComponent<FuseBehavior>().isFused = true;
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
	
	//returns list of objects axle can fuse to
	public FuseAttributes axleFuses() {
		GameObject leftSupport = GameObject.Find ("left_support_completePrefab(Clone)");
		GameObject rightSupport = GameObject.Find ("right_support_completePrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 fuseLocation = new Vector3 (0,0,0);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};

		if(leftSupport != null) {
			Vector3 leftSupportPos = leftSupport.transform.position;
			fuseLocation = new Vector3 (leftSupportPos.x, leftSupportPos.y - 0.5f, leftSupportPos.z + 16);
			fuseLocations.Add ("left_support_axle_attach", fuseLocation);
			fuseRotations.Add ("left_support_axle_attach", fuseRotation);
			fusePositions.Add ("left_support_axle_attach", acceptableRotations);
		}
		if(rightSupport != null) {
			Vector3 rightSupportPos = rightSupport.transform.position;
			fuseLocation = new Vector3 (rightSupportPos.x, rightSupportPos.y - 0.5f, rightSupportPos.z - 16);
			fuseLocations.Add ("right_support_axle_attach", fuseLocation);
			fuseRotations.Add ("right_support_axle_attach", fuseRotation);
			fusePositions.Add ("right_support_axle_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;
		
	}

	public FuseAttributes backAxleFuses() {
		GameObject platform = startObject;
		Vector3 platformPos = platform.transform.position;
		Vector3 fuseLocation = new Vector3(platformPos.x + 21, platformPos.y - 4.5f, platformPos.z);
		GameObject frontAxle = GameObject.Find ("front_axle_completePrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		fuseLocations.Add ("platform_back_axle_bottom_attach", fuseLocation);
		fuseRotations.Add ("platform_back_axle_bottom_attach", fuseRotation);

		fuseLocations.Add ("platform_back_axle_left_attach", fuseLocation);
		fuseRotations.Add ("platform_back_axle_left_attach", fuseRotation);

		fuseLocations.Add ("platform_back_axle_right_attach", fuseLocation);
		fuseRotations.Add ("platform_back_axle_right_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (0,180,90);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("platform_back_axle_bottom_attach", acceptableRotations);
		fusePositions.Add ("platform_back_axle_left_attach", acceptableRotations);
		fusePositions.Add ("platform_back_axle_right_attach", acceptableRotations);

		if(frontAxle != null) {
			fuseLocations.Add ("front_axle_back_axle_attach", fuseLocation);
			fuseRotations.Add ("front_axle_back_axle_attach", fuseRotation);
			fusePositions.Add ("front_axle_back_axle_attach", acceptableRotations);

		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}

	public FuseAttributes backRightWheelFuses() {
		GameObject backAxle = GameObject.Find ("back_axle_completePrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(backAxle != null) {
			Vector3 backAxlePos = backAxle.transform.position;
			Vector3 fuseLocation = new Vector3 (backAxlePos.x, backAxlePos.y, backAxlePos.z + 28);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));

			fuseLocations.Add ("back_axle_back_right_wheel_attach", fuseLocation);
			fuseRotations.Add ("back_axle_back_right_wheel_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (0,180,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (0,180,90);
			Quaternion acceptableRotation3 = Quaternion.Euler (0,180,180);
			Quaternion acceptableRotation4 = Quaternion.Euler (0,180,270);
			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2,
				acceptableRotation3,acceptableRotation4};
			fusePositions.Add ("back_axle_back_right_wheel_attach", acceptableRotations);
		}
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}

	public FuseAttributes frontAxleFuses() {
		GameObject platform = startObject;
		GameObject backAxle = GameObject.Find("back_axle_completePrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 platformPos = platform.transform.position;

		Quaternion acceptableRotation1 = Quaternion.Euler (0,180,90);
		Quaternion[] acceptableRotations = {acceptableRotation1};

		Vector3 fuseLocation = new Vector3 (platformPos.x - 21, platformPos.y - 4.5f,platformPos.z);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));

		fuseLocations.Add ("platform_front_axle_bottom_attach", fuseLocation);
		fuseRotations.Add ("platform_front_axle_bottom_attach", fuseRotation);
		
		fuseLocations.Add ("platform_front_axle_left_attach", fuseLocation);
		fuseRotations.Add ("platform_front_axle_left_attach", fuseRotation);
		
		fuseLocations.Add ("platform_front_axle_right_attach", fuseLocation);
		fuseRotations.Add ("platform_front_axle_right_attach", fuseRotation);

		fusePositions.Add ("platform_front_axle_bottom_attach", acceptableRotations);
		fusePositions.Add ("platform_front_axle_left_attach", acceptableRotations);
		fusePositions.Add ("platform_front_axle_right_attach", acceptableRotations);
		
		if(backAxle != null) {
			fuseLocations.Add ("back_axle_front_axle_attach", fuseLocation);
			fuseRotations.Add ("back_axle_front_axle_attach", fuseRotation);
			fusePositions.Add ("back_axle_front_axle_attach", acceptableRotations);
			
		}

		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}

	public FuseAttributes frontLeftWheelFuses() {
		GameObject frontAxle = GameObject.Find ("front_axle_completePrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		if(frontAxle != null) {
			Vector3 frontAxlePos = frontAxle.transform.position;
			Vector3 fuseLocation = new Vector3 (frontAxlePos.x, frontAxlePos.y, frontAxlePos.z - 20);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
			
			fuseLocations.Add ("front_axle_front_left_wheel_attach", fuseLocation);
			fuseRotations.Add ("front_axle_front_left_wheel_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (0,180,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (0,180,90);
			Quaternion acceptableRotation3 = Quaternion.Euler (0,180,180);
			Quaternion acceptableRotation4 = Quaternion.Euler (0,180,270);
			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2,
				acceptableRotation3,acceptableRotation4};
			fusePositions.Add ("front_axle_front_left_wheel_attach", acceptableRotations);
		}
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}

	public FuseAttributes leftSupportFuses() {
		GameObject platform = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 platformPos = platform.transform.position;
		Vector3 fuseLocation = new Vector3 (platformPos.x, platformPos.y + 13, platformPos.z - 16);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("platform_left_support_attach", fuseLocation);
		fuseLocations.Add ("axle_left_support_attach", fuseLocation);
		fuseRotations.Add ("platform_left_support_attach", fuseRotation);
		fuseRotations.Add ("axle_left_support_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (90,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("platform_left_support_attach", acceptableRotations);
		fusePositions.Add ("axle_left_support_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}

	public FuseAttributes rightSupportFuses() {
		GameObject platform = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 platformPos = platform.transform.position;
		Vector3 fuseLocation = new Vector3 (platformPos.x, platformPos.y + 13, platformPos.z + 16);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("platform_right_support_attach", fuseLocation);
		fuseLocations.Add ("axle_right_support_attach", fuseLocation);
		fuseRotations.Add ("platform_right_support_attach", fuseRotation);
		fuseRotations.Add ("axle_right_support_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
		fusePositions.Add ("platform_right_support_attach", acceptableRotations);
		fusePositions.Add ("axle_right_support_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}

	public FuseAttributes throwingArmFuses() {
		GameObject axle = GameObject.Find ("axlePrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		if(axle != null) {
			Vector3 axlePos = axle.transform.position;
			Vector3 fuseLocation = new Vector3 (axlePos.x, axlePos.y + 1.5f, axlePos.z);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
			Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};

			fuseLocations.Add ("axle_throwing_arm_left_attach", fuseLocation);
			fuseRotations.Add ("axle_throwing_arm_left_attach", fuseRotation);

			fuseLocations.Add ("axle_throwing_arm_right_attach", fuseLocation);
			fuseRotations.Add ("axle_throwing_arm_right_attach", fuseRotation);

			fuseLocations.Add ("axle_throwing_arm_bottom_attach", fuseLocation);
			fuseRotations.Add ("axle_throwing_arm_bottom_attach", fuseRotation);

			fusePositions.Add ("axle_throwing_arm_left_attach", acceptableRotations);
			fusePositions.Add ("axle_throwing_arm_right_attach", acceptableRotations);
			fusePositions.Add ("axle_throwing_arm_bottom_attach", acceptableRotations);
		}
		
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
	
	public void createAxle() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (90, 270, 0);
			GameObject newAxle = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform axleLeftSupportAttach = newAxle.transform.FindChild("axle_left_support_attach");
			Transform axleRightSupportAttach = newAxle.transform.FindChild("axle_right_support_attach");
			Transform axleThrowingArmLeftAttach = newAxle.transform.FindChild("axle_throwing_arm_left_attach");
			Transform axleThrowingArmRightAttach = newAxle.transform.FindChild("axle_throwing_arm_right_attach");
			Transform axleThrowingArmBottomAttach = newAxle.transform.FindChild("axle_throwing_arm_bottom_attach");

			FuseAttributes fuseAtts = axleFuses ();
			
			axleLeftSupportAttach.gameObject.AddComponent<FuseBehavior>();
			axleLeftSupportAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			axleLeftSupportAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Axle"));
			
			axleRightSupportAttach.gameObject.AddComponent<FuseBehavior>();
			axleRightSupportAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			axleRightSupportAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Axle"));
			
			axleThrowingArmLeftAttach.gameObject.AddComponent<FuseBehavior>();
			axleThrowingArmLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			axleThrowingArmLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Axle"));

			axleThrowingArmRightAttach.gameObject.AddComponent<FuseBehavior>();
			axleThrowingArmRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			axleThrowingArmRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Axle"));

			axleThrowingArmBottomAttach.gameObject.AddComponent<FuseBehavior>();
			axleThrowingArmBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			axleThrowingArmBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Axle"));

			instantiated[0] = newAxle;
			partCreated[0] = true;
			selectionManager.newPartCreated("axlePrefab(Clone)");
			
			enableManipulationButtons(newAxle);
			
			
		}
	}
	
	public void createThrowingArm() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,270,0);
			GameObject newThrowingArm = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform throwingArmAxleLeftAttach = newThrowingArm.transform.FindChild("throwing_arm_axle_left_attach");
			Transform throwingArmAxleRightAttach = newThrowingArm.transform.FindChild("throwing_arm_axle_right_attach");
			Transform throwingArmAxleBottomAttach = newThrowingArm.transform.FindChild("throwing_arm_axle_bottom_attach");
			
			FuseAttributes fuseAtts = throwingArmFuses ();
			
			throwingArmAxleLeftAttach.gameObject.AddComponent<FuseBehavior>();
			throwingArmAxleLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			throwingArmAxleLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("ThrowingArm"));
			
			throwingArmAxleRightAttach.gameObject.AddComponent<FuseBehavior>();
			throwingArmAxleRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			throwingArmAxleRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("ThrowingArm"));
			
			throwingArmAxleBottomAttach.gameObject.AddComponent<FuseBehavior>();
			throwingArmAxleBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			throwingArmAxleBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("ThrowingArm"));
			
			instantiated[1] = newThrowingArm;
			partCreated[1] = true;
			selectionManager.newPartCreated("throwing_arm_completePrefab(Clone)");
			
			enableManipulationButtons(newThrowingArm);
			
			
		}
	}
	
	public void createRightSupport() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,0,0);
			GameObject newRightSupport = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform rightSupportAxleAttach = newRightSupport.transform.FindChild("right_support_axle_attach");
			Transform rightSupportPlatformAttach = newRightSupport.transform.FindChild("right_support_platform_attach");

			FuseAttributes fuseAtts = rightSupportFuses ();
			
			rightSupportAxleAttach.gameObject.AddComponent<FuseBehavior>();
			rightSupportAxleAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightSupportAxleAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightSupport"));
			
			rightSupportPlatformAttach.gameObject.AddComponent<FuseBehavior>();
			rightSupportPlatformAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightSupportPlatformAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightSupport"));

			instantiated[2] = newRightSupport;	
			partCreated[2] = true;
			selectionManager.newPartCreated("right_support_completePrefab(Clone)");
			
			enableManipulationButtons(newRightSupport);
			
			
		}
	}
	
	public void createBackAxle() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (180,90,0);
			GameObject newBackAxle = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform backAxleFrontAxleAttach = newBackAxle.transform.FindChild("back_axle_front_axle_attach");
			Transform backAxlePlatformLeftAttach = newBackAxle.transform.FindChild("back_axle_platform_left_attach");
			Transform backAxlePlatformRightAttach = newBackAxle.transform.FindChild("back_axle_platform_right_attach");
			Transform backAxlePlatformBottomAttach = newBackAxle.transform.FindChild("back_axle_platform_bottom_attach");
			Transform backAxleBackRightWheelAttach = newBackAxle.transform.FindChild("back_axle_back_right_wheel_attach");

			FuseAttributes fuseAtts = backAxleFuses();
			
			backAxleFrontAxleAttach.gameObject.AddComponent<FuseBehavior>();
			backAxleFrontAxleAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backAxleFrontAxleAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackAxle"));
			
			backAxlePlatformLeftAttach.gameObject.AddComponent<FuseBehavior>();
			backAxlePlatformLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backAxlePlatformLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackAxle"));

			backAxlePlatformRightAttach.gameObject.AddComponent<FuseBehavior>();
			backAxlePlatformRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backAxlePlatformRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackAxle"));

			backAxlePlatformBottomAttach.gameObject.AddComponent<FuseBehavior>();
			backAxlePlatformBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backAxlePlatformBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackAxle"));

			backAxleBackRightWheelAttach.gameObject.AddComponent<FuseBehavior>();
			backAxleBackRightWheelAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backAxleBackRightWheelAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackAxle"));

			instantiated[3] = newBackAxle;
			partCreated[3] = true;
			selectionManager.newPartCreated("back_axle_completePrefab(Clone)");
			
			enableManipulationButtons(newBackAxle);
			
			
		}
	}
	
	public void createFrontAxle() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,180, 90);		
			GameObject newFrontAxle = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform frontAxleBackAxleAttach = newFrontAxle.transform.FindChild("front_axle_back_axle_attach");
			Transform frontAxleFrontLeftWheelAttach = newFrontAxle.transform.FindChild("front_axle_front_left_wheel_attach");
			Transform frontAxlePlatformLeftAttach = newFrontAxle.transform.FindChild("front_axle_platform_left_attach");
			Transform frontAxlePlatformRightAttach = newFrontAxle.transform.FindChild("front_axle_platform_right_attach");
			Transform frontAxlePlatformBottomAttach = newFrontAxle.transform.FindChild("front_axle_platform_bottom_attach");

			FuseAttributes fuseAtts = frontAxleFuses();
			
			frontAxleBackAxleAttach.gameObject.AddComponent<FuseBehavior>();
			frontAxleBackAxleAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			frontAxleBackAxleAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("FrontAxle"));
			
			frontAxleFrontLeftWheelAttach.gameObject.AddComponent<FuseBehavior>();
			frontAxleFrontLeftWheelAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			frontAxleFrontLeftWheelAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("FrontAxle"));

			frontAxlePlatformLeftAttach.gameObject.AddComponent<FuseBehavior>();
			frontAxlePlatformLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			frontAxlePlatformLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("FrontAxle"));

			frontAxlePlatformRightAttach.gameObject.AddComponent<FuseBehavior>();
			frontAxlePlatformRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			frontAxlePlatformRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("FrontAxle"));

			frontAxlePlatformBottomAttach.gameObject.AddComponent<FuseBehavior>();
			frontAxlePlatformBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			frontAxlePlatformBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("FrontAxle"));

			instantiated[4] = newFrontAxle;
			partCreated[4] = true;
			selectionManager.newPartCreated("front_axle_completePrefab(Clone)");
			
			enableManipulationButtons(newFrontAxle);
			
			
		}
	}

	public void createLeftSupport() {
		if(!partCreated[5]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,90);		
			GameObject newLeftSupport = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[5], pos, fuseToRotation)));
			
			Transform leftSupportAxleAttach = newLeftSupport.transform.FindChild("left_support_axle_attach");
			Transform leftSupportPlatformAttach = newLeftSupport.transform.FindChild("left_support_platform_attach");
			
			FuseAttributes fuseAtts = leftSupportFuses();
			
			leftSupportAxleAttach.gameObject.AddComponent<FuseBehavior>();
			leftSupportAxleAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftSupportAxleAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftSupport"));
			
			leftSupportPlatformAttach.gameObject.AddComponent<FuseBehavior>();
			leftSupportPlatformAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftSupportPlatformAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftSupport"));
			
			instantiated[5] = newLeftSupport;
			partCreated[5] = true;
			selectionManager.newPartCreated("left_support_completePrefab(Clone)");
			
			enableManipulationButtons(newLeftSupport);
			
			
		}
	}

	public void createFrontLeftWheel() {
		if(!partCreated[6]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();		
			GameObject newFrontLeftWheel = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[6], pos, fuseToRotation)));
			
			Transform frontLeftWheelFrontAxleAttach = newFrontLeftWheel.transform.FindChild("front_left_wheel_front_axle_attach");

			FuseAttributes fuseAtts = frontLeftWheelFuses();
			
			frontLeftWheelFrontAxleAttach.gameObject.AddComponent<FuseBehavior>();
			frontLeftWheelFrontAxleAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			frontLeftWheelFrontAxleAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("FrontLeftWheel"));

			instantiated[6] = newFrontLeftWheel;
			partCreated[6] = true;
			selectionManager.newPartCreated("front_left_wheel_completePrefab(Clone)");
			
			enableManipulationButtons(newFrontLeftWheel);
			
			
		}
	}

	public void createBackRightWheel() {
		if(!partCreated[7]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();		
			GameObject newBackRightWheel = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[7], pos, fuseToRotation)));
			
			Transform backRightWheelBackAxleAttach = newBackRightWheel.transform.FindChild("back_right_wheel_back_axle_attach");

			FuseAttributes fuseAtts = backRightWheelFuses();
			
			backRightWheelBackAxleAttach.gameObject.AddComponent<FuseBehavior>();
			backRightWheelBackAxleAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backRightWheelBackAxleAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BackRightWheel"));

			instantiated[7] = newBackRightWheel;
			partCreated[7] = true;
			selectionManager.newPartCreated("back_right_wheel_completePrefab(Clone)");
			
			enableManipulationButtons(newBackRightWheel);
			
			
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
