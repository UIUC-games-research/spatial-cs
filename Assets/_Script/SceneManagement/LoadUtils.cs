using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LoadUtils : MonoBehaviour
{
	// Stores all scenes which have been loaded. Added to in BundleScene whenever a new bundle is created.
	public static Dictionary<string, GameObject> loadedScenes = new Dictionary<string, GameObject>();

	// Bundles a scene so it is entirely parented under an empty GameObject named
	// the same way as the scene. This should be called when loading scenes to ensure
	// everything stays bundled.
	public static void BundleScene(string sceneName)
	{
		// Create empty dummy, tagged as "SceneBundle"
		if (loadedScenes.ContainsKey(sceneName))
		{
			return;
		}
		GameObject sceneBundle = new GameObject(sceneName);
		loadedScenes.Add(sceneName, sceneBundle);
		sceneBundle.tag = "SceneBundle";

		// Parent all root objects to it.
		object[] objs = FindObjectsOfType(typeof(GameObject));
		foreach (object ii in objs)
		{
			GameObject gg = (GameObject)ii;
			Transform tt = gg.transform;
			if (tt.parent == null && gg.tag != "SceneBundle")
			{
				tt.parent = sceneBundle.transform;
			}
		}

		// If nothing got parented for some reason, delete it.
		if (sceneBundle.transform.childCount == 0)
		{
			Destroy(sceneBundle);
		}
	}

	public static void LoadScene(string sceneName, MonoBehaviour caller)
	{
		// Ensure scene is already bundled.
		BundleScene(SceneManager.GetActiveScene().name);

		// Get scene ref.
		Scene current = SceneManager.GetActiveScene();

		// Load the scene we are trying to merge.
		Debug.Log("Running Function.");
		GameObject dummy = new GameObject();
		caller.StartCoroutine(Loader(sceneName,current));
	}

	static IEnumerator Loader(string sceneName, Scene current)
	{
		// Load.
		Debug.Log("Loading");
		SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		
		// Wait for it to load.
		yield return null;
		Debug.Log("Waited one frame");

		// Merge.
		Scene toLoad = SceneManager.GetSceneByName(sceneName);
		SceneManager.MergeScenes(toLoad, current);

		// Bundle.
		BundleScene(sceneName);

		// Deactivate current scene.
		loadedScenes[current.name].SetActive(false);

		// Give cursor back.
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
