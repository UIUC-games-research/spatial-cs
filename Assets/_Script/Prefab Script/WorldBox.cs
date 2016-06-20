using JetBrains.Annotations;
using UnityEngine;

public class WorldBox : MonoBehaviour {
	[UsedImplicitly] public void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			UIController.DisplayWarning("I Can Not' Enter Toxic Land");
		}
	}
	[UsedImplicitly] public void OnTriggerExit (Collider other) {UIController.DisplayWarning("");}
}
