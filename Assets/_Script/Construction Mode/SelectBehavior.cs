using UnityEngine;
using System.Collections;

public class SelectBehavior : MonoBehaviour {

	public Texture unhighTex;
	public Texture highTex;
	void Awake() {

	}

	public void setUnhighTex(Texture tex) {
		unhighTex = tex;
	}

	public void setHighTex(Texture tex) {
		highTex = tex;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
