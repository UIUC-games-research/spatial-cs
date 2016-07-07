using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateButton : MonoBehaviour {

	private GameObject objectToRotate;

	//tutorial variables
	public bool tutorialOn;
	private bool doneWithRotateX;
	private int tutorialCount;
	private int secondTutorialCount;
	private float timer;
	private bool timerStart;
	public Text useConnectButton;
	public Text nowAligned;
	public Text adjustCamera;
	public Text rotateToFind;
	public Text clickOnMatching;
	public Text lineUpBlackAreas;
	public GameObject connectButton;

	public CanvasGroup useConnectButtonPanel;
	public CanvasGroup findOtherBlackRegionPanel;

	//data collection
	private int numXClicks;
	private int numYClicks;
	private int numZClicks;


	void Awake() {
		objectToRotate = null;
		tutorialCount = 0;
		secondTutorialCount = 0;
		timer = 0;
		doneWithRotateX = false;
		numXClicks = 0;
		numYClicks = 0;
		numZClicks = 0;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//this should fix the stuck/infinite rotation glitch
		if(timerStart) {
			transform.GetComponent<Button>().interactable = false;
			timer += Time.deltaTime;
			RotateBehavior rotateBehavior = objectToRotate.GetComponent<RotateBehavior>();
			if(!rotateBehavior.rotating ()) {
				transform.GetComponent<Button>().interactable = true;
				print ("secondTutorialCount = " + secondTutorialCount);
				if(tutorialOn && tutorialCount == 2 && secondTutorialCount == 1) {
					transform.GetComponent<Button>().interactable = false;
				} else if(doneWithRotateX && secondTutorialCount == 2) {
					transform.GetComponent<Button>().interactable = false;
				}
				timerStart = false;
				timer = 0;
			}else if(rotateBehavior.rotating () && timer > 1) {
				Quaternion endRotation = rotateBehavior.getEndRotation();
				objectToRotate.transform.rotation = endRotation;
				timerStart = false;
				timer = 0;
				transform.GetComponent<Button>().interactable = true;
			}
		}
	}

	public void startRotatingLeft() {
		if (BatterySystem.GetPower() > 0)
		{
			BatterySystem.SubPower(1);
			numYClicks++;
			RotateBehavior rotateBehavior = objectToRotate.GetComponent<RotateBehavior>();
			if (!rotateBehavior.rotating())
			{
				rotateBehavior.setRotatingLeft(true);
				float targetRotation = rotateBehavior.getTargetRotation();

				objectToRotate.transform.Rotate(0, -targetRotation, 0);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(0, targetRotation, 0);

				timerStart = true;
			}
		}

	}
	
	public void startRotatingForward() {
		if (BatterySystem.GetPower() > 0)
		{
			BatterySystem.SubPower(1);
			numXClicks++;
			if (doneWithRotateX)
			{
				doneWithRotateX = false;
			}
			RotateBehavior rotateBehavior = objectToRotate.GetComponent<RotateBehavior>();
			if (!rotateBehavior.rotating() && tutorialOn && secondTutorialCount == 2)
			{
				//rotate
				rotateBehavior.setRotatingForward(true);
				float targetRotation = rotateBehavior.getTargetRotation();
				objectToRotate.transform.Rotate(-targetRotation, 0, 0);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(targetRotation, 0, 0);

				//tutorial stuff
				print("Not rotating, tutorial is on, second tutorial count = 2");
				//gameObject.GetComponent<Button>().interactable = false;
				tutorialOn = false;
				doneWithRotateX = true;

				lineUpBlackAreas.enabled = false;
				findOtherBlackRegionPanel.alpha = 0;
				useConnectButton.enabled = true;
				useConnectButtonPanel.alpha = 1;
				connectButton.GetComponent<Button>().interactable = true;

			}
			else if (!rotateBehavior.rotating() && tutorialOn && tutorialCount == 1)
			{
				//rotate
				rotateBehavior.setRotatingForward(true);
				float targetRotation = rotateBehavior.getTargetRotation();
				objectToRotate.transform.Rotate(-targetRotation, 0, 0);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(targetRotation, 0, 0);

				//tutorial stuff
				print("tutorial is on, tutorial count = 1");
				//gameObject.GetComponent<Button>().interactable = false;
				rotateToFind.enabled = false;
				clickOnMatching.enabled = true;
				secondTutorialCount++;
				tutorialCount++;
			}
			else if (!rotateBehavior.rotating() && tutorialOn && secondTutorialCount == 0)
			{
				tutorialCount++;
				print("tutorial is on, second tutorial count = 0");

				//rotate
				rotateBehavior.setRotatingForward(true);
				float targetRotation = rotateBehavior.getTargetRotation();
				objectToRotate.transform.Rotate(-targetRotation, 0, 0);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(targetRotation, 0, 0);
			}
			else if (!rotateBehavior.rotating() && tutorialOn && secondTutorialCount == 1)
			{
				secondTutorialCount++;
				//rotate
				print("tutorial is on, second tutorial count = 1");

				rotateBehavior.setRotatingForward(true);
				float targetRotation = rotateBehavior.getTargetRotation();
				objectToRotate.transform.Rotate(-targetRotation, 0, 0);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(targetRotation, 0, 0);
			}
			else if (!rotateBehavior.rotating())
			{
				//rotate
				print("Not rotating");

				rotateBehavior.setRotatingForward(true);
				float targetRotation = rotateBehavior.getTargetRotation();
				objectToRotate.transform.Rotate(-targetRotation, 0, 0);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(targetRotation, 0, 0);
			}
			else {
				print("??? else condition");

			}
			timerStart = true;
		}
	}
	
	public void startRotatingAcross() {
		if (BatterySystem.GetPower() > 0)
		{
			BatterySystem.SubPower(1);
			numZClicks++;
			RotateBehavior rotateBehavior = objectToRotate.GetComponent<RotateBehavior>();
			if (!rotateBehavior.rotating())
			{
				rotateBehavior.setRotatingAcross(true);
				float targetRotation = rotateBehavior.getTargetRotation();

				objectToRotate.transform.Rotate(0, 0, -targetRotation);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(0, 0, targetRotation);
			}
			timerStart = true;
		}
	}

	public void setObjectToRotate(GameObject toRotate) {
		objectToRotate = toRotate;
	}

	public int getNumXRotations() {
		return numXClicks;
	}

	public int getNumYRotations() {
		return numYClicks;
	}

	public int getNumZRotations() {
		return numZClicks;
	}

//	public void rotateX90() {
//		//print ("tutorialCount: " + tutorialCount + ", secondTutorialCount: " + secondTutorialCount);
//		if(tutorialOn && secondTutorialCount == 2) {
//			objectToRotate.transform.Rotate(-90,0,0);
//			gameObject.GetComponent<Button>().interactable = false;
//			getCorrectRotation.enabled = false;
//			getCorrectRotationPanel.alpha = 0;
//			nowAligned.enabled = true;
//			nowAlignedPanel.alpha = 1;
//			tutorialOn = false;
//			StartCoroutine (alignment());
//		} else if(tutorialOn && tutorialCount == 1) {
//			objectToRotate.transform.Rotate(-90,0,0);
//			gameObject.GetComponent<Button>().interactable = false;
//			rotateToBlack.enabled = false;
//			rotateToBlackPanel.alpha = 0;
//			secondTutorialCount++;
//			tutorialCount++;
//			matchBlackRegions.enabled = true;
//			matchBlackRegionsPanel.alpha = 1;
//		} else if (tutorialOn && secondTutorialCount == 0) {
//			tutorialCount++;
//			objectToRotate.transform.Rotate(-90,0,0);
//		}else if(tutorialOn && secondTutorialCount == 1){
//			secondTutorialCount++;
//			objectToRotate.transform.Rotate(-90,0,0);
//		} else {
//			objectToRotate.transform.Rotate(-90,0,0);
//		}
//
//	}

	public void rotateY90() {
		objectToRotate.transform.Rotate(0,90,0);
	}

	public void rotateY120() {
		objectToRotate.transform.Rotate(0,120,0);

	}
	
	public void rotateZ90() {

		objectToRotate.transform.Rotate(0,0,90);
		
	}
}
