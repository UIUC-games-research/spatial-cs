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
		} else if(ConversationTrigger.GetToken("playerRotationIncorrect") && !ConversationTrigger.GetToken("wrongRotationDreshaReadyToFlashObj")) {
			// wrong rotation - Dresha just finished tryRotatingAgain1 and will now flash selectedObj
			// for 2 seconds, followed by the convo tryRotatingAgain2
			GameObject selectedObj = selectPart.getSelectedObject();
			if(selectedObj.name.Equals("tutorial2_bigboxPrefab(Clone)")) {
				highlighter.HighlightTimed(GameObject.Find("bigbox_close"), 2); 
				highlighter.HighlightTimed(GameObject.Find("bigbox_far"), 2); 
			} else if(selectedObj.name.Equals("tutorial2_smallbox_bluePrefab(Clone")) {
				highlighter.HighlightTimed(GameObject.Find("smallbox_blue"), 2); 
			} else if(selectedObj.name.Equals("tutorial2_smallbox_yellowPrefab(Clone")) {
				highlighter.HighlightTimed(GameObject.Find("smallbox_yellow"), 2); 
			} else {
				highlighter.HighlightTimed(GameObject.Find("tallbox"), 2); 
			}


		} else if(ConversationTrigger.GetToken("wrongRotationDreshaReadyToFlashObj") && !ConversationTrigger.GetToken("blockTryRotatingAgain2")){
			// Dresha flashes selected obj and then lets the player try again
			StartCoroutine(highlightObjWait());
		} else if(ConversationTrigger.GetToken("playerAttachedWrongFace") && !ConversationTrigger.GetToken("wrongFaceDreshaReadyToFlashBox")) {
			// wrong shape - Dresha just finished tryDifferentShape1 and will now flash box
			// for 2 seconds
			highlighter.HighlightTimed(GameObject.Find("longbox"), 2); 

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
		//ConversationTrigger.RemoveToken("wrongFaceDreshaReadyToFlashBox"); // to stop infinite triggers

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

			Debug.Log("adding playerRotationIncorrect token!");
			ConversationTrigger.AddToken("playerRotationIncorrect"); // convo tryRotatingAgain

		} else if (fuseStatus.Equals("success")){
			//player successfully attached part
			ConversationTrigger.RemoveToken("playerAttachedWrongFace"); // to stop infinite triggers
			ConversationTrigger.RemoveToken("playerRotationIncorrect"); // to stop infinite triggers

		}
	}

	IEnumerator highlightPartButtonsWait() {
		foreach (Button b in partButtons) {
			highlighter.HighlightTimed(b.gameObject, 2);
		}
		yield return new WaitForSeconds(2f);
		ConversationTrigger.AddToken("dreshaFlashedPartButtons");
		foreach (Button b in partButtons) {
			b.enabled = true;
		}
	}

	IEnumerator highlightObjWait() {
		GameObject selectedObj = selectPart.getSelectedObject();
		if(selectedObj.name.Equals("tutorial2_bigboxPrefab(Clone)")) {
			highlighter.HighlightTimed(GameObject.Find("bigbox_close"), 1); 
			highlighter.HighlightTimed(GameObject.Find("bigbox_far"), 1); 
		} else if(selectedObj.name.Equals("tutorial2_smallbox_bluePrefab(Clone")) {
			highlighter.HighlightTimed(GameObject.Find("smallbox_blue"), 1); 
		} else if(selectedObj.name.Equals("tutorial2_smallbox_yellowPrefab(Clone")) {
			highlighter.HighlightTimed(GameObject.Find("smallbox_yellow"), 1); 
		} else {
			highlighter.HighlightTimed(GameObject.Find("tallbox"), 1); 
		}
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaFlashedSelectedObj2");
	}
		
}
