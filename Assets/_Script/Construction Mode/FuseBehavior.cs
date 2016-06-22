using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FuseBehavior : MonoBehaviour {



	//list of GameObjects this object can be fused to along with corresponding fuse locations
	private FuseAttributes fuseInfo;
	//private Dictionary<string, Quaternion> fuseRotations;
	public bool isFused;
	private GameObject assignedButton;
	private bool tagged;

	// Use this for initialization
	void Awake () {
		if(this.name.Equals("rocket_boots_start") || this.name.Equals ("base")) {
			isFused = true;
		} else {
			isFused = false;
		}
		tagged = false;
	}

	// Update is called once per frame
	void Update () {

	}

	public void setFuseTo(FuseAttributes fuseAtts) {
		fuseInfo = fuseAtts;

	}

	public void setTagged(bool isTagged) {
		tagged = isTagged;
	}

	public void setButtonTo(GameObject newButton) {
		assignedButton = newButton;
		
	}

	public Vector3 getFuseLocation(string fuseTo) {
		return fuseInfo.getFuseLocation(fuseTo);
	}

	public Quaternion getFuseRotation(string fuseTo) {
		return fuseInfo.getFuseRotation(fuseTo);
	}

	public Quaternion[] getAcceptableRotations(string fuseTo) {
		return fuseInfo.getAcceptableRotations(fuseTo);
	}

	public string[] getAcceptableLocations(string fuseTo) {
		return fuseInfo.getAcceptableLocations();
	}

	public bool fused() {
		return isFused;
	}

	public void fuse(string fuseTo, string suffix) {
		GameObject parent = gameObject.transform.parent.gameObject;
		string newFuseToName = fuseTo + suffix;
		print ("fusing " + gameObject + " to " + newFuseToName + ":");

		parent.transform.position = fuseInfo.getFuseLocation(newFuseToName);
		parent.transform.rotation = fuseInfo.getFuseRotation(newFuseToName);

		//Destroy(gameObject.GetComponent<SelectBehavior>());

		isFused = true;
		parent.GetComponent<IsFused>().isFused = true;
		//FuseBehavior[] fuseBehaviors = parent.transform.GetComponentsInChildren<FuseBehavior>();
		//for (int i = 0; i < fuseBehaviors.Length; i++) {
		//	fuseBehaviors[i].isFused = true;
		//}
		int uses = assignedButton.GetComponent<Uses>().numUses;

		if(uses > 1) {
			assignedButton.GetComponent<Uses>().numUses = uses - 1;
		} else if(uses <= 1) {
			assignedButton.transform.GetComponent<Button>().interactable = false;
		}
		assignedButton = null;

		if(tagged) {
			string[] locs = {"_left", "_right", "_top"};
			foreach (string s in locs) {
				if(fuseTo.Contains (s)) {
					gameObject.transform.parent.gameObject.GetComponent<IsFused>().locationTag = s;
				}
			}

		}
	}
}
