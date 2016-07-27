using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateButton : MonoBehaviour {

	private GameObject objectToRotate;

	//tutorial variables
	public bool tutorialOn;
	private float timer;
	private bool timerStart;

	//data collection
	private int numXClicks;
	private int numYClicks;
	private int numZClicks;


	void Awake() {
		objectToRotate = null;
		timer = 0;
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
		// disable battery restrictions when in tutorial
		if (tutorialOn || BatterySystem.GetPower() > 0)
		{
			if(!tutorialOn){
				BatterySystem.SubPower(1);
			}
			numYClicks++;
			RotateBehavior rotateBehavior = objectToRotate.GetComponent<RotateBehavior>();
			if (!rotateBehavior.rotating())
			{
				rotateBehavior.setRotatingLeft(true);
				float targetRotation = rotateBehavior.getTargetRotation();

				objectToRotate.transform.Rotate(0, -targetRotation, 0);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(0, targetRotation, 0);

			}
			timerStart = true;

		}

	}
	
	public void startRotatingForward() {
		// disable battery restrictions when in tutorial
		if (tutorialOn || BatterySystem.GetPower() > 0)
		{
			if(!tutorialOn) {
				BatterySystem.SubPower(1);
			}
			numXClicks++;
			RotateBehavior rotateBehavior = objectToRotate.GetComponent<RotateBehavior>();
			if (!rotateBehavior.rotating())
			{
				//rotate
				rotateBehavior.setRotatingForward(true);
				float targetRotation = rotateBehavior.getTargetRotation();

				objectToRotate.transform.Rotate(-targetRotation, 0, 0);
				rotateBehavior.setEndRotation(objectToRotate.transform.rotation);
				objectToRotate.transform.Rotate(targetRotation, 0, 0);

			}
				
			timerStart = true;
		}
	}
	
	public void startRotatingAcross() {
		// disable battery restrictions when in tutorial
		if (tutorialOn || BatterySystem.GetPower() > 0)
		{
			if(!tutorialOn) {
				BatterySystem.SubPower(1);
			}
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
		
}
