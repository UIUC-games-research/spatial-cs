using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CreatePartEBG : MonoBehaviour {
	
	private GameObject[] instantiated;
	public GameObject[] parts;
	private bool[] partCreated;
	private Vector3 createLoc;
	public GameObject eventSystem;
	private SelectPart selectionManager;
	public int NUM_PARTS;
	private GameObject startObject;
	private int rotateYInc;
	private int rotateXInc;
	private GameObject lastCreatedPart;

	public GameObject rotateYButton;
	public GameObject rotateXButton;
	public GameObject rotateZButton;
	public RotationGizmo rotateGizmo;

	private bool rotating;
	
	// Use this for initialization
	void Awake () {
		rotating = false;
		rotateYInc = 0;
		rotateXInc = 0;
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
		startObject = GameObject.Find ("base");
		lastCreatedPart = startObject;
		//GameObject baseGripAttach = startObject.transform.FindChild("base_grip_attach").gameObject;
		GameObject baseGripAttach = GameObject.Find("base_grip_attach");
		//to avoid errors when selectedObject starts as startObject
		baseGripAttach.GetComponent<FuseBehavior>().isFused = true;
		rotateGizmo = GameObject.FindGameObjectWithTag("RotationGizmo").GetComponent<RotationGizmo>();
	}

	// y+ = up, y- = down
	// z+ = back, z- = front
	// x+ = right, x- = left
	// (looking at boot from the front)
	
	//returns list of objects body can fuse to
	public FuseAttributes bodyFuses() {
		GameObject grip = GameObject.Find ("gripPrefab(Clone)");
		Vector3 fuseLocation = new Vector3 (0,0,0);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(270,0,0));
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		if(grip != null) {
			Vector3 gripPos = grip.transform.position;
			fuseLocation = new Vector3 (gripPos.x, gripPos.y, gripPos.z - 18f);
			fuseLocations.Add ("grip_body_attach", fuseLocation);
			fuseRotations.Add("grip_body_attach", fuseRotation);
			Quaternion alternateRotation1 = Quaternion.Euler (new Vector3(0,180,0));
			Quaternion alternateRotation2 = Quaternion.Euler (new Vector3(0,180,180));

			Quaternion[] acceptableRotations = {alternateRotation1, alternateRotation2};
			fusePositions.Add ("grip_body_attach", acceptableRotations);
			
			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		}

		return newAttributes;
		
	}
	
	public FuseAttributes gripFuses() {
		Quaternion acceptableRotation1Top = Quaternion.Euler (new Vector3(0,0,0));
		Quaternion acceptableRotation2Top = Quaternion.Euler (new Vector3(0,0,180));
		Quaternion acceptableRotation1Bottom = Quaternion.Euler (new Vector3(0,180,180));
		Quaternion acceptableRotation2Bottom = Quaternion.Euler (new Vector3(0,180,0));
		Dictionary<string, FuseAttributes> fuses = new Dictionary<string, FuseAttributes>();
		GameObject start = GameObject.Find ("base");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();
		Vector3 startPos = start.transform.position;
		Vector3 fuseLocation = new Vector3 (startPos.x, startPos.y, startPos.z - 12f);
		fuseLocations.Add ("base_grip_attach", fuseLocation);
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(270,0,0));
		fuseRotations.Add ("base_grip_attach", fuseRotation);
		Quaternion alternateRotation1 = Quaternion.Euler (new Vector3(0,180,180));
		Quaternion alternateRotation2 = Quaternion.Euler (new Vector3(0,180,240));


		Quaternion[] acceptableRotations = {alternateRotation1};
		fusePositions = new Dictionary<string, Quaternion[]>();
		fusePositions.Add ("base_grip_attach", acceptableRotations);

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		return newAttributes;
		
	}
	
	public FuseAttributes strutFuses() {
		//can fuse to any side of body or to a side of the generator
		GameObject body = GameObject.Find ("ebg_bodyPrefab(Clone)");
		GameObject generator = GameObject.Find ("generatorPrefab(Clone)");
		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		if(body != null) {
			Vector3 bodyPos = body.transform.position;
			Vector3 fuseLocation1 = new Vector3 (bodyPos.x - 10, bodyPos.y + 6, bodyPos.z - 14);
			Vector3 fuseLocation2 = new Vector3 (bodyPos.x, bodyPos.y - 11.5f, bodyPos.z - 14);
			Vector3 fuseLocation3 = new Vector3 (bodyPos.x + 10, bodyPos.y + 6, bodyPos.z - 14);
			fuseLocations.Add("body_strut_left_attach", fuseLocation3);
			fuseLocations.Add("body_strut_right_attach", fuseLocation1);
			fuseLocations.Add("body_strut_top_attach", fuseLocation2);
			fuseLocations.Add ("generator_strut_left_attach", fuseLocation3);
			fuseLocations.Add("generator_strut_right_attach", fuseLocation1);
			fuseLocations.Add("generator_strut_top_attach", fuseLocation2);

			Quaternion fuseRotationTop = Quaternion.Euler (new Vector3(270,0,0));
			Quaternion fuseRotationLeft = Quaternion.Euler (new Vector3(30,270,90));
			Quaternion fuseRotationRight = Quaternion.Euler (new Vector3(30,90,270));
			fuseRotations.Add ("body_strut_top_attach", fuseRotationTop);
			fuseRotations.Add ("body_strut_left_attach", fuseRotationLeft);
			fuseRotations.Add ("body_strut_right_attach", fuseRotationRight);
			fuseRotations.Add ("generator_strut_top_attach", fuseRotationTop);
			fuseRotations.Add ("generator_strut_left_attach", fuseRotationLeft);
			fuseRotations.Add ("generator_strut_right_attach", fuseRotationRight);

			Quaternion rotation1 = Quaternion.Euler (new Vector3(0,180,0));
			Quaternion rotation2 = Quaternion.Euler (new Vector3(0,180,240));
			Quaternion rotation3 = Quaternion.Euler (new Vector3(0,180,120));
			Quaternion[] acceptableRotationsLeft = {rotation3};
			Quaternion[] acceptableRotationsRight = {rotation1};
			Quaternion[] acceptableRotationsTop = {rotation2};
			fusePositions.Add ("body_strut_left_attach", acceptableRotationsLeft);
			fusePositions.Add ("body_strut_right_attach", acceptableRotationsRight);
			fusePositions.Add ("body_strut_top_attach", acceptableRotationsTop);
			fusePositions.Add ("generator_strut_left_attach", acceptableRotationsTop);
			fusePositions.Add ("generator_strut_right_attach", acceptableRotationsLeft);
			fusePositions.Add ("generator_strut_top_attach", acceptableRotationsRight);


			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		}

		//need case where strut is fused to generator
		
		return newAttributes;
		
	}
	
	public FuseAttributes generatorFuses() {
		//can fuse to any strut
		GameObject[] struts = GameObject.FindGameObjectsWithTag ("strut");
		Vector3 fuseLocation = new Vector3 (0,0,0);

		GameObject body = GameObject.Find ("ebg_bodyPrefab(Clone)");
		if(body != null) {
			Vector3 bodyPos = body.transform.position;
			fuseLocation = new Vector3 (bodyPos.x, bodyPos.y, bodyPos.z - 14f);

		}
		Quaternion fuseRotation = Quaternion.Euler (new Vector3(270,0,0));

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

		//for every strut currently fused, have one acceptable rotation for generator
		for(int i = 0; i < struts.Length; i++) {
			Vector3 strutPos = struts[i].transform.position;
			string suffix = struts[i].GetComponent<IsFused>().locationTag;
			if(suffix.Equals ("_left")) {
				fuseLocations.Add ("strut_top_generator_attach_left", fuseLocation);
				fuseRotations.Add ("strut_top_generator_attach_left", fuseRotation);
				//but only if the correct side of the generator is selected
				Quaternion[] acceptableRotationsLeft = {Quaternion.Euler (new Vector3(0,180,240)),
					Quaternion.Euler (new Vector3(0,180,120)), Quaternion.Euler (new Vector3(0,180,0))};
				fusePositions.Add ("strut_top_generator_attach_left", acceptableRotationsLeft);
				print ("Strut " + i + "- Position: " + strutPos + " added LEFT data");
			} else if (suffix.Equals ("_right")) { 
				fuseLocations.Add ("strut_top_generator_attach_right", fuseLocation);
				fuseRotations.Add ("strut_top_generator_attach_right", fuseRotation);
				Quaternion[] acceptableRotationsRight = {Quaternion.Euler (new Vector3(0,180,240)),
					Quaternion.Euler (new Vector3(0,180,120)), Quaternion.Euler (new Vector3(0,180,0))};
				fusePositions.Add ("strut_top_generator_attach_right", acceptableRotationsRight);
				print ("Strut " + i + "- Position: " + strutPos + " added RIGHT data");
				
			} else if (suffix.Equals ("_top")) {
				fuseLocations.Add ("strut_top_generator_attach_top", fuseLocation);
				fuseRotations.Add ("strut_top_generator_attach_top", fuseRotation);
				Quaternion[] acceptableRotationsTop = {Quaternion.Euler (new Vector3(0,180,240)),
					Quaternion.Euler (new Vector3(0,180,120)), Quaternion.Euler (new Vector3(0,180,0))};
				fusePositions.Add ("strut_top_generator_attach_top", acceptableRotationsTop);
				print ("Strut " + i + "- Position: " + strutPos + " added TOP data");
				
			}
		}
		newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);

