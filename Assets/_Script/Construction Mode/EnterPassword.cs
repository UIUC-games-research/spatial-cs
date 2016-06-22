using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterPassword : MonoBehaviour {

	// passwords from Exploration mode that are used to enter Construction Mode for each item
	private Dictionary<string, string> levelAccessPasswords;
	
	// mapping between level numbers and level names for each level
	private Dictionary<string, int> passwordNumMapping;

	public InputField passwordInput;
	public Text invalidPassword;
	private string currentPassword;
	
	void Awake() {
		currentPassword = "";
		levelAccessPasswords = new Dictionary<string, string>();
		levelAccessPasswords.Add ("e7o9at", "introConstruction");
		levelAccessPasswords.Add ("testbt", "construction");
		levelAccessPasswords.Add ("kvdikh", "axe");
		levelAccessPasswords.Add ("nyojqz", "ffa");
		levelAccessPasswords.Add ("tcugbq", "forceGloves");
		levelAccessPasswords.Add ("davbgx", "spaceshipHull");
		levelAccessPasswords.Add ("fyvtzj", "key1");
		levelAccessPasswords.Add ("9frhxi", "key2");
		levelAccessPasswords.Add ("ewzi9h", "catapult");
		levelAccessPasswords.Add ("dpapu7", "spaceshipEngine");
		levelAccessPasswords.Add ("fvfccy", "fireProofVest");
		levelAccessPasswords.Add ("e3g7kb", "key3");
	}

	// Use this for initialization
	void Start () {
		
	}

	public void checkPassword() {
		if(levelAccessPasswords.ContainsKey(currentPassword)) {
			string levelName = levelAccessPasswords[currentPassword];
			SceneManager.LoadScene(levelName);
		} else {
			invalidPassword.enabled = true;
		}
	}

	public void hideInvalidPasswordText() {
		invalidPassword.enabled = false;
	}

	public void updatePassword() {
		currentPassword = passwordInput.text;
		//print (currentPassword);
	}

	public void quit() {
		Application.Quit();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
