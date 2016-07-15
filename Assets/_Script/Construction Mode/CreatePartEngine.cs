using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartEngine : MonoBehaviour {
	
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
		startObject = GameObject.Find ("engine_whole");

		//CHANGE these lines so they refer to each black part on your starting part
		GameObject baseEngineFrontAttach = startObject.transform.FindChild("engine_base_engine_front_attach").gameObject;
		GameObject baseEngineLeftAttach = startObject.transform.FindChild("engine_base_engine_left_attach").gameObject;
		GameObject baseEngineRightAttach = startObject.transform.FindChild("engine_base_engine_right_attach").gameObject;
		GameObject baseEngineTopAttach = startObject.transform.FindChild("engine_base_engine_top_attach").gameObject;
		GameObject baseEngineTopRightAttach = startObject.transform.FindChild("engine_base_engine_top_right_attach").gameObject;

		//to avoid errors when selectedObject starts as startObject
		//CHANGE these lines to match above
		baseEngineFrontAttach.GetComponent<FuseBehavior>().isFused = true;
		baseEngineLeftAttach.GetComponent<FuseBehavior>().isFused = true;
		baseEngineRightAttach.GetComponent<FuseBehavior>().isFused = true;
		baseEngineTopAttach.GetComponent<FuseBehavior>().isFused = true;
		baseEngineTopRightAttach.GetComponent<FuseBehavior>().isFused = true;
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
	public FuseAttributes engineFrontFuses() {
		GameObject engineBase = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 engineBasePos = engineBase.transform.position;
		Vector3 fuseLocation = new Vector3 (engineBasePos.x + 22, engineBasePos.y, engineBasePos.z);
		fuseLocations.Add ("engine_base_engine_front_attach", fuseLocation);
		fuseRotations.Add("engine_base_engine_front_attach", fuseRotation);
		
		Quaternion acceptableRotation1 = Quaternion.Euler (0,270,90);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("engine_base_engine_front_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes engineTopFuses() {
		GameObject engineBase = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 engineBasePos = engineBase.transform.position;
		Vector3 fuseLocation = new Vector3 (engineBasePos.x, engineBasePos.y + 13, engineBasePos.z);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseLocations.Add ("engine_base_engine_top_attach", fuseLocation);
		fuseRotations.Add ("engine_base_engine_top_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("engine_base_engine_top_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes engineLeftFuses() {
		GameObject engineBase = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 engineBasePos = engineBase.transform.position;
		Vector3 fuseLocation = new Vector3 (engineBasePos.x, engineBasePos.y + 1, engineBasePos.z - 13);

		fuseLocations.Add("engine_base_engine_left_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("engine_base_engine_left_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (283,270,180);
		Quaternion acceptableRotation2 = Quaternion.Euler (283,180,180);
		Quaternion acceptableRotation3 = Quaternion.Euler (283,90,180);

		Quaternion[] acceptableRotations = {acceptableRotation1,acceptableRotation2,acceptableRotation3};
		
		fusePositions.Add ("engine_base_engine_left_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes engineTopRightFuses() {
		GameObject engineBase = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 engineBasePos = engineBase.transform.position;
		Vector3 fuseLocation = new Vector3 (engineBasePos.x, engineBasePos.y + 8, engineBasePos.z + 13);
		
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("engine_base_engine_top_right_attach", fuseLocation);
		fuseRotations.Add ("engine_base_engine_top_right_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (304,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("engine_base_engine_top_right_attach", acceptableRotations);

		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes engineRightFuses() {
		GameObject engineBase = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 engineBasePos = engineBase.transform.position;
		Vector3 fuseLocation = new Vector3(engineBasePos.x - 1.5f, engineBasePos.y + 1, engineBasePos.z + 13);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("engine_base_engine_right_attach", fuseLocation);
		fuseRotations.Add ("engine_base_engine_right_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("engine_base_engine_right_attach", acceptableRotations);

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
	
	public void createEngineFront() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (90,90,0);
			GameObject newEngineFront = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform engineFrontEngineBaseAttach = newEngineFront.transform.FindChild("engine_front_engine_base_attach");

			FuseAttributes fuseAtts = engineFrontFuses ();
			
			engineFrontEngineBaseAttach.gameObject.AddComponent<FuseBehavior>();
			engineFrontEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			engineFrontEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("EngineFront"));

			instantiated[0] = newEngineFront;
			partCreated[0] = true;
			selectionManager.newPartCreated("engine_frontPrefab(Clone)");
			
			enableManipulationButtons(newEngineFront);
			
			
		}
	}
	
	public void createEngineTop() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newEngineTop = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform engineTopEngineBaseAttach = newEngineTop.transform.FindChild("engine_top_engine_base_attach");
		
			FuseAttributes fuseAtts = engineTopFuses ();
			
			engineTopEngineBaseAttach.gameObject.AddComponent<FuseBehavior>();
			engineTopEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			engineTopEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("EngineTop"));

			instantiated[1] = newEngineTop;
			partCreated[1] = true;
			selectionManager.newPartCreated("engine_topPrefab(Clone)");
			
			enableManipulationButtons(newEngineTop);
			
			
		}
	}
	
	public void createEngineLeft() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,0,90);
			GameObject newEngineLeft = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform engineLeftEngineBaseAttach = newEngineLeft.transform.FindChild("engine_left_engine_base_attach");
	
			FuseAttributes fuseAtts = engineLeftFuses ();
			
			engineLeftEngineBaseAttach.gameObject.AddComponent<FuseBehavior>();
			engineLeftEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			engineLeftEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("EngineLeft"));
	
			instantiated[2] = newEngineLeft;	
			partCreated[2] = true;
			selectionManager.newPartCreated("engine_leftPrefab(Clone)");
			
			enableManipulationButtons(newEngineLeft);
			
			
		}
	}
	
	public void createEngineTopRight() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newEngineTopRight = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform engineTopRightEngineBaseAttach = newEngineTopRight.transform.FindChild("engine_top_right_engine_base_attach");

			FuseAttributes fuseAtts = engineTopRightFuses();
			
			engineTopRightEngineBaseAttach.gameObject.AddComponent<FuseBehavior>();
			engineTopRightEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			engineTopRightEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("EngineTopRight"));
		
			instantiated[3] = newEngineTopRight;
			partCreated[3] = true;
			selectionManager.newPartCreated("engine_top_rightPrefab(Clone)");
			
			enableManipulationButtons(newEngineTopRight);
			
			
		}
	}
	
	public void createEngineRight() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);		
			GameObject newEngineRight = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform engineRightEngineBaseAttach = newEngineRight.transform.FindChild("engine_right_engine_base_attach");

			FuseAttributes fuseAtts = engineRightFuses();
			
			engineRightEngineBaseAttach.gameObject.AddComponent<FuseBehavior>();
			engineRightEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			engineRightEngineBaseAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("EngineRight"));
	
			instantiated[4] = newEngineRight;
			partCreated[4] = true;
			selectionManager.newPartCreated("engine_rightPrefab(Clone)");
			
			enableManipulationButtons(newEngineRight);
			
			
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
