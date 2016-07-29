using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartKey2 : MonoBehaviour {
	
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
		startObject = GameObject.Find ("postWhole");
		GameObject postHangingLAttach = startObject.transform.FindChild("post_hanging_l_attach").gameObject;
		GameObject postMiddleTAttach = startObject.transform.FindChild("post_middle_t_attach").gameObject;
		GameObject postZigzagAttach = startObject.transform.FindChild("post_zigzag_attach").gameObject;

		//to avoid errors when selectedObject starts as startObject
		postHangingLAttach.GetComponent<FuseBehavior>().isFused = true;
		postMiddleTAttach.GetComponent<FuseBehavior>().isFused = true;
		postZigzagAttach.GetComponent<FuseBehavior>().isFused = true;
		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();
	}
	
	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at object from the front)
	
	//returns list of objects ring can fuse to
	public FuseAttributes cFuses() {
		//fuseLocations for palm: middle t front, middle t bottom, middle t top, ul corner
		//acceptable rotations: 1
		
		GameObject middleT = GameObject.Find ("middle_tPrefab(Clone)");
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(middleT != null) {
			Vector3 middleTPos = middleT.transform.position;
			Vector3 fuseLocation = new Vector3 (middleTPos.x + 4, middleTPos.y, middleTPos.z);
			fuseLocations.Add ("middle_t_c_bottom_attach", fuseLocation);
			fuseLocations.Add ("middle_t_c_top_attach", fuseLocation);
			fuseLocations.Add ("middle_t_c_front_attach", fuseLocation);
			fuseRotations.Add ("middle_t_c_bottom_attach", fuseRotation);
			fuseRotations.Add ("middle_t_c_top_attach", fuseRotation);
			fuseRotations.Add ("middle_t_c_front_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (270,90,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("middle_t_c_bottom_attach", acceptableRotations);
			fusePositions.Add ("middle_t_c_top_attach", acceptableRotations);
			fusePositions.Add ("middle_t_c_front_attach", acceptableRotations);
		}

		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes hangingLFuses() {
		//fuseLocations: 1
		//acceptable rotations: 1
		GameObject post = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 postPos = post.transform.position;
		Vector3 fuseLocation = new Vector3 (postPos.x - 8, postPos.y + 16, postPos.z);
		fuseLocations.Add ("post_hanging_l_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
		fuseRotations.Add ("post_hanging_l_attach", fuseRotation);
		
		Quaternion acceptableRotation1 = Quaternion.Euler (0,90,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("post_hanging_l_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes middleTFuses() {
		// fuse locations: 1 
		// acceptable rotations: 2
		GameObject post = startObject;
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 postPos = post.transform.position;
		Vector3 fuseLocation = new Vector3 (postPos.x + 16, postPos.y, postPos.z);
		fuseLocations.Add("post_middle_t_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
		fuseRotations.Add ("post_middle_t_attach", fuseRotation);
		
		Quaternion acceptableRotation1 = Quaternion.Euler (270,90,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (90,270,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
		
		fusePositions.Add ("post_middle_t_attach", acceptableRotations);

		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes ulCornerFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject c = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if(c != null) {
			Vector3 cPos = c.transform.position;
			Vector3 fuseLocation = new Vector3 (cPos.x + 16, cPos.y - 6, cPos.z);
			
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
			
			fuseLocations.Add ("c_ul_corner_attach", fuseLocation);
			fuseRotations.Add ("c_ul_corner_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (0,0,270);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("c_ul_corner_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes zigzagFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject post = startObject;
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 postPos = post.transform.position;
		Vector3 fuseLocation = new Vector3(postPos.x - 12, postPos.y - 8, postPos.z);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,90,0));
		
		fuseLocations.Add ("post_zigzag_attach", fuseLocation);
		fuseRotations.Add ("post_zigzag_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,90,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("post_zigzag_attach", acceptableRotations);

		
		
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
	
	
	public void createC() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);
			GameObject newC = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform cMiddleTBottomAttach = newC.transform.FindChild("c_middle_t_bottom_attach");
			Transform cMiddleTFrontAttach = newC.transform.FindChild("c_middle_t_front_attach");
			Transform cMiddleTTopAttach = newC.transform.FindChild("c_middle_t_top_attach");
			Transform cUlCornerAttach = newC.transform.FindChild("c_ul_corner_attach");
			
			FuseAttributes fuseAtts = cFuses ();
			
			cMiddleTBottomAttach.gameObject.AddComponent<FuseBehavior>();
			cMiddleTBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			cMiddleTBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("C"));
			
			cMiddleTFrontAttach.gameObject.AddComponent<FuseBehavior>();
			cMiddleTFrontAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			cMiddleTFrontAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("C"));
			
			cMiddleTTopAttach.gameObject.AddComponent<FuseBehavior>();
			cMiddleTTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			cMiddleTTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("C"));
			
			cUlCornerAttach.gameObject.AddComponent<FuseBehavior>();
			cUlCornerAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			cUlCornerAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("C"));
			
			instantiated[0] = newC;
			partCreated[0] = true;
			selectionManager.newPartCreated("cPrefab(Clone)");
			
			enableManipulationButtons(newC);
			
			
		}
	}
	
	public void createHangingL() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,90,0);
			GameObject newHangingL = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform hangingLPostAttach = newHangingL.transform.FindChild("hanging_l_post_attach");
			
			FuseAttributes fuseAtts = hangingLFuses ();
			
			hangingLPostAttach.gameObject.AddComponent<FuseBehavior>();
			hangingLPostAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			hangingLPostAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("HangingL"));
			
			instantiated[1] = newHangingL;
			partCreated[1] = true;
			selectionManager.newPartCreated("hanging_lPrefab(Clone)");
			
			enableManipulationButtons(newHangingL);
			
			
		}
	}
	
	public void createMiddleT() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);
			GameObject newMiddleT = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform middleTCBottomAttach = newMiddleT.transform.FindChild("middle_t_c_bottom_attach");
			Transform middleTCFrontAttach = newMiddleT.transform.FindChild("middle_t_c_front_attach");
			Transform middleTCTopAttach = newMiddleT.transform.FindChild("middle_t_c_top_attach");
			Transform middleTPostAttach = newMiddleT.transform.FindChild("middle_t_post_attach");

			FuseAttributes fuseAtts = middleTFuses ();
			
			middleTCBottomAttach.gameObject.AddComponent<FuseBehavior>();
			middleTCBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			middleTCBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("MiddleT"));

			middleTCFrontAttach.gameObject.AddComponent<FuseBehavior>();
			middleTCFrontAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			middleTCFrontAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("MiddleT"));

			middleTCTopAttach.gameObject.AddComponent<FuseBehavior>();
			middleTCTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			middleTCTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("MiddleT"));

			middleTPostAttach.gameObject.AddComponent<FuseBehavior>();
			middleTPostAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			middleTPostAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("MiddleT"));

			instantiated[2] = newMiddleT;	
			partCreated[2] = true;
			selectionManager.newPartCreated("middle_tPrefab(Clone)");
			
			enableManipulationButtons(newMiddleT);
			
			
		}
	}
	
	public void createUlCorner() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newUlCorner = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform ulCornerCAttach = newUlCorner.transform.FindChild("ul_corner_c_attach");
			
			FuseAttributes fuseAtts = ulCornerFuses();
			
			ulCornerCAttach.gameObject.AddComponent<FuseBehavior>();
			ulCornerCAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ulCornerCAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("UlCorner"));
			
			instantiated[3] = newUlCorner;
			partCreated[3] = true;
			selectionManager.newPartCreated("ul_cornerPrefab(Clone)");
			
			enableManipulationButtons(newUlCorner);
			
			
		}
	}
	
	public void createZigzag() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,180);		
			GameObject newZigzag = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform zigzagPostAttach = newZigzag.transform.FindChild("zigzag_post_attach");
			
			FuseAttributes fuseAtts = zigzagFuses();
			
			zigzagPostAttach.gameObject.AddComponent<FuseBehavior>();
			zigzagPostAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			zigzagPostAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Zigzag"));
			
			instantiated[4] = newZigzag;
			partCreated[4] = true;
			selectionManager.newPartCreated("zigzagPrefab(Clone)");
			
			enableManipulationButtons(newZigzag);
			
			
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
