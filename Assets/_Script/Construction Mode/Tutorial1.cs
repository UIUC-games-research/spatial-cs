using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour {

	public GameObject eventSystem;
	private SelectPart selectPart;
	private FuseEvent fuseEvent;
	public Button[] partButtons;
	public Button connectButton;
	public GameObject finishedImage;
	public Button pyrButton;
	public GameObject rotationGizmo;
	private RotationGizmo rotationScript;
	public Highlighter highlighter;
	private GameObject pyramid;
	public GameObject[] attachments;
	public Text shapesWrong;
	public Text rotationWrong;
	public Text congrats;
	public Button goToNextTutorial;

	private bool[] triggersFinished;
	private const int NUM_TRIGGERS = 25;
	private bool doneDragging;
	private bool pyrButtonClicked;
	private GameObject selectedObj;

	// Use this for initialization
	void Start () {

		triggersFinished = new bool[NUM_TRIGGERS];
		for(int i=0; i<NUM_TRIGGERS; i++) {
			triggersFinished[i] = false;
		}

		rotationScript = rotationGizmo.GetComponent<RotationGizmo>();
		fuseEvent = eventSystem.GetComponent<FuseEvent>();
		selectPart = eventSystem.GetComponent<SelectPart>();
		doneDragging = false;
		pyrButtonClicked = false;

		//disable part buttons so player can't use them while Dresha talks
		foreach(Button b in partButtons) {
			b.interactable = false;
		}
		//disable clicking on black regions while Dresha talks
		//could throw error if any of the objects in attachments do not have a MeshCollider or BoxCollider
		foreach(GameObject a in attachments) {
			BoxCollider bcollide = a.GetComponent<BoxCollider>();
			if(bcollide == null) {
				a.GetComponent<MeshCollider>().enabled = false;
			} else {
				bcollide.enabled = false;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		// first event: Dresha flashes finished image, which triggers next convo
		if(!triggersFinished[0] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashFinishedImage")) {
			triggersFinished[0] = true;
			highlighter.HighlightTimed(finishedImage, 2);
			ConversationTrigger.AddToken("dreshaFlashedFinishedImage");

		// second event: Dresha creates Pyr, which triggers next convo
		} else if (!triggersFinished[1] && ConversationTrigger.tokens.Contains("dreshaReadyToCreatePyr")) {
			triggersFinished[1] = true;
			StartCoroutine(createPyrWait());

		// third event: Dresha rotates Pyr left, then down
		} else if (!triggersFinished[2] && ConversationTrigger.tokens.Contains("dreshaReadyToRotatePyr")) {
			triggersFinished[2] = true;
			StartCoroutine(rotatePyrWait());

		// fourth event: Dresha highlights the two black regions: Pyr's and the corresponding fuseTo on the cube
		} else if (!triggersFinished[3] && ConversationTrigger.tokens.Contains("dreshaReadyToPointToBlackRegions")) {
			triggersFinished[3] = true;
			StartCoroutine(pointToBlackRegionsWait());

		// fifth event: Dresha's failed connect attempt with error message
		} else if(!triggersFinished[4] && ConversationTrigger.tokens.Contains("dreshaReadyToTryAttaching")) {
			triggersFinished[4] = true;
			StartCoroutine(tryAttachPyrWait());

		// sixth event: Welder interface fizzes to life and all controls are enabled
		} else if(!triggersFinished[5] && ConversationTrigger.tokens.Contains("dreshaReadyToEnableControls")) {
			triggersFinished[5] = true;
			StartCoroutine(enableInterfaceWait());

		} else if(!triggersFinished[6] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashPyrButton")) {
			triggersFinished[6] = true;
			// seventh event: Dresha flashes pyramid button ("Click on this!")
			Highlighter.Highlight(pyrButton.gameObject); // highlight until button is clicked
			ConversationTrigger.AddToken("dreshaFlashedPyrButton");

		} else if(!triggersFinished[7] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashPyr")) {
			triggersFinished[7] = true;
			//player has already clicked Pyr button and has just started talking about the Pyr's black part
			//eighth event: Dresha flashes Pyr
			pyramid = GameObject.Find("pyr");
			Highlighter.Highlight(pyramid); // highlight until is face is selected

		} else if(!triggersFinished[8] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashBox")) {
			triggersFinished[8] = true;
			//ninth event: Dresha flashes Box
			Highlighter.Highlight(GameObject.Find("box")); 
			ConversationTrigger.AddToken("dreshaFlashedBox");

		} else if(!triggersFinished[9] && ConversationTrigger.tokens.Contains("playerReadyToMoveCamera")
			&& Input.GetMouseButtonDown(0)) {
			triggersFinished[9] = true;
			Highlighter.Unhighlight(GameObject.Find("pyr"));
			Highlighter.Unhighlight(GameObject.Find("box"));
			//tenth event: player just depressed mouse key to begin dragging camera
			// figure out later how to get this to work for an actual drag
			doneDragging = true;
			ConversationTrigger.AddToken("playerMovedCamera");

		} else if(!triggersFinished[10] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashBoxAgain")) {
			triggersFinished[10] = true;
			Highlighter.Highlight(GameObject.Find("box")); // highlight until its face is selected

		} else if(triggersFinished[10] && !triggersFinished[11] && selectPart.getSelectedFuseTo() != null) {
			triggersFinished[11] = true;
			// player has selected a FuseTo
			Highlighter.Unhighlight(GameObject.Find("box"));
			ConversationTrigger.AddToken("playerSelectedAFuseTo");

		} else if(!triggersFinished[12] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashPyrAgain")) {
			triggersFinished[12] = true;
			// Dresha has just told player to select the pyr's attachment
			Highlighter.Highlight(GameObject.Find("pyr")); // needs to generalize to any selectedObj

		} else if(!triggersFinished[13] && triggersFinished[12] == true
			&& selectPart.getSelectedObject() != null) {
			triggersFinished[13] = true;
			// player just selected pyr's attachment (or some other selectedObject, if we're allowing different shapes
			connectButton.enabled = false;
			Highlighter.Unhighlight(GameObject.Find("pyr")); 
			ConversationTrigger.AddToken("playerSelectedAnObj");

		} else if(!triggersFinished[14] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashConnectButton")) {
			triggersFinished[14] = true;
			// Dresha will now flash connect button and explain it
			Highlighter.Highlight(connectButton.gameObject);
			ConversationTrigger.AddToken("dreshaFlashedConnectButton");

		} else if(!triggersFinished[15] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashGizmo")) {
			triggersFinished[15] = true;
			// Dresha will now flash rotation gizmo and explain it
			Highlighter.Unhighlight(connectButton.gameObject);
			StartCoroutine(highlightGizmoWait());

		} else if(!triggersFinished[16] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashPyrAfterGizmo")) {
			triggersFinished[16] = true;
			// Dresha will now flash pyr once again
			highlighter.HighlightTimed(GameObject.Find("pyr"), 2); 
			ConversationTrigger.AddToken("dreshaFlashedPyrAfterGizmo");

		} else if(!triggersFinished[17] && ConversationTrigger.tokens.Contains("readyToEnableConnectButton")) {
			triggersFinished[17] = true;
			//enable connect button - Dresha is already talking
			connectButton.enabled = true;

		} else if(!triggersFinished[18] && ConversationTrigger.GetToken("wrongRotationDreshaReadyToFlashObj")) {
			// wrong rotation - Dresha just finished tryRotatingAgain1 and will now flash selectedObj
			// for 2 seconds, followed by the convo tryRotatingAgain2
			StartCoroutine(highlightPyrWrongRotationWait());
			ConversationTrigger.RemoveToken("wrongRotationDreshaReadyToFlashObj");

		} else if(!triggersFinished[18] && ConversationTrigger.GetToken("wrongFaceDreshaReadyToFlashBox")) {
			// wrong shape - Dresha just finished tryDifferentShape1 and will now flash box
			// for 2 seconds, followed by the convo tryDifferentShape2
			StartCoroutine(highlightBoxWait());
			ConversationTrigger.RemoveToken("wrongFaceDreshaReadyToFlashBox");

		} else if(!triggersFinished[18] && ConversationTrigger.GetToken("wrongFaceDreshaReadyToFlashObj")) {
			// wrong shape - Dresha just finished tryDifferentShape2 and will now flash selectedObj
			// for 2 seconds, followed by the convo tryDifferentShape3
			StartCoroutine(highlightPyrWrongFaceWait());
			ConversationTrigger.RemoveToken("wrongFaceDreshaReadyToFlashObj");

		} else if (ConversationTrigger.GetToken("dreshaPracticeToNextLevel")) {
			goToNextTutorial.gameObject.SetActive(true);
		}

		if(congrats.enabled) {
			// player wins!
			// Dresha talks about next level
			// next level should not load until player finishes this convo
			ConversationTrigger.AddToken("playerFinishedTutorial1");

		}

	
	}  

	//disables/enables connect-related conversation tokens
	//to prevent infinite triggers since they aren't oneshots
	//called every time Connect button is invoked once player has control
	public void resetConnectTokens() {
		//BRANCH: if attaching works, go to conversation playerAttachToFinish
			//			if attaching is wrong rotation, go to conversation tryRotatingAgain
			//			if attaching is wrong shape, go to conversation tryDifferentShape
			//	These triggers are NOT oneshots
		if(!triggersFinished[18] && triggersFinished[17]) {
			string fuseStatus = fuseEvent.getFuseStatus();
			if(fuseStatus.Equals("wrongFace")) {
				// player tried to attach but selected the wrong FuseTo
				// Dresha will now tell player to select a different FuseTo
				ConversationTrigger.RemoveToken("playerRotationIncorrect"); // to stop infinite triggers
				ConversationTrigger.RemoveToken("playerAttachedSuccessfully"); // to stop infinite triggers
				ConversationTrigger.AddToken("playerAttachedWrongFace"); // convo tryDifferentShape

				//tryDifferentShape should be disabled as soon as it starts
				//tryDifferentShape should be re-enabled as soon as Connect button is clicked again
			} else if (fuseStatus.Equals("wrongRotation")) {
				// player tried to attach but rotation wasn't right
				// Dresha will now tell player to try a different rotation
				ConversationTrigger.RemoveToken("playerAttachedWrongFace"); // to stop infinite triggers
				ConversationTrigger.RemoveToken("playerAttachedSuccessfully"); // to stop infinite triggers
				ConversationTrigger.AddToken("playerRotationIncorrect"); // convo tryRotatingAgain

			} else if (fuseStatus.Equals("success")){
				triggersFinished[18] = true;
				//player successfully attached part
				//Dresha will now tell player to try rest on their own
				ConversationTrigger.RemoveToken("playerAttachedWrongFace"); // to stop infinite triggers
				ConversationTrigger.RemoveToken("playerRotationIncorrect"); // to stop infinite triggers
				ConversationTrigger.AddToken("playerAttachedSuccessfully"); // to stop infinite triggers
			}
		}
	}

	//does nothing when Pyr button is clicked subsequent times
	public void playerClicksPyrButton() {
		if(!pyrButtonClicked) {
			pyrButtonClicked = true;
			Highlighter.Unhighlight(pyrButton.gameObject);
			ConversationTrigger.AddToken("playerClicksPyrButton");
		}
	}

	IEnumerator createPyrWait() {
		yield return new WaitForSeconds(0.5f);
		eventSystem.GetComponent<CreatePartTutorial1>().createPyr();
		rotationScript.Disable();
		GameObject.Find("pyr_box_attach").GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaCreatedPyr");

	}

	IEnumerator rotatePyrWait() {
		yield return new WaitForSeconds(0.5f);
		pyramid = GameObject.Find("tutorial1_pyrPrefab(Clone)");
		rotationScript.runManualRotation(pyramid, 0f, -90f, 0f);
		rotationScript.runManualRotation(pyramid, 0f, 0f, -90f);
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaRotatedPyr");
	}

	IEnumerator pointToBlackRegionsWait() {
		selectPart.selectObject(GameObject.Find("pyr_box_attach"));
		selectPart.selectFuseTo(GameObject.Find("box_tri_attach"));
		connectButton.interactable = false;
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaPointedToBlackRegions");
	}

	IEnumerator tryAttachPyrWait() {
		yield return new WaitForSeconds(1f);
		connectButton.onClick.Invoke();
		//deselect the active part
		selectPart.resetSelectedObject();
		//deselect tri FuseTo
		selectPart.resetSelectedFuseTo();
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaTriedToAttach");
	}

	IEnumerator enableInterfaceWait() {
		yield return new WaitForSeconds(1f);
		//destroy pyramid and start anew
		eventSystem.GetComponent<CreatePartTutorial1>().clearPartsCreated();
		//enable part buttons
		foreach(Button b in partButtons) {
			b.interactable = true;
		}
		//enable clickability of black regions on all parts
		foreach(GameObject a in attachments) {
			BoxCollider bcollide = a.GetComponent<BoxCollider>();
			if(bcollide == null) {
				a.GetComponent<MeshCollider>().enabled = true;
			} else {
				bcollide.enabled = true;
			}
		}
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaEnabledInterface");
	}

	IEnumerator highlightGizmoWait() {
		// maybe should highlight only the sliders instead?
		foreach(Transform child in rotationGizmo.transform) {
			highlighter.HighlightTimed(child.gameObject, 2); 
		}
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaFlashedGizmo");
	}

	IEnumerator highlightPyrWrongRotationWait() { // needs to generalize to all selectedObj
		highlighter.HighlightTimed(GameObject.Find("pyr"), 2); 
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("wrongRotationDreshaFlashedObj");
		ConversationTrigger.RemoveToken("blockTryRotatingAgain2");

	}

	IEnumerator highlightBoxWait() { 
		highlighter.HighlightTimed(GameObject.Find("box"), 2); 
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("wrongFaceDreshaFlashedBox");
		ConversationTrigger.RemoveToken("wrongFaceDreshaReadyToFlashObj");

	}

	IEnumerator highlightPyrWrongFaceWait() { // needs to generalize to all selectedObj
		highlighter.HighlightTimed(GameObject.Find("pyr"), 2); 
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("wrongFaceDreshaFlashedObj");
		ConversationTrigger.RemoveToken("blockTryDifferentShape3");
	
	}
}
