using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	public AudioSource song;
	public AudioClip[] myMusic;

	void Awake() {
		song.clip = myMusic[0] as AudioClip;
	}

	// Use this for initialization
	void Start () {
		song.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!song.isPlaying) {
			playRandomMusic ();
		}

	}

	void playRandomMusic() {
		song.clip = myMusic [Random.Range (0, myMusic.Length)] as AudioClip;
		song.Play ();
	}
}
