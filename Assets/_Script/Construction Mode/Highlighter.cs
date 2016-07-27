using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Highlighter : MonoBehaviour
{
	// Static functions.
	// Call these on any object which has an Image or MeshRenderer attached, and it will flash.
	public static void Highlight(GameObject go)
	{
		if (go.GetComponent<Highlighter>() == null)
			go.AddComponent<Highlighter>();
	}

	public static void Unhighlight(GameObject go)
	{
		Destroy(go.GetComponent<Highlighter>());
	}

	public void HighlightTimed(GameObject go, float time) {
		Highlight(go);
		GameObject eventsystem = GameObject.Find("EventSystem");
		eventsystem.GetComponent<FuseEvent>().StartCoroutine(wait_unhighlight_timed(go, time));
	}

	IEnumerator wait_unhighlight_timed(GameObject go, float time) {
		yield return new WaitForSeconds(time);
		Unhighlight(go);
	}
		
	// When this script is applied to an object, sine waves!
	MeshRenderer meshr;
	Color defaultColor;
	Color defaultEmiss;

	Image img;
	Color defaultImg;
	void Start ()
	{
		meshr = GetComponent<MeshRenderer>();
		if (meshr != null)
		{
			defaultColor = meshr.material.GetColor("_Color");
			defaultEmiss = meshr.material.GetColor("_EmissionColor");
		}

		img = GetComponent<Image>();
		if (img != null)
		{
			defaultImg = img.color;
		}
	}

	void FixedUpdate()
	{
		float oscillate = 0.5f * (1 + Mathf.Sin(10f * Time.time));
		Color oscolor = new Color(oscillate, oscillate, oscillate);
		if (meshr != null)
		{
			meshr.material.SetColor("_Color", oscolor);
			meshr.material.SetColor("_EmissionColor", oscolor);
		}
		if (img != null)
		{
			img.color = oscolor;
		}
	}

	void OnDestroy()
	{
		if (meshr != null)
		{
			meshr.material.SetColor("_Color", defaultColor);
			meshr.material.SetColor("_EmissionColor", defaultEmiss);
		}
		if (img != null)
		{
			img.color = defaultImg;
		}
	}
}
