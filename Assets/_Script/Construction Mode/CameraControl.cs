using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class CameraControl : MonoBehaviour {

	private int numClicksHigh;
	private int numClicksLow;
	private int numClicksLeft;
	private int numClicksRight;

	public GameObject panUpButton;
	public GameObject panDownButton;
	public GameObject panLeftButton;
	public GameObject panRightButton;

	//tutorial variables
	public Text adjustCamera;
	
	void Awake() {
		numClicksHigh = 0;
		numClicksLow = 0;

	}

	// Use this for initialization
	void Start () {
	
	}

	public void addClickHigh() {
		numClicksHigh++;
		numClicksLow--;
		if(numClicksHigh > 4) {
			panUpButton.transform.GetComponent<Button>().interactable = false;
		}
		if(numClicksLow < 5) {
			panDownButton.transform.GetComponent<Button>().interactable = true;
		}
		if(numClicksHigh == 0) {
			panLeftButton.transform.GetComponent<Button>().interactable = true;
			panRightButton.transform.GetComponent<Button>().interactable = true;
		} else {
			panLeftButton.transform.GetComponent<Button>().interactable = false;
			panRightButton.transform.GetComponent<Button>().interactable = false;
		}
	}

	public void addClickLow() {
		numClicksLow++;
		numClicksHigh--;
		if(numClicksLow > 4) {
			panDownButton.transform.GetComponent<Button>().interactable = false;
		}
		if(numClicksHigh < 5) {
			panUpButton.transform.GetComponent<Button>().interactable = true;

		}
		if(numClicksLow == 0) {
			panLeftButton.transform.GetComponent<Button>().interactable = true;
			panRightButton.transform.GetComponent<Button>().interactable = true;
		} else {
			panLeftButton.transform.GetComponent<Button>().interactable = false;
			panRightButton.transform.GetComponent<Button>().interactable = false;
		}
	}

	public void addClickLeft() {
		numClicksLeft++;
		numClicksRight--;
		if(numClicksLeft > 4) {
			panLeftButton.transform.GetComponent<Button>().interactable = false;
		}
		if(numClicksRight < 5) {
			panRightButton.transform.GetComponent<Button>().interactable = true;
			
		}
		if(numClicksLeft == 0) {
			panUpButton.transform.GetComponent<Button>().interactable = true;
			panDownButton.transform.GetComponent<Button>().interactable = true;
		} else {
			panUpButton.transform.GetComponent<Button>().interactable = false;
			panDownButton.transform.GetComponent<Button>().interactable = false;
		}
	}

	public void addClickRight() {
		numClicksRight++;
		numClicksLeft--;
		if(numClicksRight > 4) {
			panRightButton.transform.GetComponent<Button>().interactable = false;
		}
		if(numClicksLeft < 5) {
			panLeftButton.transform.GetComponent<Button>().interactable = true;
			
		}
		if(numClicksRight == 0) {
			panUpButton.transform.GetComponent<Button>().interactable = true;
			panDownButton.transform.GetComponent<Button>().interactable = true;
		} else {
			panUpButton.transform.GetComponent<Button>().interactable = false;
			panDownButton.transform.GetComponent<Button>().interactable = false;
		}
	}


	public void rotateLeft() {
		transform.RotateAround(new Vector3(-90, 45, 100), new Vector3(0,1,0), 25.0f);
		addClickLeft();
	}

	public void rotateRight() {
		transform.RotateAround(new Vector3(-90, 45, 100), new Vector3(0,-1,0), 25.0f);
		addClickRight();
	}

	public void rotateUp() {
		transform.RotateAround(new Vector3(-100, 30, 100), new Vector3(1,0,0), 25.0f);
		addClickHigh ();

	}

	public void rotateDown() {
		transform.RotateAround(new Vector3(-100, 30, 100), new Vector3(-1,0,0), 25.0f);
		addClickLow ();
		
	}



//	public void panUp() {
//		transform.Translate(new Vector3(0,15,0));
//		addClickHigh ();
//	}
//
//	public void panDown() {
//		transform.Translate(new Vector3(0,-15,0));
//		addClickLow ();
//	}

	public int getNumClicksLow() {
		return numClicksLow;
	}
	
	public int getNumClicksHigh() {
		return numClicksHigh;
	}

	public int getNumClicksLeft() {
		return numClicksLeft;
	}
	
	public int getNumClicksRight() {
		return numClicksRight;
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.DownArrow) && numClicksLow < 5 && numClicksRight == 0) {
			rotateDown ();
		} else if (Input.GetKeyDown (KeyCode.UpArrow) && numClicksHigh < 5 && numClicksRight == 0) {
			rotateUp ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow) && numClicksRight < 5 && numClicksHigh == 0) {
			rotateRight();
		} else if (Input.GetKeyDown (KeyCode.LeftArrow) && numClicksLeft < 5 && numClicksHigh == 0) {
			rotateLeft();
		}
	}
}
