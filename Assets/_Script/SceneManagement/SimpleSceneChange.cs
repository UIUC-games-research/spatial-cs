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

	// Specifically for skipping tutorial 1 and 2.
	// Format of sceneNames: loaded,unloaded
	public void ConstructionSwitch(string sceneNames)
	{
		string[] scenes = sceneNames.Split(new char[] { ',' });

		// Add correct tokens for tutorials.
		/*
		if (scenes[1] == "tutorial1")
		{
			ConversationTrigger.AddToken("done_with_tutorial_1");
		}
		if (scenes[1] == "tutorial2")
		{
			ConversationTrigger.AddToken("done_with_tutorial_2");
		}
		*/
		// Load new scene, unload old scene.
		//LoadUtils.UnloadScene(scenes[1]);

		//LoadUtils.LoadScene(scenes[0]);
		StartCoroutine(loader(scenes[1], scenes[0]));
	}

	IEnumerator loader(string unload, string load)
	{
		LoadUtils.LoadScene(load);
		yield return null;
		yield return null;
		LoadUtils.UnloadScene(unload);
	}
}
