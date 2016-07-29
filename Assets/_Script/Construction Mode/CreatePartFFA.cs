using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartFFA : MonoBehaviour {
	
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
		startObject = GameObject.Find ("centerBoxWhole");
		GameObject centerHandleBottomAttach = startObject.transform.FindChild("center_box_handle_bottom_attach").gameObject;
		GameObject centerHandleTopAttach = startObject.transform.FindChild("center_box_handle_top_attach").gameObject;
		GameObject centerRingBackAttach = startObject.transform.FindChild("center_box_ring_back_attach").gameObject;
		GameObject centerRingForwardAttach = startObject.transform.FindChild("center_box_ring_forward_attach").gameObject;
		GameObject centerRingLeftAttach = startObject.transform.FindChild("center_box_ring_left_attach").gameObject;
		GameObject centerRingRightAttach = startObject.transform.FindChild("center_box_ring_right_attach").gameObject;

		
		//to avoid errors when selectedObject starts as startObject
		centerHandleBottomAttach.GetComponent<FuseBehavior>().isFused = true;
		centerHandleTopAttach.GetComponent<FuseBehavior>().isFused = true;
		centerRingBackAttach.GetComponent<FuseBehavior>().isFused = true;
		centerRingForwardAttach.GetComponent<FuseBehavior>().isFused = true;
		centerRingLeftAttach.GetComponent<FuseBehavior>().isFused = true;
		centerRingRightAttach.GetComponent<FuseBehavior>().isFused = true;

		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();
	}
	
	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at object from the front)
	
	//returns list of objects ring can fuse to
	public FuseAttributes ringFuses() {
		//fuseLocations for ring: center box ring left
		//						  center box ring right
		//						  center box ring back
		//						  center box ring forward
		//acceptable rotations: 2

		GameObject centerBox = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 centerBoxPos = centerBox.transform.position;
		Vector3 fuseLocation = new Vector3 (centerBoxPos.x, centerBoxPos.y, centerBoxPos.z);
		fuseLocations.Add ("center_box_ring_left_attach", fuseLocation);
		fuseLocations.Add("center_box_ring_right_attach", fuseLocation);
		fuseLocations.Add ("center_box_ring_back_attach", fuseLocation);
		fuseLocations.Add("center_box_ring_forward_attach", fuseLocation);
		fuseRotations.Add ("center_box_ring_left_attach", fuseRotation);
		fuseRotations.Add("center_box_ring_right_attach", fuseRotation);
		fuseRotations.Add ("center_box_ring_back_attach", fuseRotation);
		fuseRotations.Add("center_box_ring_forward_attach", fuseRotation);
		
		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("center_box_ring_left_attach", acceptableRotations);
		fusePositions.Add ("center_box_ring_right_attach", acceptableRotations);
		fusePositions.Add ("center_box_ring_back_attach", acceptableRotations);
		fusePositions.Add ("center_box_ring_forward_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes centerTriFuses() {
		//fuseLocations: 1
		//acceptable rotations: 2
		GameObject centerBox = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 centerBoxPos = centerBox.transform.position;
		Vector3 fuseLocation = new Vector3 (centerBoxPos.x + 18, centerBoxPos.y, centerBoxPos.z);
		fuseLocations.Add ("ring_center_tri_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("ring_center_tri_attach", fuseRotation);
		
		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion acceptableRotation2 = Quaternion.Euler (90,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("ring_center_tri_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes ffaHandleFuses() {
		// fuse locations: 2 
		// acceptable rotations: 1
		GameObject centerBox = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 centerBoxPos = centerBox.transform.position;
		Vector3 fuseLocation = new Vector3 (centerBoxPos.x - 15, centerBoxPos.y, centerBoxPos.z);
		
		fuseLocations.Add("center_box_handle_bottom_attach", fuseLocation);
		fuseLocations.Add("center_box_handle_top_attach", fuseLocation);
		
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("center_box_handle_bottom_attach", fuseRotation);
		fuseRotations.Add ("center_box_handle_top_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		
		fusePositions.Add ("center_box_handle_bottom_attach", acceptableRotations);
		fusePositions.Add ("center_box_handle_top_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes leftTriFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject ring = GameObject.Find("ringPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);
		
		if(ring != null) {
			Vector3 ringPos = ring.transform.position;
			fuseLocation = new Vector3 (ringPos.x, ringPos.y, ringPos.z + 26);
		}
		
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("ring_left_tri_attach", fuseLocation);
		fuseRotations.Add ("ring_left_tri_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (90,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("ring_left_tri_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes rightTriFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject ring = GameObject.Find("ringPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);
		
		if(ring != null) {
			Vector3 ringPos = ring.transform.position;
			fuseLocation = new Vector3(ringPos.x, ringPos.y, ringPos.z - 25);
		}
		
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("ring_right_tri_attach", fuseLocation);
		fuseRotations.Add ("ring_right_tri_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("ring_right_tri_attach", acceptableRotations);

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
	
	
	public void createRing() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);
			GameObject newRing = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform ringCenterBoxLeftAttach = newRing.transform.FindChild("ring_center_box_left_attach");
			Transform ringCenterBoxRightAttach = newRing.transform.FindChild("ring_center_box_right_attach");
			Transform ringCenterBoxBackAttach = newRing.transform.FindChild("ring_center_box_back_attach");
			Transform ringCenterBoxForwardAttach = newRing.transform.FindChild("ring_center_box_forward_attach");

			FuseAttributes fuseAtts = ringFuses ();
			
			ringCenterBoxLeftAttach.gameObject.AddComponent<FuseBehavior>();
			ringCenterBoxLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringCenterBoxLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Ring"));

			ringCenterBoxRightAttach.gameObject.AddComponent<FuseBehavior>();
			ringCenterBoxRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringCenterBoxRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Ring"));
			
			ringCenterBoxBackAttach.gameObject.AddComponent<FuseBehavior>();
			ringCenterBoxBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringCenterBoxBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Ring"));

			ringCenterBoxForwardAttach.gameObject.AddComponent<FuseBehavior>();
			ringCenterBoxForwardAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringCenterBoxForwardAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Ring"));

			instantiated[0] = newRing;
			partCreated[0] = true;
			selectionManager.newPartCreated("ringPrefab(Clone)");
			
			enableManipulationButtons(newRing);
			
			
		}
	}
	
	public void createCenterTri() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,270);
			GameObject newCenterTri = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform centerTriRingAttach = newCenterTri.transform.FindChild("center_tri_ring_attach");
			
			FuseAttributes fuseAtts = centerTriFuses ();
			
			centerTriRingAttach.gameObject.AddComponent<FuseBehavior>();
			centerTriRingAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			centerTriRingAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("CenterTri"));

			instantiated[1] = newCenterTri;
			partCreated[1] = true;
			selectionManager.newPartCreated("center_triPrefab(Clone)");
			
			enableManipulationButtons(newCenterTri);
			
			
		}
	}
	
	public void createLeftTri() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,90,0);
			GameObject newLeftTri = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform leftTriRingAttach = newLeftTri.transform.FindChild("left_tri_ring_attach");
			
			FuseAttributes fuseAtts = leftTriFuses ();
			
			leftTriRingAttach.gameObject.AddComponent<FuseBehavior>();
			leftTriRingAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftTriRingAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftTri"));

			instantiated[2] = newLeftTri;	
			partCreated[2] = true;
			selectionManager.newPartCreated("left_triPrefab(Clone)");
			
			enableManipulationButtons(newLeftTri);
			
			
		}
	}
	
	public void createRightTri() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newRightTri = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform rightTriRingAttach = newRightTri.transform.FindChild("right_tri_ring_attach");

			FuseAttributes fuseAtts = rightTriFuses();
			
			rightTriRingAttach.gameObject.AddComponent<FuseBehavior>();
			rightTriRingAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightTriRingAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightTri"));
		
			instantiated[3] = newRightTri;
			partCreated[3] = true;
			selectionManager.newPartCreated("right_triPrefab(Clone)");
			
			enableManipulationButtons(newRightTri);
			
			
		}
	}
	
	public void createHandle() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);		
			GameObject newHandle = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform handleCenterBoxTopAttach = newHandle.transform.FindChild("handle_center_box_top_attach");
			Transform handleCenterBoxBottomAttach = newHandle.transform.FindChild("handle_center_box_bottom_attach");
			
			FuseAttributes fuseAtts = ffaHandleFuses();
			
			handleCenterBoxTopAttach.gameObject.AddComponent<FuseBehavior>();
			handleCenterBoxTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			handleCenterBoxTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Handle"));
			
			handleCenterBoxBottomAttach.gameObject.AddComponent<FuseBehavior>();
			handleCenterBoxBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			handleCenterBoxBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Handle"));
			
			instantiated[4] = newHandle;
			partCreated[4] = true;
			selectionManager.newPartCreated("ffa_handlePrefab(Clone)");
			
			enableManipulationButtons(newHandle);
			
			
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
