using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartForceGloves : MonoBehaviour {
	
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
		startObject = GameObject.Find ("armWhole");
		GameObject armArmDecAttach = startObject.transform.FindChild("arm_arm_dec_attach").gameObject;
		GameObject armPalmAttach = startObject.transform.FindChild("arm_palm_attach").gameObject;
		
		//to avoid errors when selectedObject starts as startObject
		armArmDecAttach.GetComponent<FuseBehavior>().isFused = true;
		armPalmAttach.GetComponent<FuseBehavior>().isFused = true;
		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();
	}
	
	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at object from the front)
	
	//returns list of objects ring can fuse to
	public FuseAttributes palmFuses() {
		//fuseLocations for palm: arm palm
		//acceptable rotations: 1
		
		GameObject arm = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 armPos = arm.transform.position;
		Vector3 fuseLocation = new Vector3 (armPos.x, armPos.y + 4.5f, armPos.z - 30);
		fuseLocations.Add ("arm_palm_attach", fuseLocation);
		fuseRotations.Add ("arm_palm_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("arm_palm_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes fingersFuses() {
		//fuseLocations: 1
		//acceptable rotations: 1
		GameObject palm = GameObject.Find ("palmPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(palm != null) {
			Vector3 palmPos = palm.transform.position;
			Vector3 fuseLocation = new Vector3 (palmPos.x, palmPos.y, palmPos.z - 17);
			fuseLocations.Add ("palm_fingers_attach", fuseLocation);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("palm_fingers_attach", fuseRotation);
			
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions = new Dictionary<string, Quaternion[]>();
			fusePositions.Add ("palm_fingers_attach", acceptableRotations);
		}
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes thumbFuses() {
		// fuse locations: 1 
		// acceptable rotations: 1
		GameObject palm = GameObject.Find ("palmPrefab(Clone)");
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(palm != null) {
			Vector3 palmPos = palm.transform.position;
			Vector3 fuseLocation = new Vector3 (palmPos.x + 15, palmPos.y, palmPos.z - 10);
			fuseLocations.Add("palm_thumb_attach", fuseLocation);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("palm_thumb_attach", fuseRotation);
			
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			
			fusePositions.Add ("palm_thumb_attach", acceptableRotations);
		}


		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes armDecFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject arm = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 armPos = arm.transform.position;
		Vector3 fuseLocation = new Vector3 (armPos.x, armPos.y + 10, armPos.z - 2);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		
		fuseLocations.Add ("arm_arm_dec_attach", fuseLocation);
		fuseRotations.Add ("arm_arm_dec_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("arm_arm_dec_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes palmDecFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject palm = GameObject.Find("palmPrefab(Clone)");
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(palm != null) {
			Vector3 palmPos = palm.transform.position;
			Vector3 fuseLocation = new Vector3(palmPos.x, palmPos.y + 4, palmPos.z);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			
			fuseLocations.Add ("palm_palm_dec_attach", fuseLocation);
			fuseRotations.Add ("palm_palm_dec_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("palm_palm_dec_attach", acceptableRotations);
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
	
	
	public void createPalm() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (90,0,90);
			GameObject newPalm = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform palmArmAttach = newPalm.transform.FindChild("palm_arm_attach");
			Transform palmFingersAttach = newPalm.transform.FindChild("palm_fingers_attach");
			Transform palmThumbAttach = newPalm.transform.FindChild("palm_thumb_attach");
			Transform palmPalmDecAttach = newPalm.transform.FindChild("palm_palm_dec_attach");
			
			FuseAttributes fuseAtts = palmFuses ();
			
			palmArmAttach.gameObject.AddComponent<FuseBehavior>();
			palmArmAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			palmArmAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Palm"));
			
			palmFingersAttach.gameObject.AddComponent<FuseBehavior>();
			palmFingersAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			palmFingersAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Palm"));
			
			palmThumbAttach.gameObject.AddComponent<FuseBehavior>();
			palmThumbAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			palmThumbAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Palm"));
			
			palmPalmDecAttach.gameObject.AddComponent<FuseBehavior>();
			palmPalmDecAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			palmPalmDecAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Palm"));
			
			instantiated[0] = newPalm;
			partCreated[0] = true;
			selectionManager.newPartCreated("palmPrefab(Clone)");
			
			enableManipulationButtons(newPalm);
			
			
		}
	}
	
	public void createFingers() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (180,90,0);
			GameObject newFingers = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform fingersPalmAttach = newFingers.transform.FindChild("fingers_palm_attach");

			FuseAttributes fuseAtts = fingersFuses ();
			
			fingersPalmAttach.gameObject.AddComponent<FuseBehavior>();
			fingersPalmAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			fingersPalmAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Fingers"));
			
			instantiated[1] = newFingers;
			partCreated[1] = true;
			selectionManager.newPartCreated("fingersPrefab(Clone)");
			
			enableManipulationButtons(newFingers);
			
			
		}
	}
	
	public void createThumb() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,270,0);
			GameObject newThumb = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform thumbPalmAttach = newThumb.transform.FindChild("thumb_palm_attach");
			
			FuseAttributes fuseAtts = thumbFuses ();
			
			thumbPalmAttach.gameObject.AddComponent<FuseBehavior>();
			thumbPalmAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			thumbPalmAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Thumb"));
			
			instantiated[2] = newThumb;	
			partCreated[2] = true;
			selectionManager.newPartCreated("thumbPrefab(Clone)");
			
			enableManipulationButtons(newThumb);
			
			
		}
	}
	
	public void createArmDec() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,0,0);
			GameObject newArmDec = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform armDecArmAttach = newArmDec.transform.FindChild("arm_dec_arm_attach");
			
			FuseAttributes fuseAtts = armDecFuses();
			
			armDecArmAttach.gameObject.AddComponent<FuseBehavior>();
			armDecArmAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			armDecArmAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("ArmDec"));
			
			instantiated[3] = newArmDec;
			partCreated[3] = true;
			selectionManager.newPartCreated("arm_decPrefab(Clone)");
			
			enableManipulationButtons(newArmDec);
			
			
		}
	}
	
	public void createPalmDec() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,180,90);		
			GameObject newPalmDec = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform palmDecPalmAttach = newPalmDec.transform.FindChild("palm_dec_palm_attach");

			FuseAttributes fuseAtts = palmDecFuses();
			
			palmDecPalmAttach.gameObject.AddComponent<FuseBehavior>();
			palmDecPalmAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			palmDecPalmAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("PalmDec"));

			instantiated[4] = newPalmDec;
			partCreated[4] = true;
			selectionManager.newPartCreated("palm_decPrefab(Clone)");
			
			enableManipulationButtons(newPalmDec);
			
			
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
