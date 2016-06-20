using UnityEngine;
using UnityEngine.UI;

public class Reference : MonoBehaviour {
	//This class serves as a static refernece to important Objects;
	static Reference current;
	public GameObject blade;
	public GameObject FPS;
	public Text middle;
	
	public Texture2D itemDisplayBackdrop;
	public Texture2D yellowCubeIcon;
	public Texture2D redCubeIcon;
	public static Reference Instance () {
		if (!current) {
			if (!current) {
				current = FindObjectOfType(typeof(Reference)) as Reference;
				if (!current)
					Debug.LogError("There needs to be one active script, and there isn't any to be found.");
			}
		}
		return current;
	}
}
