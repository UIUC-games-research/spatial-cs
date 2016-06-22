using UnityEngine;
using System.Collections;

public class WorldBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIController.DisplayWarning("I Can Not' Enter Toxic Land");
        }
    }

    void OnTriggerExit(Collider other) {
        UIController.DisplayWarning("");
    }
}
