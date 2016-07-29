using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartTutorial1 : MonoBehaviour {

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
		startObject = GameObject.Find ("tutorial1_box");
		GameObject boxPyrAttach = startObject.transform.FindChild("box_pyr_attach").gameObject;
		GameObject boxTriAttach = startObject.transform.FindChild("box_tri_attach").gameObject;
		GameObject boxConeAttach = startObject.transform.FindChild("box_cone_attach").gameObject;
		//to avoid errors when selectedObject starts as startObject
		boxPyrAttach.GetComponent<FuseBehavior>().isFused = true;
		boxTriAttach.GetComponent<FuseBehavior>().isFused = true;
		boxConeAttach.GetComponent<FuseBehavior>().isFused = true;

		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();

	}

	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at boot from the front)

	//returns list of objects body can fuse to
	public FuseAttributes pyrFuses() {
		GameObject box = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 boxPos = box.transform.position;
		Vector3 fuseLocation = new Vector3 (boxPos.x, boxPos.y + 28, boxPos.z);
		fuseLocations.Add ("box_pyr_attach", fuseLocation);
		fuseRotations.Add("box_pyr_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (270,90,0);
		Quaternion acceptableRotation3 = Quaternion.Euler (270,0,0);
		Quaternion acceptableRotation4 = Quaternion.Euler (270,270,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2, 
			acceptableRotation3, acceptableRotation4};
		fusePositions.Add ("box_pyr_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes triFuses() {
		GameObject box = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();


		Vector3 boxPos = box.transform.position;
		Vector3 fuseLocation = new Vector3 (boxPos.x, boxPos.y, boxPos.z - 27);
		fuseLocations.Add ("box_tri_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,0));
		fuseRotations.Add ("box_tri_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (270,180,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("box_tri_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes coneFuses() {
		GameObject box = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		Vector3 boxPos = box.transform.position;
		Vector3 fuseLocation = new Vector3 (boxPos.x, boxPos.y - 30.5f, boxPos.z);
		fuseLocations.Add("box_cone_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("box_cone_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (90,0,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (90,180,0);
		Quaternion acceptableRotation3 = Quaternion.Euler (90,90,0);
		Quaternion acceptableRotation4 = Quaternion.Euler (90,270,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2, acceptableRotation3,acceptableRotation4};

		fusePositions.Add ("box_cone_attach", acceptableRotations);

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
		rotateYButton.transform.GetComponent<Button>().interactable = true;
		rotateXButton.transform.GetComponent<Button>().interactable = true;
		rotateZButton.transform.GetComponent<Button>().interactable = true;

		rotateYButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		rotateXButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		rotateZButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
	}


	public void createPyr() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (180,0,90);
			GameObject newPyr = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));

			Transform pyrBoxAttach = newPyr.transform.FindChild("pyr_box_attach");

			FuseAttributes fuseAtts = pyrFuses ();

			pyrBoxAttach.gameObject.AddComponent<FuseBehavior>();
			pyrBoxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			pyrBoxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Pyr"));

			instantiated[0] = newPyr;
			partCreated[0] = true;
			selectionManager.newPartCreated("tutorial1_pyrPrefab(Clone)");

			enableManipulationButtons(newPyr);


		}
	}

	public void createTri() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newTri = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));

			Transform triBoxAttach = newTri.transform.FindChild("tri_box_attach");

			FuseAttributes fuseAtts = triFuses ();

			triBoxAttach.gameObject.AddComponent<FuseBehavior>();
			triBoxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			triBoxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Tri"));

			instantiated[1] = newTri;
			partCreated[1] = true;
			selectionManager.newPartCreated("tutorial1_triPrefab(Clone)");

			enableManipulationButtons(newTri);


		}
	}

	public void createCone() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,90);
			GameObject newCone = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	

			Transform coneBoxAttach = newCone.transform.FindChild("cone_box_attach");

			FuseAttributes fuseAtts = coneFuses ();

			coneBoxAttach.gameObject.AddComponent<FuseBehavior>();
			coneBoxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			coneBoxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Cone"));


			instantiated[2] = newCone;	
			partCreated[2] = true;
			selectionManager.newPartCreated("tutorial1_conePrefab(Clone)");

			enableManipulationButtons(newCone);


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
