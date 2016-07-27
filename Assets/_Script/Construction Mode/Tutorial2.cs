using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour {
	public GameObject finishedImage;
	public Button[] partButtons;

	private bool[] triggersFinished;
	private const int NUM_TRIGGERS = 2;
	public Highlighter highlighter;

	public Text congrats;
	public Button goToRocketBootsLevel;
	public GameObject eventSystem;
	private SelectPart selectPart;
	private FuseEvent fuseEvent;
	private GameObject selectedObj;

	// Use this for initialization
	void Start () {
		triggersFinished = new bool[NUM_TRIGGERS];
		for(int i=0; i<NUM_TRIGGERS; i++) {
			triggersFinished[i] = false;
		}

		selectPart = eventSystem.GetComponent<SelectPart>();
		fuseEvent = eventSystem.GetComponent<FuseEvent>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!triggersFinished[0] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashFinishedImage2")) {
			triggersFinished[0] = true;
			// first event: Dresha flashes finished image, which triggers next convo
			highlighter.HighlightTimed(finishedImage, 2);
			ConversationTrigger.AddToken("dreshaFlashedFinishedImage2");
		} else if(!triggersFinished[1] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashPartButtons")) {
			triggersFinished[1] = true;
			// second event: Dresha flashes part buttons
			StartCoroutine(highlightPartButtonsWait());
		} else if(ConversationTrigger.GetToken("wrongRotationDreshaReadyToFlashObj")){
			// Dresha flashes selected obj and then lets the player try again
			highlightSelectedObj(1f);
			ConversationTrigger.AddToken("dreshaFlashedSelectedObj2");	

		} else if(ConversationTrigger.GetToken("playerAttachedWrongFace") && !ConversationTrigger.GetToken("wrongFaceDreshaReadyToFlashBox")) {
			// wrong shape - Dresha just finished tryDifferentShape1 and 
			// will now flash box and part menu for 2 seconds
			StartCoroutine(highlightPartButtonsWait());
			StartCoroutine(waitAndHighlightBox());

		} else if (ConversationTrigger.GetToken("showNextLevelButton2")) {
			goToRocketBootsLevel.gameObject.SetActive(true);
		}

		if(congrats.enabled) {
			// player wins!
			// Dresha talks about next level
			// next level should not load until player finishes this convo
			ConversationTrigger.AddToken("showNextLevelButton2");

		}

	

	}

	public void resetConnectTokens() {
		//BRANCH: if attaching works, go to conversation playerAttachToFinish
		//			if attaching is wrong rotation, go to conversation tryRotatingAgain
		//			if attaching is wrong shape, go to conversation tryDifferentShape
		//	These triggers are NOT oneshots
		string fuseStatus = fuseEvent.getFuseStatus();
		ConversationTrigger.RemoveToken("playerRotationIncorrect"); // to stop infinite triggers
		ConversationTrigger.RemoveToken("wrongRotationDreshaReadyToFlashObj"); // to stop infinite triggers
		ConversationTrigger.RemoveToken("dreshaFlashedSelectedObj2"); // to stop infinite triggers
		ConversationTrigger.RemoveToken("blockTryRotatingAgain2"); // to stop infinite triggers
		ConversationTrigger.RemoveToken("playerAttachedWrongFace"); // to stop infinite triggers
		ConversationTrigger.RemoveToken("wrongFaceDreshaReadyToFlashBox"); // to stop infinite triggers

		if(fuseStatus.Equals("wrongFace")) {
			// player tried to attach but selected the wrong FuseTo
			// Dresha will now tell player to select a different FuseTo
			ConversationTrigger.AddToken("playerAttachedWrongFace"); // convo tryDifferentShape

		} else if (fuseStatus.Equals("wrongRotation")) {
			// player tried to attach but rotation wasn't right
			// Dresha will now tell player to try a different rotation
			ConversationTrigger.AddToken("playerRotationIncorrect"); // convo tryRotatingAgain
		} 
	}

	private void highlightSelectedObj(float sec) { // generalizes to any selectedObj
		GameObject selectedObj = selectPart.getSelectedObject();
		if(selectedObj.name.Equals("tutorial2_bigboxPrefab(Clone)")) {
			highlighter.HighlightTimed(GameObject.Find("bigbox_close"), sec); 
			highlighter.HighlightTimed(GameObject.Find("bigbox_far"), sec); 
		} else if(selectedObj.name.Equals("tutorial2_smallbox_bluePrefab(Clone")) {
			highlighter.HighlightTimed(GameObject.Find("smallbox_blue"), sec); 
		} else if(selectedObj.name.Equals("tutorial2_smallbox_yellowPrefab(Clone")) {
			highlighter.HighlightTimed(GameObject.Find("smallbox_yellow"), sec); 
		} else {
			highlighter.HighlightTimed(GameObject.Find("tallbox"), sec); 
		}
	}

	IEnumerator highlightPartButtonsWait() {
		foreach (Button b in partButtons) {
			highlighter.HighlightTimed(b.gameObject, 1);
		}
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaFlashedPartButtons");
		foreach (Button b in partButtons) {
			b.enabled = true;
		}
	}

	IEnumerator waitAndHighlightBox() {
		yield return new WaitForSeconds(1f);
		highlighter.HighlightTimed(GameObject.Find("longbox"), 1);
	}

		
}
