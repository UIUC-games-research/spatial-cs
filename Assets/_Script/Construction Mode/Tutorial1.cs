using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour {

	public GameObject eventSystem;
	public GameObject finishedImage;
	public Button pyrButton;
	public GameObject rotationGizmo;
	private RotationGizmo rotationScript;
	private Highlighter highlighter;
	public Text errorMessage;
	public GameObject[] interfaceElements;
	private GameObject pyramid;
	private GameObject pyramidBoxAttach;
	private GameObject boxPyramidAttach;

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
			eventSystem.GetComponent<CreatePartTutorial1>().createPyr();
			//disable RotationGizmo
			rotationScript.Disable();

			triggersFinished[1] = true;
			StartCoroutine(wait2());
			ConversationTrigger.AddToken("dreshaCreatedPyr");
		// third event: Dresha rotates Pyr left, then down
		} else if (!triggersFinished[2] && ConversationTrigger.tokens.Contains("dreshaReadyToRotatePyr")) {
			pyramid = GameObject.Find("tutorial1_pyrPrefab(Clone)");
			rotationScript.runManualRotation(pyramid, 0f, -90f, 0f);
			rotationScript.runManualRotation(pyramid, 0f, 0f, -90f);
			triggersFinished[2] = true;
			StartCoroutine(wait2());
			ConversationTrigger.AddToken("dreshaRotatedPyr");
		}
		// fourth event: Dresha highlights the two black regions: Pyr's and the corresponding fuseTo on the cube

		// fifth event: Dresha's failed connect attempt with error message

		// sixth event: Welder interface fizzes to life and all controls are enabled

		// seventh event: Dresha flashes pyramid again ("Click on this!")

		//eighth event: player clicks on pyramid

		//ninth event: Dresha flashes Pyr's attachment region

		// tenth event: Dresha flashes cube's fuseTo for Pyr

		// eleventh event: player moves camera
	}  

	IEnumerator wait2() {
		yield return new WaitForSeconds(2);
	}
}
