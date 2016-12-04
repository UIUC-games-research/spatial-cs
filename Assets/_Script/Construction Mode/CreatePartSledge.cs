using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CreatePartSledge : MonoBehaviour {

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
		createLoc = new Vector3(-40, 25, 100);
		selectionManager = eventSystem.GetComponent<SelectPart>();

		//CHANGE this string to the name of your starting part
		startObject = GameObject.Find ("startObject");

		//CHANGE these lines so they refer to each black part on your starting part
		GameObject shaftHaftAttach = GameObject.Find("shaft_haft_attach");
		GameObject shaftSmallTrapezoidAttach = GameObject.Find("shaft_small_trapezoid_attach");
		GameObject shaftSpikeAttach = GameObject.Find("shaft_spike_attach");
		GameObject shaftTrapezoidAttach = GameObject.Find("shaft_trapezoid_attach");
		//to avoid errors when selectedObject starts as startObject
		//CHANGE these lines to match above
		shaftHaftAttach.GetComponent<FuseBehavior>().isFused = true;
		shaftSmallTrapezoidAttach.GetComponent<FuseBehavior>().isFused = true;
		shaftSpikeAttach.GetComponent<FuseBehavior>().isFused = true;
		shaftTrapezoidAttach.GetComponent<FuseBehavior>().isFused = true;
		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();

	}

	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at boot from the front)


	//CHANGE these next 5 methods so that they refer to the 5 prefabs you made. This requires you to 
	// change most of the variables and strings in each method. For now, set the fuseLocation to the 
	// location of whatever part you're going to attach it to, set the fuseRotation to the location 
	// (0,0,0), and make acceptableRotations contain only one rotation: Quaternion.Euler (0,0,0). Later,
	// you will come back and change fuseLocation, fuseRotation, and acceptableRotations after testing.

	//returns list of objects body can fuse to
	public FuseAttributes bottomPointLeftFuses() {
		GameObject head = GameObject.Find("head_harderPrefab(Clone)");
		GameObject bottomPointRight = GameObject.Find ("bottom_point_rightPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		if (head != null) {

			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x + 6, headPos.y - 18, headPos.z);
			fuseLocations.Add ("head_bottom_point_left_attach", fuseLocation);
			fuseRotations.Add ("head_bottom_point_left_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (90, 0, 0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("head_bottom_point_left_attach", acceptableRotations);

			fuseLocations.Add ("bottom_point_right_left_attach", fuseLocation);
			fuseRotations.Add ("bottom_point_right_left_attach", fuseRotation);

			fusePositions.Add ("bottom_point_right_left_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes bottomPointRightFuses() {
		GameObject head = GameObject.Find ("head_harderPrefab(Clone)");
		GameObject bottomPointLeft = GameObject.Find ("bottom_point_leftPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(head != null) {
			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x - 6, headPos.y - 18, headPos.z);
			fuseLocations.Add ("head_bottom_point_right_attach", fuseLocation);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("head_bottom_point_right_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (90,0,0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("head_bottom_point_right_attach", acceptableRotations);

			fuseLocations.Add ("bottom_point_left_right_attach", fuseLocation);
			fuseRotations.Add ("bottom_point_left_right_attach", fuseRotation);

			fusePositions.Add ("bottom_point_left_right_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes haftFuses() {
		GameObject shaft = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3 (shaftPos.x, shaftPos.y - 40, shaftPos.z);
		fuseLocations.Add("shaft_haft_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		fuseRotations.Add ("shaft_haft_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,90,0);
		Quaternion acceptableRotation3 = Quaternion.Euler (270,270,0);
		Quaternion acceptableRotation4 = Quaternion.Euler (270,0,0);

		Quaternion[] acceptableRotations = {acceptableRotation1,acceptableRotation2,
			acceptableRotation3,acceptableRotation4};

		fusePositions.Add ("shaft_haft_attach", acceptableRotations);

		newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes headFuses() {
		GameObject trapezoid = GameObject.Find("trapezoid_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(trapezoid != null) {
			Vector3 trapezoidPos = trapezoid.transform.position;
			Vector3 fuseLocation = new Vector3 (trapezoidPos.x, trapezoidPos.y, trapezoidPos.z - 12);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

			fuseLocations.Add ("trapezoid_head_attach", fuseLocation);
			fuseRotations.Add ("trapezoid_head_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
			Quaternion acceptableRotation3 = Quaternion.Euler (90,0,0);

			Quaternion acceptableRotation4 = Quaternion.Euler (270,0,180);
			Quaternion acceptableRotation2 = Quaternion.Euler (270,180,180);

			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2,acceptableRotation3, acceptableRotation4};
			fusePositions.Add ("trapezoid_head_attach", acceptableRotations);
		}


		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes smallTipFuses() {
		//can be fused to any strut
		GameObject smallTrapezoid = GameObject.Find("small_trapezoidPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(smallTrapezoid != null) {
			Vector3 smallTrapezoidPos = smallTrapezoid.transform.position;
			Vector3 fuseLocation = new Vector3(smallTrapezoidPos.x, smallTrapezoidPos.y, smallTrapezoidPos.z + 5.15f);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

			fuseLocations.Add ("small_trapezoid_small_tip_attach", fuseLocation);
			fuseRotations.Add ("small_trapezoid_small_tip_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (90,0,0);

			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
			fusePositions.Add ("small_trapezoid_small_tip_attach", acceptableRotations);

		}
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes smallTrapezoidFuses() {
		//can be fused to any strut
		GameObject shaft = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3(shaftPos.x, shaftPos.y + 28, shaftPos.z + 9.5f);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("shaft_small_trapezoid_attach", fuseLocation);
		fuseRotations.Add ("shaft_small_trapezoid_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (90,0,0);

		Quaternion[] acceptableRotations = {acceptableRotation1,acceptableRotation2};
		fusePositions.Add ("shaft_small_trapezoid_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes spikeFuses() {
		//can be fused to any strut
		GameObject shaft = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3(shaftPos.x, shaftPos.y + 40.5f, shaftPos.z);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("shaft_spike_attach", fuseLocation);
		fuseRotations.Add ("shaft_spike_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,90,0);
		Quaternion acceptableRotation3 = Quaternion.Euler (270,0,0);
		Quaternion acceptableRotation4 = Quaternion.Euler (270,270,0);

		Quaternion[] acceptableRotations = {acceptableRotation1,acceptableRotation2,
			acceptableRotation3,acceptableRotation4};
		fusePositions.Add ("shaft_spike_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes tipFuses() {
		//can be fused to any strut
		GameObject head = GameObject.Find("head_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if (head != null) {
			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x, headPos.y, headPos.z - 9);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3 (0, 180, 0));

			fuseLocations.Add ("head_tip_attach", fuseLocation);
			fuseRotations.Add ("head_tip_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270, 180, 0);
			Quaternion acceptableRotation2 = Quaternion.Euler (90, 0, 0);

			Quaternion[] acceptableRotations = { acceptableRotation1, acceptableRotation2 };
			fusePositions.Add ("head_tip_attach", acceptableRotations);

		}
		FuseAttributes newAttributes = new FuseAttributes (fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes topPointLeftFuses() {
		GameObject head = GameObject.Find("head_harderPrefab(Clone)");
		GameObject topPointRight = GameObject.Find ("top_point_rightPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		if (head != null) {

			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x + 6, headPos.y + 15.2f, headPos.z + 4.8f);
			fuseLocations.Add ("head_top_point_left_attach", fuseLocation);
			fuseRotations.Add ("head_top_point_left_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (270, 180, 0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("head_top_point_left_attach", acceptableRotations);

			fuseLocations.Add ("top_point_right_left_attach", fuseLocation);
			fuseRotations.Add ("top_point_right_left_attach", fuseRotation);

			fusePositions.Add ("top_point_right_left_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes topPointRightFuses() {
		GameObject head = GameObject.Find("head_harderPrefab(Clone)");
		GameObject topPointLeft = GameObject.Find ("top_point_rightPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		if (head != null) {

			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x - 6, headPos.y + 14.7f, headPos.z + 4.3f);
			fuseLocations.Add ("head_top_point_right_attach", fuseLocation);
			fuseRotations.Add ("head_top_point_right_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (270, 180, 0);

			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("head_top_point_right_attach", acceptableRotations);

			fuseLocations.Add ("top_point_left_right_attach", fuseLocation);
			fuseRotations.Add ("top_point_left_right_attach", fuseRotation);

			fusePositions.Add ("top_point_left_right_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes trapezoidFuses() {
		//can be fused to any strut
		GameObject shaft = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3(shaftPos.x, shaftPos.y + 28f, shaftPos.z - 12);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("shaft_trapezoid_attach", fuseLocation);
		fuseRotations.Add ("shaft_trapezoid_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (0,90,90);
		Quaternion acceptableRotation2 = Quaternion.Euler (90,0,0);
		Quaternion acceptableRotation3 = Quaternion.Euler (0,270,270);
		Quaternion acceptableRotation4 = Quaternion.Euler (270,180,0);

		Quaternion[] acceptableRotations = {acceptableRotation1,acceptableRotation2, 
			acceptableRotation3, acceptableRotation4};
		fusePositions.Add ("shaft_trapezoid_attach", acceptableRotations);

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
	public void createBottomPointLeft() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);
			GameObject newBottomPointLeft = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));

			GameObject bottomPointLeftHeadAttach = GameObject.Find("bottom_point_left_head_attach");
			GameObject bottomPointLeftRightAttach = GameObject.Find("bottom_point_left_right_attach");

			FuseAttributes fuseAtts = bottomPointLeftFuses ();

			bottomPointLeftHeadAttach.AddComponent<FuseBehavior>();
			bottomPointLeftHeadAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bottomPointLeftHeadAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BottomPointLeft"));

			bottomPointLeftRightAttach.AddComponent<FuseBehavior>();
			bottomPointLeftRightAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bottomPointLeftRightAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BottomPointLeft"));

			instantiated[0] = newBottomPointLeft;
			partCreated[0] = true;
			selectionManager.newPartCreated("bottom_point_leftPrefab(Clone)");

			enableManipulationButtons(newBottomPointLeft);


		}
	}

	public void createBottomPointRight() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler(0,90,90);
			GameObject newBottomPointRight = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));

			GameObject bottomPointRightHeadAttach = GameObject.Find("bottom_point_right_head_attach");
			GameObject bottomPointRightLeftAttach = GameObject.Find("bottom_point_right_left_attach");

			FuseAttributes fuseAtts = bottomPointRightFuses ();

			bottomPointRightHeadAttach.AddComponent<FuseBehavior>();
			bottomPointRightHeadAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bottomPointRightHeadAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BottomPointRight"));

			bottomPointRightLeftAttach.AddComponent<FuseBehavior>();
			bottomPointRightLeftAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bottomPointRightLeftAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BottomPointRight"));

			instantiated[1] = newBottomPointRight;
			partCreated[1] = true;
			selectionManager.newPartCreated("bottom_point_rightPrefab(Clone)");

			enableManipulationButtons(newBottomPointRight);


		}
	}

	public void createHaft() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (270,270,0);
			GameObject newHaft = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	

			Transform haftShaftAttach = newHaft.transform.FindChild("haft_shaft_attach");

			FuseAttributes fuseAtts = haftFuses ();

			haftShaftAttach.gameObject.AddComponent<FuseBehavior>();
			haftShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			haftShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Haft"));

			instantiated[2] = newHaft;	
			partCreated[2] = true;
			selectionManager.newPartCreated("haftPrefab(Clone)");

			enableManipulationButtons(newHaft);


		}
	}

	public void createHead() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler(90,180,0);
			GameObject newHead = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));

			Transform headTrapezoidAttach = newHead.transform.FindChild("head_trapezoid_attach");
			Transform headTipAttach = newHead.transform.FindChild("head_tip_attach");
			Transform headBottomPointLeftAttach = newHead.transform.FindChild("head_bottom_point_left_attach");
			Transform headBottomPointRightAttach = newHead.transform.FindChild("head_bottom_point_right_attach");
			Transform headTopPointLeftAttach = newHead.transform.FindChild("head_top_point_left_attach");
			Transform headTopPointRightAttach = newHead.transform.FindChild("head_top_point_right_attach");

			FuseAttributes fuseAtts = headFuses ();

			headTrapezoidAttach.gameObject.AddComponent<FuseBehavior>();
			headTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

			headTipAttach.gameObject.AddComponent<FuseBehavior>();
			headTipAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTipAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

			headBottomPointLeftAttach.gameObject.AddComponent<FuseBehavior>();
			headBottomPointLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headBottomPointLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

			headBottomPointRightAttach.gameObject.AddComponent<FuseBehavior>();
			headBottomPointRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headBottomPointRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

			headTopPointLeftAttach.gameObject.AddComponent<FuseBehavior>();
			headTopPointLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTopPointLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

			headTopPointRightAttach.gameObject.AddComponent<FuseBehavior>();
			headTopPointRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTopPointRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

			instantiated[3] = newHead;
			partCreated[3] = true;
			selectionManager.newPartCreated("head_harderPrefab(Clone)");

			enableManipulationButtons(newHead);


		}
	}

	public void createSmallTip() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);		
			GameObject newSmallTip = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));

			Transform smallTipTrapezoidAttach = newSmallTip.transform.FindChild("small_tip_small_trapezoid_attach");

			FuseAttributes fuseAtts = smallTipFuses ();

			smallTipTrapezoidAttach.gameObject.AddComponent<FuseBehavior>();
			smallTipTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			smallTipTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("SmallTip"));

			instantiated[4] = newSmallTip;
			partCreated[4] = true;
			selectionManager.newPartCreated("small_tipPrefab(Clone)");

			enableManipulationButtons(newSmallTip);


		}
	}

	public void createSmallTrapezoid() {
		if(!partCreated[5]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (270,90,0);		
			GameObject newSmallTrapezoid = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[5], pos, fuseToRotation)));

			Transform smallTrapezoidSmallTipAttach = newSmallTrapezoid.transform.FindChild("small_trapezoid_small_tip_attach");
			Transform smallTrapezoidShaftAttach = newSmallTrapezoid.transform.FindChild("small_trapezoid_shaft_attach");

			FuseAttributes fuseAtts = smallTrapezoidFuses ();

			smallTrapezoidSmallTipAttach.gameObject.AddComponent<FuseBehavior>();
			smallTrapezoidSmallTipAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			smallTrapezoidSmallTipAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("SmallTrapezoid"));

			smallTrapezoidShaftAttach.gameObject.AddComponent<FuseBehavior>();
			smallTrapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			smallTrapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("SmallTrapezoid"));

			instantiated[5] = newSmallTrapezoid;
			partCreated[5] = true;
			selectionManager.newPartCreated("small_trapezoidPrefab(Clone)");

			enableManipulationButtons(newSmallTrapezoid);


		}
	}

	public void createSpike() {
		if(!partCreated[6]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,270);		
			GameObject newSpike = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[6], pos, fuseToRotation)));

			Transform spikeShaftAttach = newSpike.transform.FindChild("spike_shaft_attach");

			FuseAttributes fuseAtts = spikeFuses ();

			spikeShaftAttach.gameObject.AddComponent<FuseBehavior>();
			spikeShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			spikeShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Spike"));

			instantiated[6] = newSpike;
			partCreated[6] = true;
			selectionManager.newPartCreated("spikePrefab(Clone)");

			enableManipulationButtons(newSpike);


		}
	}

	public void createTip() {
		if(!partCreated[7]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);		
			GameObject newTip = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[7], pos, fuseToRotation)));

			Transform tipHeadAttach = newTip.transform.FindChild("tip_head_attach");

			FuseAttributes fuseAtts = tipFuses ();

			tipHeadAttach.gameObject.AddComponent<FuseBehavior>();
			tipHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			tipHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Tip"));

			instantiated[7] = newTip;
			partCreated[7] = true;
			selectionManager.newPartCreated("tipPrefab(Clone)");

			enableManipulationButtons(newTip);


		}
	}

	public void createTopPointLeft() {
		if(!partCreated[8]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,180);		
			GameObject newTopPointLeft = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[8], pos, fuseToRotation)));

			Transform topPointLeftHeadAttach = newTopPointLeft.transform.FindChild("top_point_left_head_attach");
			Transform topPointLeftRightAttach = newTopPointLeft.transform.FindChild("top_point_left_right_attach");

			FuseAttributes fuseAtts = topPointLeftFuses ();

			topPointLeftHeadAttach.gameObject.AddComponent<FuseBehavior>();
			topPointLeftHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			topPointLeftHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("TopPointLeft"));

			topPointLeftRightAttach.gameObject.AddComponent<FuseBehavior>();
			topPointLeftRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			topPointLeftRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("TopPointLeft"));

			instantiated[8] = newTopPointLeft;
			partCreated[8] = true;
			selectionManager.newPartCreated("top_point_leftPrefab(Clone)");

			enableManipulationButtons(newTopPointLeft);


		}
	}

	public void createTopPointRight() {
		if(!partCreated[9]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,90,0);		
			GameObject newTopPointRight = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[9], pos, fuseToRotation)));

			Transform topPointRightHeadAttach = newTopPointRight.transform.FindChild("top_point_right_head_attach");
			Transform topPointRightLeftAttach = newTopPointRight.transform.FindChild("top_point_right_left_attach");

			FuseAttributes fuseAtts = topPointRightFuses ();

			topPointRightHeadAttach.gameObject.AddComponent<FuseBehavior>();
			topPointRightHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			topPointRightHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("TopPointRight"));

			topPointRightLeftAttach.gameObject.AddComponent<FuseBehavior>();
			topPointRightLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			topPointRightLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("TopPointRight"));

			instantiated[9] = newTopPointRight;
			partCreated[9] = true;
			selectionManager.newPartCreated("top_point_rightPrefab(Clone)");

			enableManipulationButtons(newTopPointRight);


		}
	}

	public void createTrapezoid() {
		if(!partCreated[10]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,270);		
			GameObject newTrapezoid = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[10], pos, fuseToRotation)));

			Transform trapezoidShaftAttach = newTrapezoid.transform.FindChild("trapezoid_shaft_attach");
			Transform trapezoidHeadAttach = newTrapezoid.transform.FindChild("trapezoid_head_attach");

			FuseAttributes fuseAtts = trapezoidFuses ();

			trapezoidShaftAttach.gameObject.AddComponent<FuseBehavior>();
			trapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			trapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Trapezoid"));

			trapezoidHeadAttach.gameObject.AddComponent<FuseBehavior>();
			trapezoidHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			trapezoidHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Trapezoid"));

			instantiated[10] = newTrapezoid;
			partCreated[10] = true;
			selectionManager.newPartCreated("trapezoid_harderPrefab(Clone)");

			enableManipulationButtons(newTrapezoid);


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
