using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartKey3 : MonoBehaviour {
	
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
		startObject = GameObject.Find ("block_juts_whole");
		
		//CHANGE these lines so they refer to each black part on your starting part
		//GameObject blockJutsCornerAttach = startObject.transform.FindChild("block_juts_corner_attach").gameObject;
		//GameObject blockJutsLongLBackAttach = startObject.transform.FindChild("block_juts_long_l_back_attach").gameObject;
		GameObject blockJutsLongLSideAttach = startObject.transform.FindChild("block_juts_long_l_side_attach").gameObject;
		GameObject blockJutsLongLTopAttach = startObject.transform.FindChild("block_juts_long_l_top_attach").gameObject;



		//to avoid errors when selectedObject starts as startObject
		//CHANGE these lines to match above
		//blockJutsCornerAttach.GetComponent<FuseBehavior>().isFused = true;
		//blockJutsLongLBackAttach.GetComponent<FuseBehavior>().isFused = true;
		blockJutsLongLSideAttach.GetComponent<FuseBehavior>().isFused = true;
		blockJutsLongLTopAttach.GetComponent<FuseBehavior>().isFused = true;
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
	public FuseAttributes longLFuses() {
		GameObject key3 = startObject;
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 key3Pos = key3.transform.position;
		Vector3 fuseLocation = new Vector3 (key3Pos.x+15	, key3Pos.y , key3Pos.z-10);
		fuseLocations.Add ("block_juts_long_l_back_attach", fuseLocation);
		fuseLocations.Add ("block_juts_long_l_side_attach", fuseLocation);
		fuseLocations.Add ("block_juts_long_l_top_attach", fuseLocation);
		fuseLocations.Add ("corner_long_l_attach", fuseLocation);



		fuseRotations.Add("block_juts_long_l_back_attach", fuseRotation);
		fuseRotations.Add("block_juts_long_l_side_attach", fuseRotation);
		fuseRotations.Add("block_juts_long_l_top_attach", fuseRotation);
		fuseRotations.Add("corner_long_l_attach", fuseRotation);


		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};


		Quaternion acceptableRotation2 = Quaternion.Euler (0,90,90);
		Quaternion[] acceptableRotations2 = {acceptableRotation2};


		fusePositions.Add ("block_juts_long_l_back_attach", acceptableRotations);
		fusePositions.Add ("block_juts_long_l_side_attach", acceptableRotations);
		fusePositions.Add ("block_juts_long_l_top_attach", acceptableRotations);
		fusePositions.Add ("corner_long_l_attach", acceptableRotations2);


		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;
		
	}
	
	public FuseAttributes connectorFuses() {
		GameObject key3 = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		
		Vector3 key3Pos = key3.transform.position;
		Vector3 fuseLocation = new Vector3 (key3Pos.x + 5, key3Pos.y - 5, key3Pos.z - 40);
		fuseLocations.Add ("corner_connector_attach", fuseLocation);
		fuseLocations.Add ("diagonal_connector_side_attach", fuseLocation);
		fuseLocations.Add ("diagonal_connector_top_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		fuseRotations.Add ("corner_connector_attach", fuseRotation);
		fuseRotations.Add ("diagonal_connector_side_attach", fuseRotation);
		fuseRotations.Add ("diagonal_connector_top_attach", fuseRotation);
		
		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("corner_connector_attach", acceptableRotations);
		fusePositions.Add ("diagonal_connector_side_attach", acceptableRotations);
		fusePositions.Add ("diagonal_connector_top_attach", acceptableRotations);
		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes bigCornerFuses() {
		//GameObject back = GameObject.Find ("backPrefab(Clone)");
		//GameObject bridgeCover = GameObject.Find ("bridge_coverPrefab(Clone)");

		GameObject key3 = startObject;
		Vector3 fuseLocation = new Vector3(0,0,0);
		
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		

		Vector3 key3Pos = key3.transform.position;
		fuseLocation = new Vector3 (key3Pos.x + 15, key3Pos.y + 25, key3Pos.z - 25);

		
		fuseLocations.Add("long_l_big_corner_attach", fuseLocation);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,180,90));
		fuseRotations.Add ("long_l_big_corner_attach", fuseRotation);




		Quaternion acceptableRotation1 = Quaternion.Euler (270,0,0);
		Quaternion[] acceptableRotations = {acceptableRotation1};
		
		fusePositions.Add ("long_l_big_corner_attach", acceptableRotations);

		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes cornerFuses() {
		GameObject key3 = startObject;
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);
		
		Vector3 key3Pos = key3.transform.position;
		fuseLocation = new Vector3 (key3Pos.x , key3Pos.y - 5, key3Pos.z - 20);

		
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("long_l_corner_attach", fuseLocation);
		fuseLocations.Add ("block_juts_corner_attach", fuseLocation);
		fuseLocations.Add ("connector_corner_Atach", fuseLocation);



		fuseRotations.Add ("long_l_corner_attach", fuseRotation);
		fuseRotations.Add ("block_juts_corner_attach", fuseRotation);
		fuseRotations.Add ("connector_corner_Atach", fuseRotation);

		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,90);
		Quaternion[] acceptableRotations = {acceptableRotation1};

		Quaternion acceptableRotation2 = Quaternion.Euler (0,90,90);
		Quaternion[] acceptableRotations2 = {acceptableRotation2};


		fusePositions.Add ("long_l_corner_attach", acceptableRotations2);
		fusePositions.Add ("block_juts_corner_attach", acceptableRotations);
		fusePositions.Add ("connector_corner_Atach", acceptableRotations);

		
		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}
	
	public FuseAttributes diagonalFuses() {
		GameObject key3 = startObject;

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 fuseLocation = new Vector3(0,0,0);
		
		Vector3 key3Pos = key3.transform.position;
		fuseLocation = new Vector3 (key3Pos.x+10 , key3Pos.y + 5, key3Pos.z - 45);

		Quaternion fuseRotation = Quaternion.Euler (new Vector3(0,0,0));
		
		fuseLocations.Add ("connector_diagonal_side_attach", fuseLocation);
		fuseLocations.Add ("connector_diagonal_top_attach", fuseLocation);
		fuseRotations.Add ("connector_diagonal_side_attach", fuseRotation);
		fuseRotations.Add ("connector_diagonal_top_attach", fuseRotation);
		Quaternion acceptableRotation1 = Quaternion.Euler (0,0,0);

		Quaternion[] acceptableRotations = {acceptableRotation1};


		fusePositions.Add ("connector_diagonal_side_attach", acceptableRotations);
		fusePositions.Add ("connector_diagonal_top_attach", acceptableRotations);
		
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
	
	public void createLongL() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = new Quaternion();
			GameObject newLongL = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			
			Transform longLBigCornerAttach = newLongL.transform.FindChild("long_l_big_corner_attach");
			Transform longLCornerAttach = newLongL.transform.FindChild("long_l_corner_attach");
			Transform longLBlockJutsSideAttach = newLongL.transform.FindChild("long_l_block_juts_side_attach");
			Transform longLBlockJutsBackAttach = newLongL.transform.FindChild("long_l_block_juts_back_attach");
			Transform longLBlockJutsTopAttach = newLongL.transform.FindChild("long_l_block_juts_top_attach");




			FuseAttributes fuseAtts = longLFuses ();
			
			longLBigCornerAttach.gameObject.AddComponent<FuseBehavior>();
			longLBigCornerAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			longLBigCornerAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LongL"));
			
			longLCornerAttach.gameObject.AddComponent<FuseBehavior>();
			longLCornerAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			longLCornerAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LongL"));

			longLBlockJutsSideAttach.gameObject.AddComponent<FuseBehavior>();
			longLBlockJutsSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			longLBlockJutsSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LongL"));

			longLBlockJutsBackAttach.gameObject.AddComponent<FuseBehavior>();
			longLBlockJutsBackAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			longLBlockJutsBackAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LongL"));

			longLBlockJutsTopAttach.gameObject.AddComponent<FuseBehavior>();
			longLBlockJutsTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			longLBlockJutsTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("LongL"));


			instantiated[0] = newLongL;
			partCreated[0] = true;
			selectionManager.newPartCreated("long_lPrefab(Clone)");
			
			enableManipulationButtons(newLongL);
			
			
		}
	}
	
	public void createConnector() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newConnector = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			
			Transform backLeftCoverAttach = newConnector.transform.FindChild("connector_corner_attach");
			Transform backRightCoverAttach = newConnector.transform.FindChild("connector_diagonal_side_attach");
			Transform backBridgeAttach = newConnector.transform.FindChild("connector_diagonal_top_attach");
			
			FuseAttributes fuseAtts = connectorFuses ();
			
			backLeftCoverAttach.gameObject.AddComponent<FuseBehavior>();
			backLeftCoverAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backLeftCoverAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Connector"));
			
			backRightCoverAttach.gameObject.AddComponent<FuseBehavior>();
			backRightCoverAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backRightCoverAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Connector"));
			
			backBridgeAttach.gameObject.AddComponent<FuseBehavior>();
			backBridgeAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			backBridgeAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Connector"));
			
			instantiated[1] = newConnector;
			partCreated[1] = true;
			selectionManager.newPartCreated("connectorPrefab(Clone)");
			
			enableManipulationButtons(newConnector);
			
			
		}
	}
	
	public void createBigCorner() {
		if(!partCreated[2]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newBigCorner = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[2], pos, fuseToRotation)));	
			
			Transform bigCornerLongLAttach = newBigCorner.transform.FindChild("big_corner_long_l_attach");

			//fixes off center rotation problem
			//strutTopBodyAttach.transform.localPosition = new Vector3(0, 0, 0);
			//strutTopGeneratorAttach.transform.localPosition = new Vector3(0.08f, 0, -0.72f);
			//strutTopPointyAttach.transform.localPosition = new Vector3(0, 0, 0);
			
			FuseAttributes fuseAtts = bigCornerFuses ();
			
			bigCornerLongLAttach.gameObject.AddComponent<FuseBehavior>();
			bigCornerLongLAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bigCornerLongLAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("BigCorner"));
			

			instantiated[2] = newBigCorner;	
			partCreated[2] = true;
			selectionManager.newPartCreated("big_cornerPrefab(Clone)");
			
			enableManipulationButtons(newBigCorner);
			
			
		}
	}
	
	public void createCorner() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newCorner = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			
			Transform cornerLongLAttach = newCorner.transform.FindChild("corner_long_l_attach");
			Transform cornerblockJutsAttach = newCorner.transform.FindChild("corner_block_juts_attach");
			Transform cornerConnectorAttach = newCorner.transform.FindChild("corner_connector_attach");



			FuseAttributes fuseAtts = cornerFuses();
			
			cornerLongLAttach.gameObject.AddComponent<FuseBehavior>();
			cornerLongLAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			cornerLongLAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Corner"));
			
			cornerblockJutsAttach.gameObject.AddComponent<FuseBehavior>();
			cornerblockJutsAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			cornerblockJutsAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Corner"));

			cornerConnectorAttach.gameObject.AddComponent<FuseBehavior>();
			cornerConnectorAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			cornerConnectorAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Corner"));


			instantiated[3] = newCorner;
			partCreated[3] = true;
			selectionManager.newPartCreated("cornerPrefab(Clone)");
			
			enableManipulationButtons(newCorner);
			
			
		}
	}
	
	public void createDiagonal() {
		if(!partCreated[4]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();		
			GameObject newDiagonal = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			
			Transform diagonalConnectorSideAttach = newDiagonal.transform.FindChild("diagonal_connector_side_attach");
			Transform diagonalConnectorTopAttach = newDiagonal.transform.FindChild("diagonal_connector_top_attach");
			
			FuseAttributes fuseAtts = diagonalFuses();
			
			diagonalConnectorSideAttach.gameObject.AddComponent<FuseBehavior>();
			diagonalConnectorSideAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			diagonalConnectorSideAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Diagonal"));
			
			diagonalConnectorTopAttach.gameObject.AddComponent<FuseBehavior>();
			diagonalConnectorTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			diagonalConnectorTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Diagonal"));
			
			instantiated[4] = newDiagonal;
			partCreated[4] = true;
			selectionManager.newPartCreated("diagonalPrefab(Clone)");
			
			enableManipulationButtons(newDiagonal);
			
			
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
