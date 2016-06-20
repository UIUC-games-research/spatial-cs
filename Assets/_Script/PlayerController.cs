using JetBrains.Annotations;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

///<summary>This controls all aspects of the player character but movement.</summary>
[UsedImplicitly] public class PlayerController : MonoBehaviour {
	[UsedImplicitly] public void Start () {character = Game.character;}
	const string toggleInventoryHotkey = "i";
	HeadsUpDisplayState displayState = new HeadsUpDisplayState();
	public PlayerCharacter character;
	// ReSharper disable once InconsistentNaming
	[UsedImplicitly] public void OnGUI () {
		DrawToggleInventoryPrompt();
		character.DrawHeadsUpDisplay(displayState);
	}
	void DrawToggleInventoryPrompt () {
		GUI.Label(new Rect(Screen.width - 10 - 200, 10, 200, 200), "PRESS \"" + toggleInventoryHotkey.ToUpper() + "\" TO TOGGLE THE INVENTORY");
	}
	void ToggleInventory () {displayState.inventoryShown = !displayState.inventoryShown;}
	public static PlayerController Current {get {return Global.NonNull(FindObjectOfType<PlayerController>(), "Missing active PlayerController in scene.");}}
	[UsedImplicitly] public void Update () {
		character.recipeInMind = Level.Current.Recipe;
		Configure(GetComponent<RigidbodyFirstPersonController>());//Continuously project these settings there, so that the model is the authoritative source of state.
		AcceptInput();
	}
	void AcceptInput () {
		if (Input.GetKeyDown(toggleInventoryHotkey)) ToggleInventory();
		character.AcceptInput();
	}
	void Configure (RigidbodyFirstPersonController fpsController) {
		fpsController.movementSettings.JumpForce = character.JumpForce;
	}
	public void Collect (Item item) {
		Level.Current.NotePlayerScored();
		character.Collect(item);
	}
}
