using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartKey1 : MonoBehaviour {
	
	private GameObject[] instantiated;
	public GameObject[] parts;
	private bool[] partCreated;
	private Vector3 createLoc;
	public GameObject eventSystem;
	private SelectPart selectionManager;
	public int NUM_PARTS;
	private GameObject startObject;

	public GameObject rotateLeftButton;
	public GameObject rotateForwardButton;
	public GameObject rotateAcrossButton;
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
		startObject = GameObject.Find ("dangly_T_complete");

		//print (startObject.transform.FindChild ("dangly_T_upright_L_attach"));
		GameObject uprightLAttach = startObject.transform.FindChild("dangly_T_upright_L_attach").gameObject;
		GameObject uprightTAttach = startObject.transform.FindChild("dangly_T_upright_T_attach").gameObject;
		GameObject walkingPantsAttach = startObject.transform.FindChild("dangly_T_walking_pants_attach").gameObject;
		//to avoid errors when selectedObject starts as startObject
		uprightLAttach.GetComponent<FuseBehavior>().isFused = true;
		uprightTAttach.GetComponent<FuseBehavior>().isFused = true;
		walkingPantsAttach.GetComponent<FuseBehavior>().isFused = true;
		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();
	}
	
	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at boot from the front)
	
	//returns list of objects body can fuse to
	public FuseAttributes uprightLFuses() {
		GameObject danglyT = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 danglyTPos = danglyT.transform.position;
		Vector3 fuseLocation = new Vector3 (danglyTPos.x + 9, danglyTPos.y + 8, danglyTPos.z);
		fuseLocations.Add ("dangly_T_upright_L_attach", fuseLocation);
		fuseRotations.Add("dangly_T_upright_L_attach", fuseRotation);

		Quaternion acceptableRotation = Quaternion.Euler (270,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation};
		fusePositions.Add ("dangly_T_upright_L_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;
		
	}
	
	public FuseAttributes uprightTFuses() {
		GameObject dangly_T = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 startPos = dangly_T.transform.position;
		Vector3 fuseLocation = new Vector3 (startPos.x - 7, startPos.y + 24, startPos.z);
		fuseLocations.Add ("dangly_T_upright_T_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
		fuseRotations.Add ("dangly_T_upright_T_attach", fuseRotation);

		Quaternion rotation1 = Quaternion.Euler (0,90,270);
		Quaternion rotation2 = Quaternion.Euler (0,270,270);
		Quaternion[] acceptableRotations = {rotation1,rotation2};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("dangly_T_upright_T_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes waluigiFuses() {
		//can fuse to any side of body or to a side of the generator
		GameObject uprightL = GameObject.Find ("upright_LPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		if(uprightL != null) {
			Vector3 uprightLPos = uprightL.transform.position;
			Vector3 fuseLocation = new Vector3 (uprightLPos.x + 16, uprightLPos.y - 16, uprightLPos.z);
			fuseLocations.Add("upright_L_waluigi_attach", fuseLocation);
			
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
			fuseRotations.Add ("upright_L_waluigi_attach", fuseRotation);
			Quaternion acceptableRotation = Quaternion.Euler (270,180,0);
			Quaternion[] acceptableRotations = {acceptableRotation};

			fusePositions.Add ("upright_L_waluigi_attach", acceptableRotations);
			
			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		}
		
		//need case where strut is fused to generator
		
		return newAttributes;
		
	}
	
	public FuseAttributes walkingPantsFuses() {
		//can fuse to any strut
		GameObject dangly_T = startObject;
		Vector3 danglyTPos = dangly_T.transform.position;
		Vector3 fuseLocation = new Vector3 (danglyTPos.x - 23, danglyTPos.y + 8, danglyTPos.z);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		fuseLocations.Add ("dangly_T_walking_pants_attach", fuseLocation);
		fuseRotations.Add ("dangly_T_walking_pants_attach", fuseRotation);
		Quaternion acceptableRotation = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation};
		fusePositions.Add ("dangly_T_walking_pants_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes uprightRectFuses() {
		//can be fused to any strut
		GameObject walkingPants = GameObject.Find("walking_pantsPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		if(walkingPants != null) {
			Vector3 walkingPantsPos = walkingPants.transform.position;
			Vector3 fuseLocation = new Vector3(walkingPantsPos.x, walkingPantsPos.y, walkingPantsPos.z - 8);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));

			fuseLocations.Add ("walking_pants_upright_rect_attach", fuseLocation);
			fuseRotations.Add ("walking_pants_upright_rect_attach", fuseRotation);
			Quaternion acceptableRotation = Quaternion.Euler (270,270,0);
			Quaternion[] acceptableRotations = {acceptableRotation};
			fusePositions.Add ("walking_pants_upright_rect_attach", acceptableRotations);

		}

		newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

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
		
		if(toRotate.name.Equals ("ebg_bodyPrefab(Clone)") || toRotate.name.Equals ("gripPrefab(Clone)")) {
			rotateLeftButton.transform.GetComponent<Button>().interactable = false;
		} else {
			rotateLeftButton.transform.GetComponent<Button>().interactable = true;
			rotateLeftButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		}
		rotateForwardButton.transform.GetComponent<Button>().interactable = true;
		rotateAcrossButton.transform.GetComponent<Button>().interactable = true;
		
		rotateForwardButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		rotateAcrossButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
	}
	
	
	public void createUprightL() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,0,90);
			GameObject newUprightL = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform uprightLDanglyTAttach = newUprightL.transform.FindChild("upright_L_dangly_T_attach");
			Transform uprightLWaluigiAttach = newUprightL.transform.FindChild("upright_L_waluigi_attach");

			//fixes off center rotation problem
			//bodyChild.transform.localPosition = new Vector3(0,0,0);
			//bodyGripAttach.transform.localPosition = new Vector3(0, -0.35f, 0);
			//bodyStrutLeftAttach.transform.localPosition = new Vector3(-0.6f, 1.4f, -0.42f);
			//bodyStrutRightAttach.transform.localPosition = new Vector3(0.7f, 1.4f, -0.2f);
			//bodyStrutTopAttach.transform.localPosition = new Vector3(0, 1.4f, 0.4f);
			
			FuseAttributes fuseAtts = uprightLFuses ();
			
			uprightLDanglyTAttach.gameObject.AddComponent<FuseBehavior>();
			uprightLDanglyTAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			uprightLDanglyTAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("UprightL"));
			
			uprightLWaluigiAttach.gameObject.AddComponent<FuseBehavior>();
			uprightLWaluigiAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			uprightLWaluigiAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("UprightL"));
			
			instantiated[0] = newUprightL;
			partCreated[0] = true;
			selectionManager.newPartCreated("upright_LPrefab(Clone)");

			enableManipulationButtons(newUprightL);
			
			
		}
	}
	
	public void createUprightT() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newUprightT = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform uprightTdanglyTAttach = newUprightT.transform.FindChild("upright_T_dangly_T_attach");

			//fixes off center rotation problem
			//uprightTdanglyTAttach.transform.localPosition = new Vector3(0, 0, 0);

			FuseAttributes fuseAtts = uprightTFuses ();
			
			uprightTdanglyTAttach.gameObject.AddComponent<FuseBehavior>();
			uprightTdanglyTAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			uprightTdanglyTAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("UprightT"));
			
	
			
			instantiated[1] = newUprightT;
			partCreated[1] = true;
			selectionManager.newPartCreated("uprightTPrefab(Clone)");

			enableManipulationButtons(newUprightT);
			
			
		}
	}
	
	public void createWaluigi() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newWaluigi = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform waluigiUprightLAttach = newWaluigi.transform.FindChild("waluigi_upright_L_attach");

			//fixes off center rotation problem
			//strutTopBodyAttach.transform.localPosition = new Vector3(0, 0, 0);
			//strutTopGeneratorAttach.transform.localPosition = new Vector3(0.08f, 0, -0.72f);
			//strutTopPointyAttach.transform.localPosition = new Vector3(0, 0, 0);
			
			FuseAttributes fuseAtts = waluigiFuses ();
			
			waluigiUprightLAttach.gameObject.AddComponent<FuseBehavior>();
			waluigiUprightLAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			waluigiUprightLAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Waluigi"));

			
			instantiated[2] = newWaluigi;	
			partCreated[2] = true;
			selectionManager.newPartCreated("waluigiPrefab(Clone)");

			enableManipulationButtons(newWaluigi);
			
			
		}
	}
	
	public void createWalkingPants() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newWalkingPants = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform walkingPantsDanglyTAttach = newWalkingPants.transform.FindChild("walking_pants_dangly_T_attach");
			Transform walkingPantsUprightRectAttach = newWalkingPants.transform.FindChild("walking_pants_upright_rect_attach");

			//fixes off center problem
			//generatorStrutLeftAttach.transform.localPosition = new Vector3(0, 0, 0);
			//generatorStrutRightAttach.transform.localPosition = new Vector3(0, 0, 0);
			//generatorStrutTopAttach.transform.localPosition = new Vector3(0, 0, 0);
			
			FuseAttributes fuseAtts = walkingPantsFuses ();
			
			walkingPantsDanglyTAttach.gameObject.AddComponent<FuseBehavior>();
			walkingPantsDanglyTAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			walkingPantsDanglyTAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("WalkingPants"));
			
			walkingPantsUprightRectAttach.gameObject.AddComponent<FuseBehavior>();
			walkingPantsUprightRectAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			walkingPantsUprightRectAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("WalkingPants"));
			
	
			
			instantiated[3] = newWalkingPants;
			partCreated[3] = true;
			selectionManager.newPartCreated("walking_pantsPrefab(Clone)");

			enableManipulationButtons(newWalkingPants);
			
			
		}
	}
	
	public void createUprightRect() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();		
			GameObject newUprightRect = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform uprightRectWalkingPantsAttach = newUprightRect.transform.FindChild("upright_rect_walking_pants_attach");

			//fixes off center rotation problem
			//bodyChild.transform.localPosition = new Vector3(0, 0, 0);
			//pointyStrutTopAttach.transform.localPosition = new Vector3(0, -1.35f, 0);
			//dec1.transform.localPosition = new Vector3(0.05f, 0, -1.52f);
			//dec2.transform.localPosition = new Vector3(0.7f, 0, -1.85f);
			
			FuseAttributes fuseAtts = uprightRectFuses ();
			
			uprightRectWalkingPantsAttach.gameObject.AddComponent<FuseBehavior>();
			uprightRectWalkingPantsAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			uprightRectWalkingPantsAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("UprightRect"));
			
			instantiated[4] = newUprightRect;
			partCreated[4] = true;
			selectionManager.newPartCreated("upright_rectPrefab(Clone)");

			enableManipulationButtons(newUprightRect);
			
			
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
