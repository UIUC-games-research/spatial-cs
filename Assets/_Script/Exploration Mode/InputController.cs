using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
    //This controller manages the all the behaviour triggered by input that is not process by firstpersoncontroller
    //AKA non movement related inputs

    public bool xEnable = false;
    public bool vEnable = true;

    static public InputController current;
    public static InputController Instance()
    {
        if (!current)
        {
            if (!current)
            {
                current = FindObjectOfType(typeof(InputController)) as InputController;
                if (!current)
                    Debug.LogError("There needs to be one active script, and there isn't any to be found.");
            }

        }
        return current;
    }



    // Update is called once per frame
    void Update()
    {
        //Fires Catapull
        if (Input.GetKeyDown("x") && xEnable)
        {
            GameObject bullet = ObjectPooler.Instance().GetPooledObject(1);
            bullet.SetActive(true);
        }

        if (Input.GetKeyDown("v") && vEnable)
        {
            if (Reference.Instance().blade.activeSelf)
                Reference.Instance().blade.SetActive(false);
            else
                Reference.Instance().blade.SetActive(true);
        }

    }
}
