using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SimpleSceneChange : MonoBehaviour
{
	void Start ()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void SceneSwitch(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
