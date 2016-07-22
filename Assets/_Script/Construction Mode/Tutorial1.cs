using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour {

	public GameObject eventSystem;
	private SelectPart selectPart;
	public Button[] partButtons;
	public Button connectButton;
	public GameObject finishedImage;
	public Button pyrButton;
	public GameObject rotationGizmo;
	private RotationGizmo rotationScript;
	private Highlighter highlighter;
	public Text errorMessage;
	private GameObject pyramid;
	public GameObject[] attachments;

	private bool[] triggersFinished;
	private const int NUM_TRIGGERS = 11;

	// Use this for initialization
	void Start () {

		triggersFinished = new bool[NUM_TRIGGERS];
		for(int i=0; i<NUM_TRIGGERS; i++) {
			triggersFinished[i] = false;
		}

		rotationScript = rotationGizmo.GetComponent<RotationGizmo>();
		highlighter = new Highlighter();
		selectPart = eventSystem.GetComponent<SelectPart>();

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
			highlighter.Highlight2Sec(finishedImage);
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
		// seventh event: Dresha flashes pyramid again ("Click on this!")
		} else if(!triggersFinished[6] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashPyramid")) {
			triggersFinished[6] = true;
			StartCoroutine(enableInterfaceWait());
		//eighth event: player clicks on pyramid button
		}

		//ninth event: Dresha flashes Pyr's attachment region

		// tenth event: Dresha flashes cube's fuseTo for Pyr

		// eleventh event: player moves camera
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
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaPointedToBlackRegions");
	}

	IEnumerator tryAttachPyrWait() {
		connectButton.interactable = false;
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
}
