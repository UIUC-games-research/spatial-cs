using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartFFAHarder : MonoBehaviour {

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
		startObject = GameObject.Find ("startObject");
		GameObject centerHandleBottomAttach = startObject.transform.FindChild("center_box_handle_bottom_attach").gameObject;
		GameObject centerHandleTopAttach = startObject.transform.FindChild("center_box_handle_top_attach").gameObject;
		GameObject centerRingLargePartAttach = startObject.transform.FindChild("center_box_ring_large_part_attach").gameObject;
		GameObject centerRingLargePartLeftAttach = startObject.transform.FindChild("center_box_ring_large_part_left_attach").gameObject;
		GameObject centerRingLongPartAttach = startObject.transform.FindChild("center_box_ring_long_part_attach").gameObject;
		GameObject centerRingSmallPartAttach = startObject.transform.FindChild("center_box_ring_small_part_attach").gameObject;


		//to avoid errors when selectedObject starts as startObject
		centerHandleBottomAttach.GetComponent<FuseBehavior>().isFused = true;
		centerHandleTopAttach.GetComponent<FuseBehavior>().isFused = true;
		centerRingLargePartAttach.GetComponent<FuseBehavior>().isFused = true;
		centerRingLargePartLeftAttach.GetComponent<FuseBehavior>().isFused = true;
		centerRingLongPartAttach.GetComponent<FuseBehavior>().isFused = true;
		centerRingSmallPartAttach.GetComponent<FuseBehavior>().isFused = true;

		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();
	}

	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at object from the front)

	//returns list of objects ring can fuse to
	public FuseAttributes ringLargePartFuses() {
		//fuseLocations for ring: center box ring left
		//						  center box ring right
		//						  center box ring back
		//						  center box ring forward
		//acceptable rotations: 2

		GameObject centerBox = startObject;
		GameObject ringLongPart = GameObject.Find("ring_long_partPrefab(Clone)");
		GameObject ringSmallPart = GameObject.Find("ring_small_partPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 centerBoxPos = centerBox.transform.position;
		Vector3 fuseLocation = new Vector3 (centerBoxPos.x, centerBoxPos.y, centerBoxPos.z);
		fuseLocations.Add ("center_box_ring_large_part_attach", fuseLocation);
		fuseLocations.Add("center_box_ring_large_part_left_attach", fuseLocation);
		fuseRotations.Add ("center_box_ring_large_part_attach", fuseRotation);
		fuseRotations.Add("center_box_ring_large_part_left_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("center_box_ring_large_part_attach", acceptableRotations);
		fusePositions.Add ("center_box_ring_large_part_left_attach", acceptableRotations);

		if (ringLongPart != null) {
			fuseLocations.Add ("ring_long_part_ring_large_part_attach", fuseLocation);
			fuseRotations.Add ("ring_long_part_ring_large_part_attach", fuseRotation);
			fusePositions.Add ("ring_long_part_ring_large_part_attach", acceptableRotations);

		}

		if (ringSmallPart != null) {
			fuseLocations.Add("ring_small_part_ring_large_part_attach", fuseLocation);
			fuseRotations.Add("ring_small_part_ring_large_part_attach", fuseRotation);
			fusePositions.Add ("ring_small_part_ring_large_part_attach", acceptableRotations);

		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes ringLongPartFuses() {
		//fuseLocations for ring: center box ring left
		//						  center box ring right
		//						  center box ring back
		//						  center box ring forward
		//acceptable rotations: 2

		GameObject centerBox = startObject;
		GameObject ringLargePart = GameObject.Find("ring_large_partPrefab(Clone)");
		GameObject ringSmallPart = GameObject.Find("ring_small_partPrefab(Clone)");
		GameObject centerTri = GameObject.Find("center_tri_harderPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 centerBoxPos = centerBox.transform.position;
		Vector3 fuseLocation = new Vector3 (centerBoxPos.x + 7.5f, centerBoxPos.y, centerBoxPos.z);
		fuseLocations.Add ("center_box_ring_long_part_attach", fuseLocation);
		fuseRotations.Add ("center_box_ring_long_part_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("center_box_ring_long_part_attach", acceptableRotations);

		if (ringLargePart != null) {
			fuseLocations.Add ("ring_large_part_ring_long_part_attach", fuseLocation);
			fuseRotations.Add ("ring_large_part_ring_long_part_attach", fuseRotation);
			fusePositions.Add ("ring_large_part_ring_long_part_attach", acceptableRotations);

		}

		if (ringSmallPart != null) {
			fuseLocations.Add("ring_small_part_ring_long_part_attach", fuseLocation);
			fuseRotations.Add("ring_small_part_ring_long_part_attach", fuseRotation);
			fusePositions.Add ("ring_small_part_ring_long_part_attach", acceptableRotations);

		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes ringSmallPartFuses() {
		//fuseLocations for ring: center box ring left
		//						  center box ring right
		//						  center box ring back
		//						  center box ring forward
		//acceptable rotations: 2

		GameObject centerBox = startObject;
		GameObject ringLargePart = GameObject.Find("ring_large_partPrefab(Clone)");
		GameObject ringLongPart = GameObject.Find("ring_long_partPrefab(Clone)");

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 centerBoxPos = centerBox.transform.position;
		Vector3 fuseLocation = new Vector3 (centerBoxPos.x, centerBoxPos.y, centerBoxPos.z - 12.5f);
		fuseLocations.Add ("center_box_ring_small_part_attach", fuseLocation);
		fuseRotations.Add ("center_box_ring_small_part_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("center_box_ring_small_part_attach", acceptableRotations);

		if (ringLargePart != null) {
			fuseLocations.Add ("ring_large_part_ring_small_part_attach", fuseLocation);
			fuseRotations.Add ("ring_large_part_ring_small_part_attach", fuseRotation);
			fusePositions.Add ("ring_large_part_ring_small_part_attach", acceptableRotations);

		}

		if (ringLongPart != null) {
			fuseLocations.Add("ring_long_part_ring_small_part_attach", fuseLocation);
			fuseRotations.Add("ring_long_part_ring_small_part_attach", fuseRotation);
			fusePositions.Add ("ring_long_part_ring_small_part_attach", acceptableRotations);

		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes centerTriFuses() {
		//fuseLocations: 1
		//acceptable rotations: 2
		GameObject ringLongPart = GameObject.Find("ring_long_partPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		if (ringLongPart != null) {
			Vector3 ringLongPartPos = ringLongPart.transform.position;
			Vector3 fuseLocation = new Vector3 (ringLongPartPos.x + 7.5f, ringLongPartPos.y, ringLongPartPos.z);
			fuseLocations.Add ("ring_long_part_center_tri_attach", fuseLocation);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
			fuseRotations.Add ("ring_long_part_center_tri_attach", fuseRotation);

			Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
			Quaternion acceptableRotation2 = Quaternion.Euler (90,0,0);
			Quaternion[] acceptableRotations = {acceptableRotation1, acceptableRotation2};
			fusePositions = new Dictionary<string, Quaternion[]>();
			fusePositions.Add ("ring_long_part_center_tri_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes handleTopFuses() {
		// fuse locations: 2 
		// acceptable rotations: 1
		GameObject centerBox = startObject;
		GameObject handleBottom = GameObject.Find ("handle_bottomPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 centerBoxPos = centerBox.transform.position;
		Vector3 fuseLocation = new Vector3 (centerBoxPos.x - 15, centerBoxPos.y + 7.5f, centerBoxPos.z -1);

		fuseLocations.Add("center_box_handle_top_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("center_box_handle_top_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};

		fusePositions.Add ("center_box_handle_top_attach", acceptableRotations);

		if (handleBottom != null) {
			fuseLocations.Add("handle_bottom_handle_top_attach", fuseLocation);
			fuseRotations.Add ("handle_bottom_handle_top_attach", fuseRotation);
			fusePositions.Add ("handle_bottom_handle_top_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes handleBottomFuses() {
		// fuse locations: 2 
		// acceptable rotations: 1
		GameObject centerBox = startObject;
		GameObject handleTop = GameObject.Find ("handle_topPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		Vector3 centerBoxPos = centerBox.transform.position;
		Vector3 fuseLocation = new Vector3 (centerBoxPos.x - 15, centerBoxPos.y - 7.5f, centerBoxPos.z - 1);

		fuseLocations.Add("center_box_handle_bottom_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("center_box_handle_bottom_attach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};

		fusePositions.Add ("center_box_handle_bottom_attach", acceptableRotations);

		if (handleTop != null) {
			fuseLocations.Add ("handle_top_handle_bottom_attach", fuseLocation);
			fuseRotations.Add ("handle_top_handle_bottom_attach", fuseRotation);
			fusePositions.Add ("handle_top_handle_bottom_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}


	public FuseAttributes leftTriFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject ringLargePart = GameObject.Find("ring_large_partPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);

		if(ringLargePart != null) {
			Vector3 ringPos = ringLargePart.transform.position;
			fuseLocation = new Vector3 (ringPos.x - 3.1f, ringPos.y, ringPos.z + 22);
		}

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));

		fuseLocations.Add ("ring_large_part_tri_attach", fuseLocation);
		fuseRotations.Add ("ring_large_part_tri_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (90,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions.Add ("ring_large_part_tri_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes rightTriFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject ringSmallPart = GameObject.Find("ring_small_partPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);

		if(ringSmallPart != null) {
			Vector3 ringPos = ringSmallPart.transform.position;
			fuseLocation = new Vector3(ringPos.x, ringPos.y, ringPos.z - 12.5f);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));

			fuseLocations.Add ("ring_small_part_right_tri_attach", fuseLocation);
			fuseRotations.Add ("ring_small_part_right_tri_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("ring_small_part_right_tri_attach", acceptableRotations);
		}
			
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes rightTriChunkFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject rightTri = GameObject.Find("right_tri_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);

		if(rightTri != null) {
			Vector3 rightTriPos = rightTri.transform.position;
			fuseLocation = new Vector3(rightTriPos.x - 7.3f, rightTriPos.y, rightTriPos.z - 0.2f);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));

			fuseLocations.Add ("right_tri_right_tri_chunk_angle_attach", fuseLocation);
			fuseLocations.Add ("right_tri_right_tri_chunk_back_attach", fuseLocation);
			fuseLocations.Add ("right_tri_right_tri_chunk_side_attach", fuseLocation);
			fuseRotations.Add ("right_tri_right_tri_chunk_angle_attach", fuseRotation);
			fuseRotations.Add ("right_tri_right_tri_chunk_back_attach", fuseRotation);
			fuseRotations.Add ("right_tri_right_tri_chunk_side_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("right_tri_right_tri_chunk_angle_attach", acceptableRotations);
			fusePositions.Add ("right_tri_right_tri_chunk_back_attach", acceptableRotations);
			fusePositions.Add ("right_tri_right_tri_chunk_side_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes scaleneFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject leftTri = GameObject.Find("left_tri_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);

		if(leftTri != null) {
			Vector3 leftTriPos = leftTri.transform.position;
			fuseLocation = new Vector3(leftTriPos.x + 2.4f, leftTriPos.y, leftTriPos.z + 6.5f);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));

			fuseLocations.Add ("left_tri_scalene_back_attach", fuseLocation);
			fuseLocations.Add ("left_tri_scalene_side_attach", fuseLocation);
			fuseRotations.Add ("left_tri_scalene_back_attach", fuseRotation);
			fuseRotations.Add ("left_tri_scalene_side_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (90,0,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("left_tri_scalene_back_attach", acceptableRotations);
			fusePositions.Add ("left_tri_scalene_side_attach", acceptableRotations);
		}

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;

	}

	public FuseAttributes blueTriFuses() {
		//fuse locations: 1
		//acceptable rotations: 1
		GameObject centerTri = GameObject.Find("center_tri_harderPrefab(Clone)");

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);

		if(centerTri != null) {
			Vector3 centerTriPos = centerTri.transform.position;
			fuseLocation = new Vector3(centerTriPos.x + 8.2f, centerTriPos.y, centerTriPos.z + 2.4f);
			Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));

			fuseLocations.Add ("center_tri_blue_tri_back_attach", fuseLocation);
			fuseLocations.Add ("center_tri_blue_tri_side_attach", fuseLocation);
			fuseRotations.Add ("center_tri_blue_tri_back_attach", fuseRotation);
			fuseRotations.Add ("center_tri_blue_tri_side_attach", fuseRotation);
			Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
			Quaternion[] acceptableRotations = {acceptableRotation1};
			fusePositions.Add ("center_tri_blue_tri_back_attach", acceptableRotations);
			fusePositions.Add ("center_tri_blue_tri_side_attach", acceptableRotations);
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


	public void createRingLargePart() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);
			GameObject newRingLargePart = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));

			Transform ringLargePartCenterBoxSideAttach = newRingLargePart.transform.FindChild("ring_large_part_center_box_side_attach");
			Transform ringLargePartRingLongPartAttach = newRingLargePart.transform.FindChild("ring_large_part_ring_long_part_attach");
			Transform ringLargePartCenterBoxBackAttach = newRingLargePart.transform.FindChild("ring_large_part_center_box_back_attach");
			Transform ringLargePartRingSmallPartAttach = newRingLargePart.transform.FindChild("ring_large_part_ring_small_part_attach");
			Transform ringLargePartLeftTriAttach = newRingLargePart.transform.FindChild("ring_large_part_tri_attach");

			FuseAttributes fuseAtts = ringLargePartFuses ();

			ringLargePartCenterBoxSideAttach.gameObject.AddComponent<FuseBehavior>();
			ringLargePartCenterBoxSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLargePartCenterBoxSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLargePart"));

			ringLargePartRingLongPartAttach.gameObject.AddComponent<FuseBehavior>();
			ringLargePartRingLongPartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLargePartRingLongPartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLargePart"));

			ringLargePartCenterBoxBackAttach.gameObject.AddComponent<FuseBehavior>();
			ringLargePartCenterBoxBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLargePartCenterBoxBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLargePart"));

			ringLargePartRingSmallPartAttach.gameObject.AddComponent<FuseBehavior>();
			ringLargePartRingSmallPartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLargePartRingSmallPartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLargePart"));

			ringLargePartLeftTriAttach.gameObject.AddComponent<FuseBehavior>();
			ringLargePartLeftTriAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLargePartLeftTriAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLargePart"));

			instantiated[0] = newRingLargePart;
			partCreated[0] = true;
			selectionManager.newPartCreated("ring_large_partPrefab(Clone)");

			enableManipulationButtons(newRingLargePart);


		}
	}

	public void createRingLongPart() {
		if(!partCreated[1]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);
			GameObject newRingLongPart = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));

			Transform ringLongPartCenterBoxAttach = newRingLongPart.transform.FindChild("ring_long_part_center_box_attach");
			Transform ringLongPartCenterTriAttach = newRingLongPart.transform.FindChild("ring_long_part_center_tri_attach");
			Transform ringLongPartRingLargePartAttach = newRingLongPart.transform.FindChild("ring_long_part_ring_large_part_attach");
			Transform ringLongPartRingSmallPartAttach = newRingLongPart.transform.FindChild("ring_long_part_ring_small_part_attach");

			FuseAttributes fuseAtts = ringLongPartFuses ();

			ringLongPartCenterBoxAttach.gameObject.AddComponent<FuseBehavior>();
			ringLongPartCenterBoxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLongPartCenterBoxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLongPart"));

			ringLongPartCenterTriAttach.gameObject.AddComponent<FuseBehavior>();
			ringLongPartCenterTriAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLongPartCenterTriAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLongPart"));

			ringLongPartRingLargePartAttach.gameObject.AddComponent<FuseBehavior>();
			ringLongPartRingLargePartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLongPartRingLargePartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLongPart"));

			ringLongPartRingSmallPartAttach.gameObject.AddComponent<FuseBehavior>();
			ringLongPartRingSmallPartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringLongPartRingSmallPartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingLongPart"));

			instantiated[1] = newRingLongPart;
			partCreated[1] = true;
			selectionManager.newPartCreated("ring_long_partPrefab(Clone)");

			enableManipulationButtons(newRingLongPart);


		}
	}

	public void createRingSmallPart() {
		if(!partCreated[2]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = Quaternion.Euler (0,180,0);
			GameObject newRingSmallPart = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));

			Transform ringSmallPartCenterBoxAttach = newRingSmallPart.transform.FindChild("ring_small_part_center_box_attach");
			Transform ringSmallPartRightTriAttach = newRingSmallPart.transform.FindChild("ring_small_part_right_tri_attach");
			Transform ringSmallPartRingLargePartAttach = newRingSmallPart.transform.FindChild("ring_small_part_ring_large_part_attach");
			Transform ringSmallPartRingLongPartAttach = newRingSmallPart.transform.FindChild("ring_small_part_ring_long_part_attach");

			FuseAttributes fuseAtts = ringSmallPartFuses ();

			ringSmallPartCenterBoxAttach.gameObject.AddComponent<FuseBehavior>();
			ringSmallPartCenterBoxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringSmallPartCenterBoxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingSmallPart"));

			ringSmallPartRightTriAttach.gameObject.AddComponent<FuseBehavior>();
			ringSmallPartRightTriAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringSmallPartRightTriAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingSmallPart"));

			ringSmallPartRingLargePartAttach.gameObject.AddComponent<FuseBehavior>();
			ringSmallPartRingLargePartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringSmallPartRingLargePartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingSmallPart"));

			ringSmallPartRingLongPartAttach.gameObject.AddComponent<FuseBehavior>();
			ringSmallPartRingLongPartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			ringSmallPartRingLongPartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RingSmallPart"));

			instantiated[2] = newRingSmallPart;
			partCreated[2] = true;
			selectionManager.newPartCreated("ring_small_partPrefab(Clone)");

			enableManipulationButtons(newRingSmallPart);


		}
	}


	public void createCenterTri() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,0,270);
			GameObject newCenterTri = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));

			Transform centerTriRingLongPartAttach = newCenterTri.transform.FindChild("center_tri_ring_long_part_attach");
			Transform centerTriBlueTriBackAttach = newCenterTri.transform.FindChild("center_tri_blue_tri_back_attach");
			Transform centerTriBlueTriSideAttach = newCenterTri.transform.FindChild("center_tri_blue_tri_side_attach");

			FuseAttributes fuseAtts = centerTriFuses ();

			centerTriRingLongPartAttach.gameObject.AddComponent<FuseBehavior>();
			centerTriRingLongPartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			centerTriRingLongPartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("CenterTri"));

			centerTriBlueTriBackAttach.gameObject.AddComponent<FuseBehavior>();
			centerTriBlueTriBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			centerTriBlueTriBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("CenterTri"));

			centerTriBlueTriSideAttach.gameObject.AddComponent<FuseBehavior>();
			centerTriBlueTriSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			centerTriBlueTriSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("CenterTri"));

			instantiated[3] = newCenterTri;
			partCreated[3] = true;
			selectionManager.newPartCreated("center_tri_harderPrefab(Clone)");

			enableManipulationButtons(newCenterTri);


		}
	}

	public void createLeftTri() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (90,90,0);
			GameObject newLeftTri = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));	

			Transform leftTriRingLargePartAttach = newLeftTri.transform.FindChild("left_tri_ring_large_part_attach");
			Transform leftTriScaleneBackAttach = newLeftTri.transform.FindChild("left_tri_scalene_back_attach");
			Transform leftTriScaleneSideAttach = newLeftTri.transform.FindChild("left_tri_scalene_side_attach");

			FuseAttributes fuseAtts = leftTriFuses ();

			leftTriRingLargePartAttach.gameObject.AddComponent<FuseBehavior>();
			leftTriRingLargePartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftTriRingLargePartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftTri"));

			leftTriScaleneBackAttach.gameObject.AddComponent<FuseBehavior>();
			leftTriScaleneBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftTriScaleneBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftTri"));

			leftTriScaleneSideAttach.gameObject.AddComponent<FuseBehavior>();
			leftTriScaleneSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			leftTriScaleneSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LeftTri"));


			instantiated[4] = newLeftTri;	
			partCreated[4] = true;
			selectionManager.newPartCreated("left_tri_harderPrefab(Clone)");

			enableManipulationButtons(newLeftTri);


		}
	}

	public void createRightTri() {
		if(!partCreated[5]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newRightTri = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[5], pos, fuseToRotation)));

			Transform rightTriRingSmallPartAttach = newRightTri.transform.FindChild("right_tri_ring_small_part_attach");
			Transform rightTriRightTriChunkBackAttach = newRightTri.transform.FindChild("right_tri_right_tri_chunk_back_attach");
			Transform rightTriRightTriChunkSideAttach = newRightTri.transform.FindChild("right_tri_right_tri_chunk_side_attach");
			Transform rightTriRightTriChunkAngleAttach = newRightTri.transform.FindChild("right_tri_right_tri_chunk_angle_attach");

			FuseAttributes fuseAtts = rightTriFuses();

			rightTriRingSmallPartAttach.gameObject.AddComponent<FuseBehavior>();
			rightTriRingSmallPartAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightTriRingSmallPartAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightTri"));

			rightTriRightTriChunkBackAttach.gameObject.AddComponent<FuseBehavior>();
			rightTriRightTriChunkBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightTriRightTriChunkBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightTri"));

			rightTriRightTriChunkSideAttach.gameObject.AddComponent<FuseBehavior>();
			rightTriRightTriChunkSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightTriRightTriChunkSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightTri"));

			rightTriRightTriChunkAngleAttach.gameObject.AddComponent<FuseBehavior>();
			rightTriRightTriChunkAngleAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightTriRightTriChunkAngleAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightTri"));

			instantiated[5] = newRightTri;
			partCreated[5] = true;
			selectionManager.newPartCreated("right_tri_harderPrefab(Clone)");

			enableManipulationButtons(newRightTri);


		}
	}

	public void createHandleTop() {
		if(!partCreated[6]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);		
			GameObject newHandleTop = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[6], pos, fuseToRotation)));

			Transform handleTopCenterBoxAttach = newHandleTop.transform.FindChild("handle_top_center_box_attach");
			Transform handleTopHandleBottomAttach = newHandleTop.transform.FindChild("handle_top_handle_bottom_attach");

			FuseAttributes fuseAtts = handleTopFuses();

			handleTopCenterBoxAttach.gameObject.AddComponent<FuseBehavior>();
			handleTopCenterBoxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			handleTopCenterBoxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("HandleTop"));

			handleTopHandleBottomAttach.gameObject.AddComponent<FuseBehavior>();
			handleTopHandleBottomAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			handleTopHandleBottomAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("HandleTop"));

			instantiated[6] = newHandleTop;
			partCreated[6] = true;
			selectionManager.newPartCreated("handle_topPrefab(Clone)");

			enableManipulationButtons(newHandleTop);


		}
	}

	public void createHandleBottom() {
		if(!partCreated[7]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);		
			GameObject newHandleBottom = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[7], pos, fuseToRotation)));

			Transform handleBottomCenterBoxAttach = newHandleBottom.transform.FindChild("handle_bottom_center_box_attach");
			Transform handleBottomHandleTopAttach = newHandleBottom.transform.FindChild("handle_bottom_handle_top_attach");

			FuseAttributes fuseAtts = handleBottomFuses();

			handleBottomCenterBoxAttach.gameObject.AddComponent<FuseBehavior>();
			handleBottomCenterBoxAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			handleBottomCenterBoxAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("HandleBottom"));

			handleBottomHandleTopAttach.gameObject.AddComponent<FuseBehavior>();
			handleBottomHandleTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			handleBottomHandleTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("HandleBottom"));

			instantiated[7] = newHandleBottom;
			partCreated[7] = true;
			selectionManager.newPartCreated("handle_bottomPrefab(Clone)");

			enableManipulationButtons(newHandleBottom);


		}
	}

	public void createBlueTri() {
		if(!partCreated[8]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);		
			GameObject newBlueTri = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[8], pos, fuseToRotation)));

			Transform blueTriCenterTriBackAttach = newBlueTri.transform.FindChild("blue_tri_center_tri_back_attach");
			Transform blueTriCenterTriSideAttach = newBlueTri.transform.FindChild("blue_tri_center_tri_side_attach");

			FuseAttributes fuseAtts = blueTriFuses();

			blueTriCenterTriBackAttach.gameObject.AddComponent<FuseBehavior>();
			blueTriCenterTriBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			blueTriCenterTriBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BlueTri"));

			blueTriCenterTriSideAttach.gameObject.AddComponent<FuseBehavior>();
			blueTriCenterTriSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			blueTriCenterTriSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BlueTri"));

			instantiated[8] = newBlueTri;
			partCreated[8] = true;
			selectionManager.newPartCreated("blue_triPrefab(Clone)");

			enableManipulationButtons(newBlueTri);


		}
	}

	public void createRightTriChunk() {
		if(!partCreated[9]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);		
			GameObject newRightTriChunk = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[9], pos, fuseToRotation)));

			Transform rightTriChunkRightTriBackAttach = newRightTriChunk.transform.FindChild("right_tri_chunk_right_tri_back_attach");
			Transform rightTriChunkRightTriSideAttach = newRightTriChunk.transform.FindChild("right_tri_chunk_right_tri_side_attach");
			Transform rightTriChunkRightTriAngleAttach = newRightTriChunk.transform.FindChild("right_tri_chunk_right_tri_angle_attach");

			FuseAttributes fuseAtts = rightTriChunkFuses();

			rightTriChunkRightTriBackAttach.gameObject.AddComponent<FuseBehavior>();
			rightTriChunkRightTriBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightTriChunkRightTriBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightTriChunk"));

			rightTriChunkRightTriSideAttach.gameObject.AddComponent<FuseBehavior>();
			rightTriChunkRightTriSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightTriChunkRightTriSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightTriChunk"));

			rightTriChunkRightTriAngleAttach.gameObject.AddComponent<FuseBehavior>();
			rightTriChunkRightTriAngleAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			rightTriChunkRightTriAngleAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("RightTriChunk"));

			instantiated[9] = newRightTriChunk;
			partCreated[9] = true;
			selectionManager.newPartCreated("right_tri_chunkPrefab(Clone)");

			enableManipulationButtons(newRightTriChunk);


		}
	}

	public void createScalene() {
		if(!partCreated[10]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler (0,90,0);		
			GameObject newScalene = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[10], pos, fuseToRotation)));

			Transform scaleneLeftTriBackAttach = newScalene.transform.FindChild("scalene_left_tri_back_attach");
			Transform scaleneLeftTriSideAttach = newScalene.transform.FindChild("scalene_left_tri_side_attach");

			FuseAttributes fuseAtts = scaleneFuses();

			scaleneLeftTriBackAttach.gameObject.AddComponent<FuseBehavior>();
			scaleneLeftTriBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			scaleneLeftTriBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Scalene"));

			scaleneLeftTriSideAttach.gameObject.AddComponent<FuseBehavior>();
			scaleneLeftTriSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			scaleneLeftTriSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Scalene"));

			instantiated[10] = newScalene;
			partCreated[10] = true;
			selectionManager.newPartCreated("scalenePrefab(Clone)");

			enableManipulationButtons(newScalene);


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
