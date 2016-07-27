using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour {
	public GameObject finishedImage;
	public Button[] partButtons;

	private bool[] triggersFinished;
	private const int NUM_TRIGGERS = 18;
	public Highlighter highlighter;

	public Text congrats;
	public Button goToRocketBootsLevel;
	private GameObject selectedObj;

	// Use this for initialization
	void Start () {
		triggersFinished = new bool[NUM_TRIGGERS];
		for(int i=0; i<NUM_TRIGGERS; i++) {
			triggersFinished[i] = false;
		}

		//disable part buttons so player can't use them while Dresha talks
		foreach(Button b in partButtons) {
			b.interactable = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!triggersFinished[0] && ConversationTrigger.tokens.Contains("dreshaReadyToFlashFinishedImage2")) {
			triggersFinished[0] = true;
			// first event: Dresha flashes finished image, which triggers next convo
			highlighter.HighlightTimed(finishedImage, 2);
			ConversationTrigger.AddToken("dreshaFlashedFinishedImage2");
		}




	}
}
