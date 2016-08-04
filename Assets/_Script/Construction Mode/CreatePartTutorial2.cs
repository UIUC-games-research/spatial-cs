using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartTutorial2 : MonoBehaviour {

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
		startObject = GameObject.Find ("tutorial2_longbox");

		//CHANGE these lines so they refer to each black part on your starting part
		GameObject longboxBigBoxAttach = startObject.transform.FindChild("longbox_bigbox_attach").gameObject;
		GameObject longboxTallboxAttach = startObject.transform.FindChild("longbox_tallbox_attach").gameObject;
		GameObject longboxSmallboxYellowAttach = startObject.transform.FindChild("longbox_smallbox_yellow_attach").gameObject;
		//to avoid errors when selectedObject starts as startObject
		//CHANGE these lines to match above
		//these lines may be unnecessary?
		longboxBigBoxAttach.GetComponent<FuseBehavior>().isFused = true;
		longboxTallboxAttach.GetComponent<FuseBehavior>().isFused = true;
		longboxSmallboxYellowAttach.GetComponent<FuseBehavior>().isFused = true;
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
	public FuseAttributes smallboxYellowFuses() {
		GameObject longbox = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 smallboxYellowPos = longbox.transform.position;
		Vector3 fuseLocation = new Vector3 (smallboxYellowPos.x + 24, 
			smallboxYellowPos.y, smallboxYellowPos.z - 32);
		fuseLocations.Add ("longbox_smallbox_yellow_attach", fuseLocation);
		fuseRotations.Add("longbox_smallbox_yellow_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,180);
		Quaternion acceptableRotation2 = Quaternion.Euler (0,180,0);
		Quaternion acceptableRotation3 = Quaternion.Euler (270,180,0);
		Quaternion acceptableRotation4 = Quaternion.Euler (90,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2, 
			acceptableRotation3, acceptableRotation4};
		fusePositions.Add ("longbox_smallbox_yellow_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes tallboxFuses() {
		GameObject longbox = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 longboxPos = longbox.transform.position;
		Vector3 fuseLocation = new Vector3 (longboxPos.x + 24, longboxPos.y, longboxPos.z);
		fuseLocations.Add ("longbox_tallbox_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		fuseRotations.Add ("longbox_tallbox_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (90,180,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("longbox_tallbox_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes smallboxBlueFuses() {
		GameObject bigbox = GameObject.Find("tutorial2_bigboxPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		if(bigbox != null) {
			Vector3 bigboxPos = bigbox.transform.position;
			Vector3 fuseLocation = new Vector3 (bigboxPos.x + 24, bigboxPos.y, bigboxPos.z);
			fuseLocations.Add("bigbox_smallbox_blue_attach", fuseLocation);

			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
			fuseRotations.Add ("bigbox_smallbox_blue_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (90,180,0);
			Quaternion acceptableRotation3 = Quaternion.Euler (0,180,0);
			Quaternion acceptableRotation4 = Quaternion.Euler (0,0,180);
			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2, 
				acceptableRotation3, acceptableRotation4};

			fusePositions.Add ("bigbox_smallbox_blue_attach", acceptableRotations);

			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		}

		return newAttributes;

	}

	public FuseAttributes bigboxFuses() {
		GameObject longbox = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 headPos = longbox.transform.position;
		Vector3 fuseLocation = new Vector3 (headPos.x + 32, headPos.y, headPos.z + 32);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));

		fuseLocations.Add ("longbox_bigbox_attach", fuseLocation);
		fuseRotations.Add ("longbox_bigbox_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (0,0,180);
		Quaternion acceptableRotation3 = Quaternion.Euler (90,180,0);
		Quaternion acceptableRotation4 = Quaternion.Euler (0,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, 
			acceptableRotation2, acceptableRotation3, acceptableRotation4};
		fusePositions.Add ("longbox_bigbox_attach", acceptableRotations);


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
	public void createSmallboxYellow() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (180,90,180);
			GameObject newSmallboxYellow = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));

			Transform smallboxYellowLongboxAttach = newSmallboxYellow.transform.FindChild("smallbox_yellow_longbox_attach");

			FuseAttributes fuseAtts = smallboxYellowFuses ();

			smallboxYellowLongboxAttach.gameObject.AddComponent<FuseBehavior>();
			smallboxYellowLongboxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			smallboxYellowLongboxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("SmallboxYellow"));

			instantiated[0] = newSmallboxYellow;
			partCreated[0] = true;
			selectionManager.newPartCreated("tutorial2_smallbox_yellowPrefab(Clone)");

			enableManipulationButtons(newSmallboxYellow);


		}
	}

	public void createTallbox() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,90);
			GameObject newTallbox = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));

			Transform tallboxLongboxAttach = newTallbox.transform.FindChild("tallbox_longbox_attach");

			FuseAttributes fuseAtts = tallboxFuses ();

			tallboxLongboxAttach.gameObject.AddComponent<FuseBehavior>();
			tallboxLongboxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			tallboxLongboxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Tallbox"));

			instantiated[1] = newTallbox;
			partCreated[1] = true;
			selectionManager.newPartCreated("tutorial2_tallboxPrefab(Clone)");

			enableManipulationButtons(newTallbox);


		}
	}

	public void createSmallboxBlue() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);
			GameObject newSmallboxBlue = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	

			Transform smallboxBlueBigboxAttach = newSmallboxBlue.transform.FindChild("smallbox_blue_bigbox_attach");

			FuseAttributes fuseAtts = smallboxBlueFuses ();

			smallboxBlueBigboxAttach.gameObject.AddComponent<FuseBehavior>();
			smallboxBlueBigboxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			smallboxBlueBigboxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("SmallboxBlue"));

			instantiated[2] = newSmallboxBlue;	
			partCreated[2] = true;
			selectionManager.newPartCreated("tutorial2_smallbox_bluePrefab(Clone)");

			enableManipulationButtons(newSmallboxBlue);


		}
	}

	public void createBigbox() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,0,0);
			GameObject newBigbox = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));

			Transform bigboxLongboxAttach = newBigbox.transform.FindChild("bigbox_longbox_attach");
			Transform bigboxSmallboxBlueAttach = newBigbox.transform.FindChild("bigbox_smallbox_blue_attach");

			FuseAttributes fuseAtts = bigboxFuses ();

			bigboxLongboxAttach.gameObject.AddComponent<FuseBehavior>();
			bigboxLongboxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bigboxLongboxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Bigbox"));

			bigboxSmallboxBlueAttach.gameObject.AddComponent<FuseBehavior>();
			bigboxSmallboxBlueAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bigboxSmallboxBlueAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Bigbox"));

			instantiated[3] = newBigbox;
			partCreated[3] = true;
			selectionManager.newPartCreated("tutorial2_bigboxPrefab(Clone)");

			enableManipulationButtons(newBigbox);


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
