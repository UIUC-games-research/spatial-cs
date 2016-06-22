using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(this);
		foreach(Transform child in transform) {
			DontDestroyOnLoad(child);
		}
		GameObject[] passwordCanvases = GameObject.FindGameObjectsWithTag("passwordCanvas");
		if (passwordCanvases.Length > 1) {
			print ("Number of passwordCanvases: " + GameObject.FindGameObjectsWithTag("passwordCanvas").Length);
			Destroy(gameObject);
		}
	}
	// Use this for initialization
	void Start () {

	}
	

	// Update is called once per frame
	void Update () {
	
	}
}
