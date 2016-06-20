using JetBrains.Annotations;
using UnityEngine;

public class PickUp : MonoBehaviour {
	[UsedImplicitly] public bool respawn = true;
	[UsedImplicitly] public float respawnTime = 5.0f;
	[UsedImplicitly] public string itemTypeName = "YellowCube";//It's a string because Unity's reflective inspector doesn't handle abstract types nicely.
	Item item;
	[UsedImplicitly] public void Start () {item = (Item) Item.ConcreteTypeNamed(itemTypeName).New();}
	[UsedImplicitly] public void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			var playerController = other.GetComponent<PlayerController>();
			playerController.Collect(item);
			Level.Current.NotePlayerScored();
			if (respawn)
				ObjectRespawner.Instance().RespawnObj(gameObject, respawnTime);
			//todo if it doesn't respawn, there's nothing deleting it. Fix that.
		}
	}
}
