using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextRandColor : MonoBehaviour
{
	Image imageComponent;
	float r = 1f;
	float g = 1f;
	float b = 1f;

	void Start()
	{
		imageComponent = GetComponent<Image>();
	}

	void FixedUpdate()
	{
		imageComponent.color = new Color(Random.Range(0f, r), Random.Range(0f, g), Random.Range(0f, b));
	}

	public void SetVars(float[] rgbWeights)
	{
		r = rgbWeights[0];
		g = rgbWeights[1];
		b = rgbWeights[2];
	}
}
