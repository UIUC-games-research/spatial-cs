using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;


public class GameController : MonoBehaviour
{
    //score system
    private int score = 0;
    public int maxScore = 10;
    public int level = 1;
    //Returns static instance
    private static GameController myGameController;
    public static GameController Instance()
    {
        if (!myGameController)
        {
            if (!myGameController)
            {
                myGameController = FindObjectOfType(typeof(GameController)) as GameController;
                if (!myGameController)
                    Debug.LogError("There needs to be one active script, and there isn't any to be found.");
            }

        }
        return myGameController;
    }

    public int getLevel() {
        return level;
    }

    public void Score()
    {
        score++;
        UIController.DisplayInfo("Material Block: " + score.ToString() + "/" + maxScore.ToString());

        if (score >= maxScore)
        {
            switch (level) {
                case 1:
                    // TODO: Load Cons Level Here
                    EnableRocketJump(250);
                    break;

                case 2:
                    InputController.Instance().xEnable = true;
                    break;
                case 3:
                    break;
            }

        }
    }


    public void EnableRocketJump(float force)
    {
      
        RigidbodyFirstPersonController fps = Reference.Instance().FPS.GetComponent<RigidbodyFirstPersonController>();
        fps.movementSettings.JumpForce = force;
    }
    

}
