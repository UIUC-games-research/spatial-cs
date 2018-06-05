using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial1 : MonoBehaviour {

	public GameObject eventSystem;
	private SelectPart selectPart;
	private FuseEvent fuseEvent;
    public Camera mainCam;
	public Button[] partButtons;
	public Button connectButton;
	public GameObject finishedImage;
	public Button b1p1Button;
	public GameObject rotationGizmo;
	private RotationGizmo rotationScript;
    private CameraControls cameraControls;
	public Highlighter highlighter;
    private GameObject bb1_b1p2_a1; // incorrect first attempt
    private GameObject bb1_b1p1_a1; // correct last attempt
    private GameObject b1p1_bb1_a1; // selected fuse area on part
	public Text shapesWrong;
	public Text rotationWrong;
	public Text congrats;
	public Button goToNextTutorial;
    public bool tutorialOn;
    private Vector3 baseStartPosition;
    public GameObject bb1;
    private GameObject b1p1;

    public Image arrowPartButtons;
    public Image arrowFinishedImageLeft;
    public Image arrowFinishedImageUp;

    private const float MOVEMENT_SPEED = 100f;
    private const float SHOW_IMAGE_DURATION = 2f;
    private float step;

    private bool flashedPartButtons;
    private bool clickedB1P1Button;
    private bool flashedFinishedImage;
    private bool selectedAC;
    private bool selectedFuseTo;
    private bool rotatedOnceWrongFace;
    private bool rotatedTwiceWrongFace;
    private bool rotatedOnceWrongRotation;


	private GameObject selectedObj;

	void Awake() {
	}

	// Use this for initialization
	void Start () {

		rotationScript = rotationGizmo.GetComponent<RotationGizmo>();
		fuseEvent = eventSystem.GetComponent<FuseEvent>();
		selectPart = eventSystem.GetComponent<SelectPart>();
        cameraControls = mainCam.GetComponent<CameraControls>();
        baseStartPosition = new Vector3(-100, 30, 100);
        flashedPartButtons = false;
        clickedB1P1Button = false;
        flashedFinishedImage = false;
        selectedAC = false;
        selectedFuseTo = false;
        rotatedOnceWrongFace = false;
        rotatedTwiceWrongFace = false;
    }

    // Update is called once per frame
    void Update () {

        // show Dresha moving starting part into position from bottom of screen
        if(!ConversationTrigger.GetToken("finishedMovingbb1") && ConversationTrigger.GetToken("finishedConst_1"))
        {
            step = MOVEMENT_SPEED * Time.deltaTime;
            bb1.transform.position = Vector3.MoveTowards(bb1.transform.position, baseStartPosition, step);
            if(bb1.transform.position.Equals(baseStartPosition))
            {
                Debug.Log("finished moving!");
                ConversationTrigger.AddToken("finishedMovingbb1");
            }
        }

        // draw player's attention to the part buttons at the bottom of the screen
        else if(!flashedPartButtons && ConversationTrigger.GetToken("finishedConst_2"))
        {
            flashedPartButtons = true;
            StartCoroutine(showImageAndAddToken(arrowPartButtons, SHOW_IMAGE_DURATION, "finishedFlashingPartButtons"));
            highlightPartButtons();

        }

        // Dresha clicks the part button, part appears
        else if (!clickedB1P1Button && ConversationTrigger.GetToken("finishedConst_3"))
        {
            clickedB1P1Button = true;
            b1p1Button.onClick.Invoke();
            b1p1Button.interactable = false;
            StartCoroutine(waitThenAddToken("finishedSelectingPart", 2f));

        }

        // draw player's attention to the finished image at the top left of the screen
        else if (!flashedFinishedImage && ConversationTrigger.GetToken("finishedConst_4"))
        {
            flashedFinishedImage = true;
            StartCoroutine(showImageAndAddToken(arrowFinishedImageLeft.GetComponent<Image>(), SHOW_IMAGE_DURATION, "finishedFlashingFinishedImage"));
            StartCoroutine(showImage(arrowFinishedImageUp.GetComponent<Image>(), SHOW_IMAGE_DURATION));
            highlighter.HighlightTimed(finishedImage, 2);

        }

        // Dresha selects the black area on bb1
        else if (!selectedFuseTo && ConversationTrigger.GetToken("finishedConst_5"))
        {
            selectedFuseTo = true;
            // moves camera from starting position to a good view of bb1_b1p2_a1
            cameraControls.autoRotateCamera(-0.3f, -46f, 0f, 2f);

            bb1_b1p2_a1 = GameObject.Find("bb1_b1p2_a1");
            selectPart.selectFuseTo(bb1_b1p2_a1);
            StartCoroutine(waitThenAddToken("finishedSelectingbb1_a1", 2f));
        }

        // Dresha selects the black area on b1p1
        else if (!selectedAC && ConversationTrigger.GetToken("finishedConst_6"))
        {
            selectedAC = true;
            b1p1 = GameObject.Find("b1p1Prefab(Clone)");
            b1p1_bb1_a1 = b1p1.transform.GetChild(1).gameObject;
            //moves camera from cam angle1 to a good view of b1p1_bb1_a1
            cameraControls.autoRotateCamera(-63f, -8.4f, 0f, 2f);

            selectPart.selectObject(b1p1_bb1_a1);
            StartCoroutine(waitThenAddToken("finishedSelectingb1p1_a1", 2f));
        }

        // Dresha rotates once along y axis - should go so the black part is facing up
        else if (!rotatedOnceWrongFace && ConversationTrigger.GetToken("finishedConst_7"))
        {
            rotatedOnceWrongFace = true;
 
            StartCoroutine(rotateWrongFaceScript());
            //GameObject YRightSlider = rotationScript.yGizmo.transform.GetChild(0).gameObject); // should be YRight
            //StartCoroutine(waitThenRotateAndHighlight(b1p1, YRightSlider, 0f, -90f,0f, 2f));
            //StartCoroutine(waitThenAddToken("finishedRotatingWrongFace", 0f));
        }

        //Then Dresha rotates once along z axis - should go so the black part is facing bb1
        else if (!rotatedTwiceWrongFace && ConversationTrigger.GetToken("finishedConst_8"))
        {
            rotatedTwiceWrongFace = true;

            StartCoroutine(rotateTwiceWrongFaceScript());
            //GameObject YRightSlider = rotationScript.yGizmo.transform.GetChild(0).gameObject); // should be YRight
            //StartCoroutine(waitThenRotateAndHighlight(b1p1, YRightSlider, 0f, -90f,0f, 2f));
            //StartCoroutine(waitThenAddToken("finishedRotatingWrongFace", 0f));
        }

    }

    IEnumerator rotateWrongFaceScript()
    {
        b1p1 = GameObject.Find("b1p1Prefab(Clone)");
        Highlighter.Highlight(rotationScript.yGizmo); 
        yield return new WaitForSeconds(2f);
        Highlighter.Unhighlight(rotationScript.yGizmo);
        rotationScript.runManualRotation(b1p1, 0,90,0);
        yield return new WaitForSeconds(2f);
        Highlighter.Highlight(rotationScript.zGizmo);

        ConversationTrigger.AddToken("finishedRotatingOnceWrongFace");

    }

    IEnumerator rotateTwiceWrongFaceScript()
    {
        Highlighter.Unhighlight(rotationScript.zGizmo);
        rotationScript.runManualRotation(b1p1, 0, 0, -90);
        yield return new WaitForSeconds(2f);
        ConversationTrigger.AddToken("finishedRotatingTwiceWrongFace");
    }

    IEnumerator waitThenHighlight(GameObject toHighlight, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Highlighter.Highlight(toHighlight); 

    }

    IEnumerator waitThenAddToken(string token, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ConversationTrigger.AddToken(token);
        Debug.Log("Added token " + token + " successfully!");
    }

    IEnumerator showImage(Image imgToFlash, float time)
    {
        imgToFlash.enabled = true;
        yield return new WaitForSeconds(time);
        imgToFlash.enabled = false;

    }

    IEnumerator showImageAndAddToken(Image imgToFlash, float time, string token)
    {
        imgToFlash.enabled = true;
        yield return new WaitForSeconds(time);
        imgToFlash.enabled = false;

        ConversationTrigger.AddToken(token);
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
