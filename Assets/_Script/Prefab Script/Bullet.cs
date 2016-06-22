using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    Rigidbody rb;
    // Use this for initialization
    void Awake () {
        rb = this.gameObject.GetComponent<Rigidbody>();
        
    }

    void OnEnable() {
        Vector3 position = Reference.Instance().FPS.transform.position;
        this.gameObject.transform.position = new Vector3(position.x, position.y+4, position.z);
        StartCoroutine(Disable(3.0f));
        rb.AddForce(Camera.main.transform.forward * 1000);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Disable(float second) {
        yield return new WaitForSeconds(second);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
