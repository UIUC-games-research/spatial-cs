using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextWave : MonoBehaviour
{
	float timeOffset = 0f;
	float wavePower = 1f;
	float waveSpeed = 4f;
	float offsetFixer = 8f;

	Vector2 initialPos;


	// Use this for initialization
	void Start ()
	{
		initialPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		// Need to add 8 to the y position (Half of the height.)
		transform.localPosition = new Vector2(initialPos.x, initialPos.y + offsetFixer + (wavePower * Mathf.Sin(waveSpeed * (Time.time + timeOffset))));

		//transform.localScale = new Vector2(transform.localScale.x * 1.01f, transform.localScale.y * 1.01f);
	}

	public void SetVars(float timeOffset_, float wavePower_, float waveSpeed_, float offsetFixer_)
	{
		timeOffset = timeOffset_;
		wavePower = wavePower_;
		waveSpeed = waveSpeed_;
		offsetFixer = offsetFixer_;
	}

}