//		if(strut != null) {
//			//then body must already be placed
//			GameObject body = GameObject.Find ("ebg_bodyPrefab(Clone)");
//			Vector3 bodyPos = body.transform.position;
//			fuseLocation = new Vector3 (bodyPos.x, bodyPos.y + 15, bodyPos.z);
//			fuseLocations.Add ("strut_top_generator_attach", fuseLocation);
//			fuseRotations.Add ("strut_top_generator_attach", fuseRotation);
//			Quaternion alternateRotation1 = Quaternion.Euler (new Vector3(0,120,0));
//			Quaternion alternateRotation2 = Quaternion.Euler (new Vector3(0,240,0));
//			Quaternion[] acceptableRotations = {fuseRotation, alternateRotation1, alternateRotation2};
//
//			fusePositions.Add ("strut_top_body_attach", acceptableRotations);
//			
//			newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
//		}
		
		return newAttributes;
		
	}
	
	public FuseAttributes pointyFuses() {
		//can be fused to any strut
		GameObject[] struts = GameObject.FindGameObjectsWithTag ("strut");
		Quaternion fuseRotationLeft = Quaternion.Euler (new Vector3(30,270,90));
		Quaternion fuseRotationRight = Quaternion.Euler (new Vector3(30,90,270));
		Quaternion fuseRotationTop = Quaternion.Euler (new Vector3(270,0,0));

		Dictionary<string, Vector3> fuseLocations = new Dictionary<string, Vector3>();
		Dictionary<string, Quaternion> fuseRotations = new Dictionary<string, Quaternion>();
		Dictionary<string, Quaternion[]> fusePositions = new Dictionary<string, Quaternion[]>();

		//this part has to be in here or else there is a stupid bug where the struts array is empty yet
		//has length 1
		print ("Struts array length: " + struts.Length);
		for(int i = 0; i < struts.Length; i++) {
			print (i + ": " + struts[i]);
		}

		//for every strut not fused, have one fuseLocation and one fuseRotation and one acceptableRotation
		for(int i = 0; i < struts.Length; i++) {
			if(!struts[i].transform.FindChild ("strut_top_pointy_attach").GetComponent<FuseBehavior>().isFused) {
				Vector3 strutPos = struts[i].transform.position;
				string suffix = struts[i].GetComponent<IsFused>().locationTag;
				if(suffix.Equals ("_left")) {
					fuseLocations.Add ("strut_top_pointy_attach_left", new Vector3 (strutPos.x + 2, strutPos.y + 1, strutPos.z - 6));
					fuseRotations.Add ("strut_top_pointy_attach_left", fuseRotationLeft);
					Quaternion[] acceptableRotationsLeft = {Quaternion.Euler (new Vector3(0,180,120))};
					fusePositions.Add ("strut_top_pointy_attach_left", acceptableRotationsLeft);
				} else if (suffix.Equals ("_right")) { 
					fuseLocations.Add ("strut_top_pointy_attach_right", new Vector3 (strutPos.x - 2, strutPos.y + 1, strutPos.z - 6));
					fuseRotations.Add ("strut_top_pointy_attach_right", fuseRotationRight);
					Quaternion[] acceptableRotationsRight = {Quaternion.Euler (new Vector3(0,180,0))};
					fusePositions.Add ("strut_top_pointy_attach_right", acceptableRotationsRight);

				} else if (suffix.Equals ("_top")) {
					fuseLocations.Add ("strut_top_pointy_attach_top", new Vector3 (strutPos.x, strutPos.y - 2, strutPos.z - 6));
					fuseRotations.Add ("strut_top_pointy_attach_top", fuseRotationTop);
					Quaternion[] acceptableRotationsTop = {Quaternion.Euler (new Vector3(0,180,240))};
					fusePositions.Add ("strut_top_pointy_attach_top", acceptableRotationsTop);

				}
			}

		}


		FuseAttributes newAttributes = new FuseAttributes(fuseLocations, fuseRotations, fusePositions);
		
		return newAttributes;
		
	}

	public void setRotateButtonsInteractableY() {
		//rotateZ and rotateX can only be used when rotateY has come full circle 
		//if and only if rotateY rotates in 120 degree increments
		
		//only called in EBG mode when rotateY is clicked
		rotateYInc++;
		print ("lastCreatedPart: " + lastCreatedPart);
		if (rotateYInc == 3) {
			if(lastCreatedPart.name.Equals ("strut_topPrefab(Clone)") || lastCreatedPart.name.Equals ("pointy_topPrefab(Clone)")) {
				rotateXButton.GetComponent<Button>().interactable = true;
			} else {
				rotateXButton.GetComponent<Button>().interactable = true;
				rotateZButton.GetComponent<Button>().interactable = true;
			}
			rotateYInc = 0;
		} else {
			rotateXButton.GetComponent<Button>().interactable = false;
			rotateZButton.GetComponent<Button>().interactable = false;
		}
	}

	public void setRotateButtonsInteractableX() {
		//rotateZ and rotateY can only be used when rotateX has come full circle 
		//if and only if rotateY rotates in 120 degree increments
		
		//only called in EBG mode when rotateX is clicked
		print ("rotateXInc: " + rotateXInc);
		if(lastCreatedPart.name.Equals ("strut_topPrefab(Clone)") || lastCreatedPart.name.Equals ("pointy_topPrefab(Clone)")) {
			rotateXInc++;
			if (rotateXInc == 4) {
				rotateYButton.GetComponent<Button>().interactable = true;
				rotateXInc = 0;
			} else {
				rotateYButton.GetComponent<Button>().interactable = false;
				rotateZButton.GetComponent<Button>().interactable = false;
			}
		} else {
			rotateXInc = 0;
		}

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

		if(toRotate.name.Equals ("ebg_bodyPrefab(Clone)") || toRotate.name.Equals ("gripPrefab(Clone)")) {
			rotateXButton.transform.GetComponent<Button>().interactable = true;
			rotateXButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
			rotateYButton.transform.GetComponent<Button>().interactable = false;
			rotateZButton.transform.GetComponent<Button>().interactable = true;
			rotateZButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		} else if (toRotate.name.Equals ("strut_topPrefab(Clone)") || toRotate.name.Equals ("pointy_topPrefab(Clone)")) {
			rotateYButton.transform.GetComponent<Button>().interactable = true;
			rotateYButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
			rotateXButton.transform.GetComponent<Button>().interactable = true;
			rotateXButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
			rotateZButton.transform.GetComponent<Button>().interactable = false;
		} else {
			rotateXButton.transform.GetComponent<Button>().interactable = true;
			rotateXButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
			rotateYButton.transform.GetComponent<Button>().interactable = true;
			rotateYButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
			rotateZButton.transform.GetComponent<Button>().interactable = true;
			rotateZButton.transform.GetComponent<RotateButton>().setObjectToRotate(toRotate);
		}


	}
	
	
	public void createBody() {
		if(!partCreated[0]) {
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated		
			Quaternion fuseToRotation = new Quaternion();
			GameObject newBody = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[0], pos, fuseToRotation)));
			lastCreatedPart = newBody;

			Transform bodyGripAttach = newBody.transform.FindChild("body_grip_attach");
			Transform bodyStrutLeftAttach = newBody.transform.FindChild("body_strut_left_attach");
			Transform bodyStrutRightAttach = newBody.transform.FindChild("body_strut_right_attach");
			Transform bodyStrutTopAttach = newBody.transform.FindChild("body_strut_top_attach");

			FuseAttributes fuseAtts = bodyFuses ();

			bodyGripAttach.gameObject.AddComponent<FuseBehavior>();
			bodyGripAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bodyGripAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Body"));

			bodyStrutLeftAttach.gameObject.AddComponent<FuseBehavior>();
			bodyStrutLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bodyStrutLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Body"));

			bodyStrutRightAttach.gameObject.AddComponent<FuseBehavior>();
			bodyStrutRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bodyStrutRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Body"));

			bodyStrutTopAttach.gameObject.AddComponent<FuseBehavior>();
			bodyStrutTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			bodyStrutTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Body"));

			instantiated[0] = newBody;
			partCreated[0] = true;
			selectionManager.newPartCreated("egb_bodyPrefab(Clone)");
			
			createDirections(parts[0]);
			
			enableManipulationButtons(newBody);
			
			
		}
	}
	
	public void createGrip() {
		if(!partCreated[1]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newGrip = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[1], pos, fuseToRotation)));
			lastCreatedPart = newGrip;

			Transform gripBaseAttach = newGrip.transform.FindChild("grip_base_attach");
			Transform gripBodyAttach = newGrip.transform.FindChild("grip_body_attach");
	
			FuseAttributes fuseAtts = gripFuses ();

			gripBaseAttach.gameObject.AddComponent<FuseBehavior>();
			gripBaseAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			gripBaseAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Grip"));

			gripBodyAttach.gameObject.AddComponent<FuseBehavior>();
			gripBodyAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			gripBodyAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Grip"));

			instantiated[1] = newGrip;
			partCreated[1] = true;
			selectionManager.newPartCreated("gripPrefab(Clone)");
			createDirections(parts[1]);
			
			enableManipulationButtons(newGrip);
			
			
		}
	}
	
	public void createStrut() {
		if(!partCreated[2] || (partCreated[2] && instantiated[2].GetComponent<IsFused>().isFused)){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler(270,0,0);
			GameObject newStrut = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate(parts[2], pos, fuseToRotation)));	
			lastCreatedPart = newStrut;

			Transform strutTopBodyAttach = newStrut.transform.FindChild("strut_top_body_attach");
			Transform strutTopGeneratorAttach = newStrut.transform.FindChild("strut_top_generator_attach");
			Transform strutTopPointyAttach = newStrut.transform.FindChild("strut_top_pointy_attach");

			FuseAttributes fuseAtts = strutFuses ();

			strutTopBodyAttach.gameObject.AddComponent<FuseBehavior>();
			strutTopBodyAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			strutTopBodyAttach.gameObject.GetComponent<FuseBehavior>().setTagged(true);
			strutTopBodyAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Strut"));

			strutTopGeneratorAttach.gameObject.AddComponent<FuseBehavior>();
			strutTopGeneratorAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			strutTopGeneratorAttach.gameObject.GetComponent<FuseBehavior>().setTagged(true);
			strutTopGeneratorAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Strut"));

			strutTopPointyAttach.gameObject.AddComponent<FuseBehavior>();
			strutTopPointyAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			strutTopPointyAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Strut"));

			instantiated[2] = newStrut;	
			partCreated[2] = true;
			selectionManager.newPartCreated("strut_topPrefab(Clone)");
			createDirections(parts[2]);
			
			enableManipulationButtons(newStrut);
			
			
		}
	}
	
	public void createGenerator() {
		if(!partCreated[3]){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = new Quaternion();
			GameObject newGen = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[3], pos, fuseToRotation)));
			lastCreatedPart = newGen;

			Transform energy = newGen.transform.FindChild("top_energy");
			Transform generatorStrutLeftAttach = newGen.transform.FindChild("generator_strut_left_attach");
			Transform generatorStrutRightAttach = newGen.transform.FindChild("generator_strut_right_attach");
			Transform generatorStrutTopAttach = newGen.transform.FindChild("generator_strut_top_attach");

			FuseAttributes fuseAtts = generatorFuses ();

			generatorStrutLeftAttach.gameObject.AddComponent<FuseBehavior>();
			generatorStrutLeftAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			generatorStrutLeftAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Generator"));
	
			generatorStrutRightAttach.gameObject.AddComponent<FuseBehavior>();
			generatorStrutRightAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			generatorStrutRightAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Generator"));

			generatorStrutTopAttach.gameObject.AddComponent<FuseBehavior>();
			generatorStrutTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			generatorStrutTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Generator"));

			instantiated[3] = newGen;
			partCreated[3] = true;
			selectionManager.newPartCreated("generatorPrefab(Clone)");
			createDirections(parts[3]);
			
			enableManipulationButtons(newGen);
			
			
		}
	}
	
	public void createPointy() {
		if(!partCreated[4] || (partCreated[4] && instantiated[4].GetComponent<IsFused>().isFused)){
			clearPartsCreated();
			Vector3 pos = createLoc; // this is where the object will appear when it's instantiated
			Quaternion fuseToRotation = Quaternion.Euler(270,0,0);

			GameObject newPointy = rotateGizmo.Enable(LoadUtils.InstantiateParenter((GameObject)Instantiate (parts[4], pos, fuseToRotation)));
			lastCreatedPart = newPointy;

			Transform pointyStrutTopAttach = newPointy.transform.FindChild("pointy_strut_top_attach");
			Transform dec1 = newPointy.transform.FindChild ("pointy_top_dec1");
			Transform dec2 = newPointy.transform.FindChild ("pointy_top_dec2");

			FuseAttributes fuseAtts = pointyFuses ();

			pointyStrutTopAttach.gameObject.AddComponent<FuseBehavior>();
			pointyStrutTopAttach.gameObject.GetComponent<FuseBehavior>().setFuseTo(fuseAtts);
			pointyStrutTopAttach.gameObject.GetComponent<FuseBehavior>().setButtonTo(GameObject.Find ("Pointy"));
			
			instantiated[4] = newPointy;
			partCreated[4] = true;
			selectionManager.newPartCreated("pointy_topPrefab(Clone)");
			createDirections(parts[4]);
			
			enableManipulationButtons(newPointy);
			
			
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
