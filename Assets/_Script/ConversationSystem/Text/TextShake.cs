using UnityEngine;
using System.Collections;

public class TextShake : MonoBehaviour
{
	float shakePower = 1f;
	float offsetFixer = 8f;

	bool shaker = false;
	Vector2 initialPos;
	



	void Start()
	{
		initialPos =  new Vector2(transform.localPosition.x, transform.localPosition.y + offsetFixer);
	}


	void FixedUpdate ()
	{
		if (shaker)
		{
			shaker = false;
			transform.localPosition = initialPos;
		}
		else
		{
			shaker = true;
			transform.localPosition = new Vector3(initialPos.x + Random.Range(-1f * shakePower, 1f * shakePower), initialPos.y + Random.Range(-1f * shakePower, 1f * shakePower));
		}
	}

	public void SetVars(float shakePower_, float offsetFixer_)
	{
		shakePower = shakePower_;
		offsetFixer = offsetFixer_;
	}
}
