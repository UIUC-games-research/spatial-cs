using JetBrains.Annotations;
using UnityEngine;

public class Level : MonoBehaviour {
	public int number = 1;
	public int maxScore = 10;
	//score system
	public int score = 0;
	public static Level Current {
		get {
			return Global.NonNull(FindObjectOfType<Level>(), "Missing active GameController in scene.");
		}
	}
	[CanBeNull] public Recipe Recipe {
		get {
			if (number == 1) return Recipe.oneYellowCube;
			if (number == 2) return Recipe.twoYellowCubesTwoRedCubes;
			return null;
		}
	}
	public void NotePlayerScored () {
		++score;
		if (score >= maxScore) {
			var character = PlayerController.Current.character;
			if (number == 1) character.rocketJumpEnabled = true;
			if (number == 2) character.catapultEnabled = true;
			if (number == 3) character.bladeEnabled = true;
		}
	}
}
