using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePart : MonoBehaviour {
	
	private GameObject[] instantiated;
	public GameObject[] parts;
	private bool[] partCreated;
	private Vector3 createLoc;
	public GameObject eventSystem;
	private SelectPart selectionManager;
	public int NUM_PARTS;
	private GameObject startObject;
	private string password = "e7O9aT";

	public GameObject rotateLeftButton;
	public GameObject rotateForwardButton;
	public GameObject rotateAcrossButton;
	
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
		startObject = GameObject.Find ("rocket_boots_start");

		//to avoid errors when selectedObject starts as startObject
		startObject.GetComponent<IsFused>().isFused = true;
	}

	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at boot from the front)

	//returns list of objects body can fuse to
	public FuseAttributes bodyFuses() {
		GameObject soleHeel = GameObject.Find ("rocket_boots_start");
		GameObject toe = GameObject.Find ("ToePrefab(Clone)");
		Vector3 soleHeelPos = soleHeel.transform.position;
		Vector3 fuseLocation = new Vector3 (soleHeelPos.x, soleHeelPos.y + 10, soleHeelPos.z);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Quaternion acceptableRotation = Quaternion.Euler (new Vector3(270,180,0));

		Quaternion[] acceptableRotations = {acceptableRotation};

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		fuseLocations.Add ("Sole_Heel_Top_Attach", fuseLocation);
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		fuseRotations.Add ("Sole_Heel_Top_Attach", fuseRotation);
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("Sole_Heel_Top_Attach", acceptableRotations);
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		if(toe != null) {
			fusePositions.Add ("Toe_Side_Attach", acceptableRotations);
			fuseLocations.Add ("Toe_Side_Attach", fuseLocation);
			fuseRotations.Add ("Toe_Side_Attach", fuseRotation);

			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		}

		return newAttributes;

	}

	public FuseAttributes calfFuses() {
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		fuseRotations.Add ("Body_Top_Attach", fuseRotation);
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		GameObject body = GameObject.Find ("BodyPrefab(Clone)");
		if(body != null) {
			Vector3 bodyPos = body.transform.position;
			Vector3 fuseLocation = new Vector3 (bodyPos.x, bodyPos.y + 25, bodyPos.z + 9.5f);
			fuseLocations.Add ("Body_Top_Attach", fuseLocation);
			Quaternion alternateRotation1 = Quaternion.Euler (new Vector3(270,0,0));
			Quaternion alternateRotation2 = Quaternion.Euler (new Vector3(270,90,0));
			Quaternion alternateRotation3 = Quaternion.Euler (new Vector3(270,270,0));
			Quaternion alternateRotation4 = Quaternion.Euler (new Vector3(270,180,0));

			Quaternion[] acceptableRotations = { alternateRotation1, alternateRotation2, 
				alternateRotation3, alternateRotation4};

			fusePositions.Add ("Body_Top_Attach", acceptableRotations);
			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		}
			
		return newAttributes;
		
	}

	public FuseAttributes trimFuses() {
		GameObject calf = GameObject.Find ("calfPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		if(calf != null) {
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("Calf_Top_Attach", fuseRotation);
			Vector3 calfPos = calf.transform.position;
			if(calfPos.x == 0 && calfPos.y == 0 && calfPos.z == 0) {
				print ("Invalid calfPos: trying again");
				calfPos = calf.transform.position;
			}
			Vector3 fuseLocation = new Vector3 (calfPos.x, calfPos.y + 21, calfPos.z);
			fuseLocations.Add ("Calf_Top_Attach", fuseLocation);
			Quaternion alternateRotation1 = Quaternion.Euler (new Vector3(270,0,0));
			Quaternion alternateRotation2 = Quaternion.Euler (new Vector3(270,90,0));
			Quaternion alternateRotation3 = Quaternion.Euler (new Vector3(270,270,0));
			Quaternion alternateRotation4 = Quaternion.Euler (new Vector3(270,180,0));
			Quaternion[] acceptableRotations = {alternateRotation1, alternateRotation2, 
				alternateRotation3, alternateRotation4};

			fusePositions.Add ("Calf_Top_Attach", acceptableRotations);
			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		}
			
		return newAttributes;
		
	}

	public FuseAttributes toeFuses() {
		GameObject soleToe = GameObject.Find ("ToeSolePrefab(Clone)");
		GameObject body = GameObject.Find ("BodyPrefab(Clone)");
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Quaternion acceptableRotation1 = Quaternion.Euler (new Vector3(270,180,0));
		Quaternion acceptableRotation2 = Quaternion.Euler (new Vector3(90,0,0));

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		if(soleToe != null) {
			Vector3 soleToePos = soleToe.transform.position;
			Vector3 fuseLocation1 = new Vector3 (soleToePos.x, soleToePos.y + 13, soleToePos.z);
			Quaternion[] acceptableRotationsSoleToe = {acceptableRotation2};

			fuseLocations.Add ("Sole_Toe_Top_Attach", fuseLocation1);
			fuseRotations.Add ("Sole_Toe_Top_Attach", fuseRotation);
			fusePositions.Add ("Sole_Toe_Top_Attach", acceptableRotationsSoleToe);
		}
		if(body != null) {
			Vector3 bodyPos = body.transform.position;
			Quaternion[] acceptableRotationsBody = {acceptableRotation1};
			if(soleToe == null) {
				Vector3 fuseLocation2 = new Vector3 (bodyPos.x, bodyPos.y + 3, bodyPos.z - 35);

				fuseLocations.Add ("Body_Side_Attach", fuseLocation2);
				fuseRotations.Add ("Body_Side_Attach", fuseRotation);
			} else {
				Vector3 soleToePos = soleToe.transform.position;
				Vector3 fuseLocation1 = new Vector3 (soleToePos.x, soleToePos.y + 13, soleToePos.z);
				fuseLocations.Add ("Body_Side_Attach", fuseLocation1);
				fuseRotations.Add ("Body_Side_Attach", fuseRotation);
			}
			fusePositions.Add ("Body_Side_Attach", acceptableRotationsBody);
		}

		newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;
		
	}

	public FuseAttributes toeSoleFuses() {
		GameObject soleHeel = GameObject.Find ("rocket_boots_start");
		GameObject toe = GameObject.Find ("ToePrefab(Clone)");
		Vector3 soleHeelPos = soleHeel.transform.position;
		Vector3 fuseLocation = new Vector3 (soleHeelPos.x, soleHeelPos.y, soleHeelPos.z - 35);
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		fuseLocations.Add ("Sole_Heel_Side_Attach", fuseLocation);
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Quaternion acceptableRotation = Quaternion.Euler (new Vector3(270,180,0));
		fuseRotations.Add ("Sole_Heel_Side_Attach", fuseRotation);

		Quaternion[] acceptableRotations = {acceptableRotation};

		if(soleHeel != null) {
			fusePositions.Add ("Sole_Heel_Side_Attach", acceptableRotations);

		}
		if(toe != null) {
			fusePositions.Add ("Toe_Bottom_Attach", acceptableRotations);
			fuseLocations.Add ("Toe_Bottom_Attach", fuseLocation);
			fuseRotations.Add ("Toe_Bottom_Attach", fuseRotation);

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


	public void createBody() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = new Quaternion();
			GameObject newBody = LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation));

			Transform bodyTopAttach = newBody.transform.FindChild("Body_Top_Attach");
			Transform bodySideAttach = newBody.transform.FindChild("Body_Side_Attach");
			Transform bodyBottomAttach = newBody.transform.FindChild("Body_Bottom_Attach");

			FuseAttributes fuseAtts = bodyFuses ();

			bodyTopAttach.gameObject.AddComponent<FuseBehavior>();
			bodyTopAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bodyTopAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Body"));

			bodySideAttach.gameObject.AddComponent<FuseBehavior>();
			bodySideAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bodySideAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Body"));

			bodyBottomAttach.gameObject.AddComponent<FuseBehavior>();
			bodyBottomAttach.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bodyBottomAttach.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Body"));

			instantiated[0] = newBody;
			partCreated[0] = true;
			selectionManager.newPartCreated("BodyPrefab(Clone)");

			createDirections(parts[0]);

			enableManipulationButtons(newBody);


		}
	}

	public void createCalf() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = new Vector3(-30, 25, 140); // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,90);
			GameObject newCalf = LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation));

			Transform calfTopAttach = newCalf.transform.FindChild("Calf_Top_Attach");
			Transform calfBottomAttach = newCalf.transform.FindChild("Calf_Bottom_Attach");

			FuseAttributes fuseAtts = calfFuses ();
			calfTopAttach.gameObject.AddComponent<FuseBehavior>();
			calfTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			calfTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Calf"));

			calfBottomAttach.gameObject.AddComponent<FuseBehavior>();
			calfBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			calfBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Calf"));

			instantiated[1] = newCalf;
			partCreated[1] = true;
			selectionManager.newPartCreated("calfPrefab(Clone)");
			createDirections(parts[1]);

			enableManipulationButtons(newCalf);


		}
	}

	public void createTrim() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);
			GameObject newTrim = LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation));	

			Transform topTrim = newTrim.transform.FindChild("Top_Trim");
			Transform topTrimAttach = newTrim.transform.FindChild("Top_Trim_Attach");

			FuseAttributes fuseAtts = trimFuses ();

			topTrimAttach.gameObject.AddComponent<FuseBehavior>();
			topTrimAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			topTrimAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Trim"));

			instantiated[2] = newTrim;	
			partCreated[2] = true;
			selectionManager.newPartCreated("trimPrefab(Clone)");
			createDirections(parts[2]);

			enableManipulationButtons(newTrim);


		}
	}

	public void createToe() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newToe = LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation));

			Transform toeBottomAttach = newToe.transform.FindChild("Toe_Bottom_Attach");
			Transform toeSideAttach = newToe.transform.FindChild("Toe_Side_Attach");

			FuseAttributes fuseAtts = toeFuses ();

			toeBottomAttach.gameObject.AddComponent<FuseBehavior>();
			toeBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			toeBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Toe"));

			toeSideAttach.gameObject.AddComponent<FuseBehavior>();
			toeSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			toeSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Toe"));

			instantiated[3] = newToe;
			partCreated[3] = true;
			selectionManager.newPartCreated("ToePrefab(Clone)");
			createDirections(parts[3]);

			enableManipulationButtons(newToe);


		}
	}

	public void createToeSole() {
		if(!partCreated[6]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();		
			GameObject newToeSole = LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[6], pos, fuseToRotation));

			Transform toeSoleTopAttach = newToeSole.transform.FindChild("Sole_Toe_Top_Attach");
			Transform toeSoleSideAttach = newToeSole.transform.FindChild("Sole_Toe_Side_Attach");

			FuseAttributes fuseAtts = toeSoleFuses ();

			toeSoleTopAttach.gameObject.AddComponent<FuseBehavior>();
			toeSoleTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			toeSoleTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("ToeSole"));


			toeSoleSideAttach.gameObject.AddComponent<FuseBehavior>();
			toeSoleSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			toeSoleSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("ToeSole"));

			instantiated[6] = newToeSole;
			partCreated[6] = true;
			selectionManager.newPartCreated("ToeSolePrefab(Clone)");
			createDirections(parts[6]);

			enableManipulationButtons(newToeSole);


		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
