using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[UsedImplicitly] public class TelePort : MonoBehaviour {
	[UsedImplicitly] public void Start () {}
	[UsedImplicitly] public void Update () {}
	[UsedImplicitly] public void OnTriggerEnter (Collider other) {SceneManager.LoadScene("Level2");}
}
