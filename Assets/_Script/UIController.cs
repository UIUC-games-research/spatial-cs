using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{
    public static void DisplayWarning(string msg)
    {
        Reference.Instance().middle.text = msg;
    }
	public static void DisplayInfo(string msg)
	{
		Reference.Instance().topLeft.text = msg;
	}
}
