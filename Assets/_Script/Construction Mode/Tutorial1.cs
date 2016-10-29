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

	private GameObject selectedObj;
	private GameObject conversationSystem;

	void Awake() {
	}

	// Use this for initialization
	void Start () {

		rotationScript = rotationGizmo.GetComponent<RotationGizmo>();
		fuseEvent = eventSystem.GetComponent<FuseEvent>();
		selectPart = eventSystem.GetComponent<SelectPart>();
		conversationSystem = GameObject.Find("ConversationSystem");

	
	}

	// Update is called once per frame
	void Update () {


	
	}  
		

	private void highlightPartButtons() {
		foreach (Button b in partButtons) {
			highlighter.HighlightTimed(b.gameObject, 2);
		}
	//	ConversationTrigger.AddToken("dreshaFlashedPartButtons");

	}

	//highlights for the specified length of time, then unhighlights
	private void highlightSelectedObj(float sec) { // generalizes to any selectedObj
		GameObject activePart = selectPart.getActivePart();
		Debug.Log("Selected Obj to highlight: " + activePart);
		if(activePart.name.Equals("tutorial1_triPrefab(Clone)")) {
			highlighter.HighlightTimed(GameObject.Find("tri"), sec); 
		} else if(activePart.name.Equals("tutorial1_pyrPrefab(Clone)")) {
			highlighter.HighlightTimed(GameObject.Find("pyr"), sec); 
		} else {
			highlighter.HighlightTimed(GameObject.Find("cone"), sec); 
		}
	}
		

	private void unhighlightSelectedObj() { // generalizes to any selectedObj
		GameObject activePart = selectPart.getActivePart();
		Debug.Log("Selected Obj to UNhighlight: " + activePart);
		if(activePart.name.Equals("tutorial1_triPrefab(Clone)")) {
			Highlighter.Highlight(GameObject.Find("tri")); 
		} else if(activePart.name.Equals("tutorial1_pyrPrefab(Clone)")) {
			Highlighter.Unhighlight(GameObject.Find("pyr")); 
		} else {
			Highlighter.Unhighlight(GameObject.Find("cone")); 
		}
	}

	IEnumerator highlightGizmoWait() {
		// maybe should highlight only the sliders instead?
		foreach(Transform child in rotationGizmo.transform) {
			highlighter.HighlightTimed(child.gameObject, 2); 
		}
		yield return new WaitForSeconds(1f);
		ConversationTrigger.AddToken("dreshaFlashedGizmo");
	}


		
}
