using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Bullet : MonoBehaviour {
	Rigidbody rb;
	[UsedImplicitly] public void Awake () {rb = gameObject.GetComponent<Rigidbody>();}
	[UsedImplicitly] public void OnEnable () {
		var position = Reference.Instance().FPS.transform.position;
		gameObject.transform.position = new Vector3(position.x, position.y + 4, position.z);
		StartCoroutine(Disable(3.0f));
		rb.AddForce(Camera.main.transform.forward * 1000);
	}
	[UsedImplicitly] public void Update () {}
	IEnumerator Disable (float second) {
		yield return new WaitForSeconds(second);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		gameObject.SetActive(false);
	}
}
