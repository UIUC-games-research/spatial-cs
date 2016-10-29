using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial2 : MonoBehaviour {
	public GameObject finishedImage;
	public Button[] partButtons;

	public Highlighter highlighter;

	public Text congrats;
	public Button goToRocketBootsLevel;
	public GameObject eventSystem;
	private SelectPart selectPart;
	private FuseEvent fuseEvent;
	private GameObject selectedObj;
	private GameObject conversationSystem;

	private bool done;

	void Awake() {

	}

	// Use this for initialization
	void Start () {

		selectPart = eventSystem.GetComponent<SelectPart>();
		fuseEvent = eventSystem.GetComponent<FuseEvent>();
		conversationSystem = GameObject.Find("ConversationSystem");

	}
	
	// Update is called once per frame
	void Update () {


	}
		

	private void highlightSelectedObj(float sec) { // generalizes to any selectedObj
		GameObject selectedObj = selectPart.getActivePart();
		if(selectedObj.name.Equals("tutorial2_bigboxPrefab(Clone)")) {
			highlighter.HighlightTimed(GameObject.Find("bigbox_close"), sec); 
			highlighter.HighlightTimed(GameObject.Find("bigbox_far"), sec); 
		} else if(selectedObj.name.Equals("tutorial2_smallbox_bluePrefab(Clone)")) {
			highlighter.HighlightTimed(GameObject.Find("smallbox_blue"), sec); 
		} else if(selectedObj.name.Equals("tutorial2_smallbox_yellowPrefab(Clone)")) {
			highlighter.HighlightTimed(GameObject.Find("smallbox_yellow"), sec); 
		} else {
			highlighter.HighlightTimed(GameObject.Find("tallbox"), sec); 
		}
	}

	private void highlightPartButtons(float sec) {
		foreach (Button b in partButtons) {
			highlighter.HighlightTimed(b.gameObject, sec);
		}
	}
		
}
