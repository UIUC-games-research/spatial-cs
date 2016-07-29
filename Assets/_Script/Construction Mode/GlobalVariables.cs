using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GlobalVariables : MonoBehaviour {
	private List<string> levelNames;

	private List<bool> levelComplete;
	
	private List<Text> levelPasswordTexts;

	public bool newLoad = true;

	private Text rocketBootsPassword;
	private Text axePassword;
	private Text key1Password;
	private Text ffaPassword;
	private Text hullPassword;
	private Text catapultPassword;
	private Text key2Password;
	private Text enginePassword;
	private Text vestPassword;
	private Text glovesPassword;
	private Text key3Password;


	void Awake() {
		DontDestroyOnLoad(this);
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}

		levelNames = new List<string>();
		levelNames.Add ("boot");
		levelNames.Add ("axe");
		levelNames.Add ("key1");
		levelNames.Add ("ffa");
		levelNames.Add ("hull");
		levelNames.Add ("catapult");
		levelNames.Add ("key2");
		levelNames.Add ("engine");
		levelNames.Add ("vest");
		levelNames.Add ("gloves");
		levelNames.Add ("key3");

		levelComplete = new List<bool>();
		for(int i = 0; i < levelNames.Count; i++) {
			levelComplete.Add (false);
		}

		rocketBootsPassword = GameObject.Find ("bootPassword").GetComponent<Text>();
		axePassword = GameObject.Find ("axePassword").GetComponent<Text>();
		key1Password = GameObject.Find ("key1Password").GetComponent<Text>();
		ffaPassword = GameObject.Find ("ffaPassword").GetComponent<Text>();
		hullPassword = GameObject.Find ("hullPassword").GetComponent<Text>();
		catapultPassword = GameObject.Find ("catapultPassword").GetComponent<Text>();
		key2Password = GameObject.Find ("key2Password").GetComponent<Text>();
		enginePassword = GameObject.Find ("enginePassword").GetComponent<Text>();
		vestPassword = GameObject.Find ("vestPassword").GetComponent<Text>();
		glovesPassword = GameObject.Find ("glovesPassword").GetComponent<Text>();
		key3Password = GameObject.Find ("key3Password").GetComponent<Text>();


		levelPasswordTexts = new List<Text>();
		levelPasswordTexts.Add (rocketBootsPassword);
		levelPasswordTexts.Add (axePassword);
		levelPasswordTexts.Add (key1Password);
		levelPasswordTexts.Add (ffaPassword);
		levelPasswordTexts.Add (hullPassword);
		levelPasswordTexts.Add (catapultPassword);
		levelPasswordTexts.Add (key2Password);
		levelPasswordTexts.Add (enginePassword);
		levelPasswordTexts.Add (vestPassword);
		levelPasswordTexts.Add (glovesPassword);
		levelPasswordTexts.Add (key3Password);

		foreach (string levelName in levelNames) {
			int i = levelNames.IndexOf(levelName);
			levelPasswordTexts[i].enabled = false;
		}
	}

	// Use this for initialization
	void Start () {

	}

	public void setLevelCompleted(string levelName) {
		int i = levelNames.IndexOf(levelName);
		levelComplete[i] = true;
	}
	
	public void hidePasswords() {
		foreach (string levelName in levelNames) {
			int i = levelNames.IndexOf(levelName);
			levelPasswordTexts[i].enabled = false;
		}
	}

	public void unHidePasswords() {
		foreach (string levelName in levelNames) {
			int i = levelNames.IndexOf(levelName);
			levelPasswordTexts[i].enabled = true;
		}
	}

	public void backToMainScreen() {
		SceneManager.LoadScene("passwordScreen");
		newLoad = true;
		//since this object is persistent across levels, the following code is ok:
		for(int i = 0; i < levelNames.Count; i++) {
			bool isComplete = levelComplete[i];
			if(!isComplete) {
				levelPasswordTexts[i].enabled = false;
			} else {
				print (levelNames[i] + "Password");
				levelPasswordTexts[i].enabled = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneManager.GetSceneAt(0).isLoaded && newLoad) {
			newLoad = false;

			rocketBootsPassword = GameObject.Find ("bootPassword").GetComponent<Text>();
			axePassword = GameObject.Find ("axePassword").GetComponent<Text>();
			key1Password = GameObject.Find ("key1Password").GetComponent<Text>();
			ffaPassword = GameObject.Find ("ffaPassword").GetComponent<Text>();
			hullPassword = GameObject.Find ("hullPassword").GetComponent<Text>();
			catapultPassword = GameObject.Find ("catapultPassword").GetComponent<Text>();
			key2Password = GameObject.Find ("key2Password").GetComponent<Text>();
			enginePassword = GameObject.Find ("enginePassword").GetComponent<Text>();
			vestPassword = GameObject.Find ("vestPassword").GetComponent<Text>();
			glovesPassword = GameObject.Find ("glovesPassword").GetComponent<Text>();
			key3Password = GameObject.Find ("key3Password").GetComponent<Text>();

			levelPasswordTexts = new List<Text>();
			levelPasswordTexts.Add (rocketBootsPassword);
			levelPasswordTexts.Add (axePassword);
			levelPasswordTexts.Add (key1Password);
			levelPasswordTexts.Add (ffaPassword);
			levelPasswordTexts.Add (hullPassword);
			levelPasswordTexts.Add (catapultPassword);
			levelPasswordTexts.Add (key2Password);
			levelPasswordTexts.Add (enginePassword);
			levelPasswordTexts.Add (vestPassword);
			levelPasswordTexts.Add (glovesPassword);
			levelPasswordTexts.Add (key3Password);

			for(int i = 0; i < levelNames.Count; i++) {
				bool isComplete = levelComplete[i];
				if(!isComplete) {
					levelPasswordTexts[i].enabled = false;
				} else {
					print (levelNames[i] + "Password");
					levelPasswordTexts[i].enabled = true;
				}
			}
		}
	}
}
