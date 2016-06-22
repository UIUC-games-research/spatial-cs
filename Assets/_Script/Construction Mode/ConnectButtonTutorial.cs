using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectButtonTutorial : MonoBehaviour {

	public Text useConnectButton;
	public Text nowYou;
	private bool tutorialOn;
	private GameObject topButton;

	public Texture midUnhighTex;
	public Texture midHighTex;

	public CanvasGroup useConnectButtonPanel;
	public CanvasGroup nowYouPanel;

	// Use this for initialization
	void Awake () {
		tutorialOn = true;
		topButton = GameObject.Find ("Top");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void continueTutorial() {
		if(tutorialOn) {
			// initiateFuse() should take care of disabling connect button
			useConnectButton.enabled = false;
			useConnectButtonPanel.alpha = 0;
			nowYou.enabled = true;
			nowYouPanel.alpha = 1;
			tutorialOn = false;
			topButton.GetComponent<Button>().interactable = true;
			GameObject midTopAttach = GameObject.Find ("mid_attach");
			midTopAttach.AddComponent<SelectBehavior>();
			midTopAttach.GetComponent<SelectBehavior>().setUnhighTex(midUnhighTex);
			midTopAttach.GetComponent<SelectBehavior>().setHighTex(midHighTex);
		}
	}
}
