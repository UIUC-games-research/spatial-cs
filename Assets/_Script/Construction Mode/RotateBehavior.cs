using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateBehavior : MonoBehaviour {
	Quaternion endRotation = new Quaternion();

	private float rotation = 0; 
	public float targetRotationLeft;
	public float targetRotationForward;
	public float targetRotationAcross;
	private float targetRotation = 90;

	private float rotateSpeed = 180.0f;
	private bool rotatingLeft = false;
	private bool rotatingForward = false;
	private bool rotatingAcross = false;

	// Use this for initialization
	void Start () {
	
	}

	public bool getRotatingLeft() {
		return rotatingLeft;
	}

	public bool getRotatingForward() {
		return rotatingForward;
	}

	public bool getRotatingAcross() {
		return rotatingAcross;
	}

	public void setRotatingLeft(bool rotating) {
		rotatingLeft = rotating;
	}	

	public void setRotatingForward(bool rotating) {
		rotatingForward = rotating;
	}

	public void setRotatingAcross(bool rotating) {
		rotatingAcross = rotating;
	}

	public void setEndRotation(Quaternion rotation) {
		endRotation = rotation;
	}

	public float getTargetRotationLeft() {
		return targetRotationLeft;
	}

	public float getTargetRotationForward() {
		return targetRotationForward;
	}

	public float getTargetRotationAcross() {
		return targetRotationAcross;
	}

	public Quaternion getEndRotation() {
		return endRotation;
	}

	public bool rotating() {
		return rotatingLeft || rotatingForward || rotatingAcross;
	}

	public float getTargetRotation() {
		if(rotatingLeft) {
			return targetRotationLeft;
		} else if(rotatingForward) {
			return targetRotationForward;
		} else if(rotatingAcross) {
			return targetRotationAcross;
		} else {
			return 1000; //shouldn't happen
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetKeyDown ("q") && !rotating) {
//			rotating = true;
//			startRotation = transform.rotation;
//			transform.Rotate(targetRotation, 0,0);
//			endRotation = transform.rotation;
//			transform.Rotate (-targetRotation,0,0);
//		}
		if(rotating ()) {
			float rotationAmt = rotateSpeed * Time.deltaTime;
			targetRotation = getTargetRotation();
			if(!Mathf.Approximately(rotation, targetRotation)) {
				//first we check to see if there's just a small amount of rotation to shave off
				// this check keeps us from never reaching the target rotation
				if(Quaternion.Angle (transform.rotation, endRotation) < 1.0f) {
					if(rotatingLeft) {
						rotatingLeft = false;
					}else if (rotatingForward) {
						rotatingForward = false;
					} else if (rotatingAcross) {
						rotatingAcross = false;
					}
					transform.rotation = endRotation;
					rotation = 0;
				} else if(Mathf.Abs(rotation-targetRotation) < rotationAmt) {
					if(rotatingLeft) {
						transform.Rotate(0, targetRotation-rotation, 0);
					} else if (rotatingForward) {
						transform.Rotate(targetRotation-rotation, 0, 0);
					} else { //rotating Across
						transform.Rotate(0, 0, targetRotation-rotation);
					}
					rotation += rotation-targetRotation;
				} else {
					if(rotatingLeft) {
						transform.Rotate(0, -rotationAmt, 0);
					} else if (rotatingForward) {
						transform.Rotate(-rotationAmt, 0, 0);
					} else { //rotating Across
						transform.Rotate(0, 0, -rotationAmt);
					}
					rotation += rotationAmt;
				}
			}
		}
	}


}
