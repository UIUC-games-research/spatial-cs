using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartAxe : MonoBehaviour {
	
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
		GameObject shaftHaftAttach = startObject.transform.FindChild("shaft_haft_attach").gameObject;
		GameObject shaftTrapezoidAttach = startObject.transform.FindChild("shaft_trapezoid_attach").gameObject;
		//to avoid errors when selectedObject starts as startObject
		//CHANGE these lines to match above
		shaftHaftAttach.GetComponent<FuseBehavior>().isFused = true;
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
	public FuseAttributes haftFuses() {
		GameObject shaft = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 shaftPos = shaft.transform.position;
		Vector3 fuseLocation = new Vector3 (shaftPos.x, shaftPos.y - 30, shaftPos.z);
		fuseLocations.Add ("shaft_haft_attach", fuseLocation);
		fuseRotations.Add("shaft_haft_attach", fuseRotation);
		
		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,90,0);
		Quaternion acceptableRotation3 = Quaternion.Euler (270,180,0);
		Quaternion acceptableRotation4 = Quaternion.Euler (270,270,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2, 
			acceptableRotation3, acceptableRotation4};
		fusePositions.Add ("shaft_haft_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes headFuses() {
		GameObject trapezoid = GameObject.Find ("trapezoidPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(trapezoid != null) {
			Vector3 trapezoidPos = trapezoid.transform.position;
			Vector3 fuseLocation = new Vector3 (trapezoidPos.x, trapezoidPos.y - 0.5f, trapezoidPos.z - 12);
			fuseLocations.Add ("trapezoid_head_attach", fuseLocation);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("trapezoid_head_attach", fuseRotation);
			
			Quaternion acceptableRotation1 = Quaternion.Euler (90,0,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (0,90,90);
			Quaternion acceptableRotation3 = Quaternion.Euler (270,180,0);
			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2, acceptableRotation3};
			fusePositions = new Dictionary<string, Quaternion[]>();
			fusePositions.Add ("trapezoid_head_attach", acceptableRotations);
		}
	
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes trapezoidFuses() {
		GameObject shaft = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		if(shaft != null) {
			Vector3 shaftPos = shaft.transform.position;
			Vector3 fuseLocation = new Vector3 (shaftPos.x, shaftPos.y + 21, shaftPos.z - 12);
			fuseLocations.Add("shaft_trapezoid_attach", fuseLocation);
			
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("shaft_trapezoid_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (90,0,0);
			Quaternion acceptableRotation3 = Quaternion.Euler (0,90,90);
			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2, acceptableRotation3};
			
			fusePositions.Add ("shaft_trapezoid_attach", acceptableRotations);
			
			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		}

		return newAttributes;
		
	}
	
	public FuseAttributes bottomPointFuses() {
		GameObject head = GameObject.Find("headPrefab(Clone)");
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(head != null) {
			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3 (headPos.x, headPos.y - 17.5f, headPos.z);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

			fuseLocations.Add ("head_bottom_point_attach", fuseLocation);
			fuseRotations.Add ("head_bottom_point_attach", fuseRotation);
			Quaternion acceptableRotation = Quaternion.Euler (90,0,0);
			Quaternion[] acceptableRotations = {acceptableRotation};
			fusePositions.Add ("head_bottom_point_attach", acceptableRotations);
		}

		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes topPointFuses() {
		//can be fused to any strut
		GameObject head = GameObject.Find("headPrefab(Clone)");
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(head != null) {
			Vector3 headPos = head.transform.position;
			Vector3 fuseLocation = new Vector3(headPos.x, headPos.y + 15, headPos.z + 5);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			
			fuseLocations.Add ("head_top_point_attach", fuseLocation);
			fuseRotations.Add ("head_top_point_attach", fuseRotation);
			Quaternion acceptableRotation = Quaternion.Euler (270,180,0);
			Quaternion[] acceptableRotations = {acceptableRotation};
			fusePositions.Add ("head_top_point_attach", acceptableRotations);
			
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
	public void createHaft() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);
			GameObject newHaft = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform haftShaftAttach = newHaft.transform.FindChild("haft_shaft_attach");
			
			FuseAttributes fuseAtts = haftFuses ();
			
			haftShaftAttach.gameObject.AddComponent<FuseBehavior>();
			haftShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			haftShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Haft"));

			instantiated[0] = newHaft;
			partCreated[0] = true;
			selectionManager.newPartCreated("haftPrefab(Clone)");

			enableManipulationButtons(newHaft);
			
			
		}
	}
	
	public void createHead() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newHead = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform headTrapezoidAttach = newHead.transform.FindChild("head_trapezoid_attach");
			Transform headBottomPointAttach = newHead.transform.FindChild("head_bottom_point_attach");
			Transform headTopPointAttach = newHead.transform.FindChild("head_top_point_attach");
			
			FuseAttributes fuseAtts = headFuses ();
			
			headTrapezoidAttach.gameObject.AddComponent<FuseBehavior>();
			headTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTrapezoidAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

			headBottomPointAttach.gameObject.AddComponent<FuseBehavior>();
			headBottomPointAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headBottomPointAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));

			headTopPointAttach.gameObject.AddComponent<FuseBehavior>();
			headTopPointAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			headTopPointAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Head"));
			
			
			instantiated[1] = newHead;
			partCreated[1] = true;
			selectionManager.newPartCreated("headPrefab(Clone)");

			enableManipulationButtons(newHead);
			
			
		}
	}
	
	public void createTrapezoid() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (270,0,90);
			GameObject newTrapezoid = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform trapezoidHeadAttach = newTrapezoid.transform.FindChild("trapezoid_head_attach");
			Transform trapezoidShaftAttach = newTrapezoid.transform.FindChild("trapezoid_shaft_attach");
			
			FuseAttributes fuseAtts = trapezoidFuses ();
			
			trapezoidHeadAttach.gameObject.AddComponent<FuseBehavior>();
			trapezoidHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			trapezoidHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Trapezoid"));

			trapezoidShaftAttach.gameObject.AddComponent<FuseBehavior>();
			trapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			trapezoidShaftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Trapezoid"));
			
			instantiated[2] = newTrapezoid;	
			partCreated[2] = true;
			selectionManager.newPartCreated("trapezoidPrefab(Clone)");

			enableManipulationButtons(newTrapezoid);
			
			
		}
	}
	
	public void createBottomPoint() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newBottomPoint = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform bottomPointHeadAttach = newBottomPoint.transform.FindChild("bottom_point_head_attach");

			//fixes off center problem
			//generatorStrutLeftAttach.transform.localPosition = new Vector3(0, 0, 0);
			//generatorStrutRightAttach.transform.localPosition = new Vector3(0, 0, 0);
			//generatorStrutTopAttach.transform.localPosition = new Vector3(0, 0, 0);
			
			FuseAttributes fuseAtts = bottomPointFuses ();
			
			bottomPointHeadAttach.gameObject.AddComponent<FuseBehavior>();
			bottomPointHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bottomPointHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BottomPoint"));

			instantiated[3] = newBottomPoint;
			partCreated[3] = true;
			selectionManager.newPartCreated("bottom_pointPrefab(Clone)");

			enableManipulationButtons(newBottomPoint);
			
			
		}
	}
	
	public void createTopPoint() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,270);		
			GameObject newTopPoint = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform topPointHeadAttach = newTopPoint.transform.FindChild("top_point_head_attach");
			
			FuseAttributes fuseAtts = topPointFuses ();
			
			topPointHeadAttach.gameObject.AddComponent<FuseBehavior>();
			topPointHeadAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			topPointHeadAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("TopPoint"));
			
			instantiated[4] = newTopPoint;
			partCreated[4] = true;
			selectionManager.newPartCreated("top_pointPrefab(Clone)");

			enableManipulationButtons(newTopPoint);
			
			
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
