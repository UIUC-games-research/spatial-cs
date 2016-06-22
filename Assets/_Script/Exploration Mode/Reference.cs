using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Reference : MonoBehaviour {
    //This class serves as a static refernece to important Objects;
    private static Reference current;


    //UI
    public Text topLeft;
    public Text middle;
   
    public GameObject FPS;
    public GameObject blade;

	// Player
	public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController player;





	public static Reference Instance()
    {
        if (!current)
        {
            if (!current)
            {
                current = FindObjectOfType(typeof(Reference)) as Reference;
                if (!current)
                    Debug.LogError("There needs to be one active script, and there isn't any to be found.");
            }

        }
        return current;

    }

}
