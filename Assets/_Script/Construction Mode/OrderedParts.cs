using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OrderedParts : MonoBehaviour
{
	// The order which parts are created. Set in Inspector.
	public Button[] ordering;
	int currentIndex = 0;

	public void NextPart ()
	{
		ordering[currentIndex].onClick.Invoke();
		currentIndex++;
	}
}
