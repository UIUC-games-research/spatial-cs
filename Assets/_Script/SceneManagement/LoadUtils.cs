using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LoadUtils : MonoBehaviour
{
	// Store static reference to controller.
	static LoadUtils selfRef;

	// Store root object of currently loaded scene.
	static GameObject currentSceneObject;

	// Stores all scenes which have been loaded. Added to in BundleScene whenever a new bundle is created.
	public static Dictionary<string, GameObject> loadedScenes = new Dictionary<string, GameObject>();

	void Awake()
	{
		selfRef = this;
	}

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
			if (tt.parent == null && gg.tag != "SceneBundle" && gg.tag != "SceneManager")
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

	// Since the load function requires a coroutine, it's nice to have a wrapper
	// like this to just call. We could probably do the bundle and get the current
	// scene ref inside the coroutine, but this is fine.
	public static void LoadScene(string sceneName)
	{
		// Ensure scene is already bundled properly.
		BundleScene(SceneManager.GetActiveScene().name);

		// Get scene ref.
		Scene current = SceneManager.GetActiveScene();

		// If the scene is already loaded, just switch to it.
		if (loadedScenes.ContainsKey(sceneName))
		{
			Debug.Log("Scene already loaded, going back to that one.");
			selfRef.StartCoroutine(Switcher(sceneName));
		}
		else
		{
			// Load the scene we are trying to merge.
			selfRef.StartCoroutine(Loader(sceneName, current));
		}
	}

	// Coroutine for loading scenes the silly-but-useful way.
	static IEnumerator Loader(string sceneName, Scene current)
	{
		// Load.
		SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
		
		// Wait for it to load.
		yield return null;

		// Merge.
		Scene toLoad = SceneManager.GetSceneByName(sceneName);
		SceneManager.MergeScenes(toLoad, current);

		// Bundle.
		BundleScene(sceneName);

		// Disable all scenes for a frame to prevent unique objects from yelling.
		// EventSystems, AudioListeners, and the like. EventSystems are the main problem,
		// since they are prone to disabling themselves. Jerks.
		foreach(KeyValuePair<string, GameObject> ii in loadedScenes)
		{
			ii.Value.SetActive(false);
		}
		yield return null;
		// Activate the scene we just loaded
		loadedScenes[sceneName].SetActive(true);
		currentSceneObject = loadedScenes[sceneName];

		// Give cursor back.
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	static IEnumerator Switcher(string sceneName)
	{
		// Disable all for safety.
		foreach (KeyValuePair<string, GameObject> ii in loadedScenes)
		{
			ii.Value.SetActive(false);
		}
		yield return null;
		// Activate the scene we want.
		loadedScenes[sceneName].SetActive(true);
		currentSceneObject = loadedScenes[sceneName];
	}

	public static GameObject InstantiateParenter(GameObject toParent)
	{
		toParent.transform.SetParent(currentSceneObject.transform);
		return toParent;
	}
	
}
