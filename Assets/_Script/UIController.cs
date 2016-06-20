using UnityEngine;

public class UIController : MonoBehaviour {
	public static void DisplayWarning (string msg) {Reference.Instance().middle.text = msg;}
}
