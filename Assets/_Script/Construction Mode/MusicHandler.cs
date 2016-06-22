using UnityEngine;
using System.Collections;

public class MusicHandler : MonoBehaviour {

	public AudioSource source;
	public AudioClip tutorialMusic;
	public AudioClip firstConstructionMusic;

	// Use this for initialization
	void Start () {
		source.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
