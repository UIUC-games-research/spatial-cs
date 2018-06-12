using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenu : MonoBehaviour {

    public GameObject controlsMenu;
    public Button openControls;
    public Button closeControls;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void openControlsMenu()
    {
        controlsMenu.SetActive(true);
        openControls.interactable = false;
        closeControls.gameObject.SetActive(true);
    }

    public void closeControlsMenu()
    {
        controlsMenu.SetActive(false);
        openControls.interactable = true;
        closeControls.gameObject.SetActive(false);
    }
}
