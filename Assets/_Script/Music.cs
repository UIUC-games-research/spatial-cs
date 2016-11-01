using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	public AudioSource audio;
	public AudioClip[] myMusic;

	void Awake() {
		audio.clip = myMusic[0] as AudioClip;
	}

	// Use this for initialization
	void Start () {
		audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!audio.isPlaying) {
			playRandomMusic ();
		}

	}

	void playRandomMusic() {
		audio.clip = myMusic [Random.Range (0, myMusic.Length)] as AudioClip;
		audio.Play ();
	}
}
