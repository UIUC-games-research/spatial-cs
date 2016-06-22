using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraButtonUpTutorial : MonoBehaviour {
	
	public Text clickCameraUp;
	public Text clickCameraDown;
	public Text nowYou;
	public GameObject topButton;
	public GameObject bottomButton;
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
		print ("Up Button: tutorialOn? " + tutorialOn + ", tutorialCount = " + tutorialCount);
		if(tutorialOn) {
			bottomButton.GetComponent<Button>().interactable = true;
			if(tutorialCount < 3) {
				tutorialCount++;
			} else { 
				nowYou.enabled = true;
				clickCameraUp.enabled = false;
				topButton.GetComponent<Button>().interactable = true;
				tutorialOn = false;
			}
			
		}
	}
}

