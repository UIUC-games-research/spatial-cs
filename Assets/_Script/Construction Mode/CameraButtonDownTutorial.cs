using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraButtonDownTutorial : MonoBehaviour {

	public Text clickCameraUp;
	public Text clickCameraDown;
	public GameObject clickCameraUpButton;

	private bool tutorialOn;
	private int tutorialCount;

	//this method is triggered when a camera button is clicked
	// should be attached to "see higher up" button on interface
	// Use this for initialization
	void Awake () {
		tutorialOn = true;
		tutorialCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void continueTutorial() {
		print ("Down Button: tutorialOn? " + tutorialOn + ", tutorialCount = " + tutorialCount);
		if(tutorialOn) {
			if(tutorialCount < 3) {
				tutorialCount++;
			} else { 
				clickCameraDown.enabled = false;
				clickCameraUp.enabled = true;
				clickCameraUpButton.GetComponent<Button>().interactable = true;
				tutorialOn = false;
			}

		}
	}
}
