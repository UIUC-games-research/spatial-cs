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

		// Ensure selfRef exists.
		EnsureRefExists();

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
			if (ii.Value != null)
				ii.Value.SetActive(false);
		}
		yield return null;
		// Activate the scene we just loaded
		loadedScenes[sceneName].SetActive(true);
		currentSceneObject = loadedScenes[sceneName];

		// Give cursor back.
		//! This might actually cause problems in the future if we have a scene change which
		//! happens between two sections of the game in which the mouse is hidden. We'll see.
		//Cursor.visible = true;
		//Cursor.lockState = CursorLockMode.None;
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

	// This can be a good idea to save memory. Sometimes it may be worth unloading a scene completely,
	// Losing any progress in that scene since load. For example: Completed Construction-mode stages.
	public static void UnloadScene(string sceneName)
	{
		Destroy(loadedScenes[sceneName]);
		loadedScenes.Remove(sceneName);
	}

	// This is the function which is used to load a fresh area. Any in-progress construction will be lost.
	// That shouldn't be a problem, because there should never be any leftover parts, and all constructed items
	// will be finished. Spawn point for the player MUST be pre-determined and passed into this function.
	public static void LoadNewExplorationLevel(string sceneName, Vector3 spawnPos)
	{
		// Grab player objects, bring to scene root, set as nondelete.
		GameObject playerRefs = GameObject.Find("Player (Including All Menus)");
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		playerRefs.transform.SetParent(null);
		DontDestroyOnLoad(playerRefs);

		// Clear the loadedscenes dictionary.
		loadedScenes.Clear();

		// Then load the new scene and position the player.
		SceneManager.LoadScene(sceneName);
		player.transform.position = spawnPos;

		// And setup a new position tracking file for the new scene.
		SimpleData.CreateNewPositionFile(sceneName);
	}

	static void EnsureRefExists()
	{
		if (selfRef == null)
		{
			GameObject instance = Instantiate(Resources.Load("Prefabs/SceneManager") as GameObject);
			selfRef = instance.GetComponent<LoadUtils>();
		}
	}

	public static GameObject InstantiateParenter(GameObject toParent)
	{
		if (currentSceneObject != null)
		{
			toParent.transform.SetParent(currentSceneObject.transform);
		}
		return toParent;
	}

	public static GameObject IconParenter(GameObject toParent)
	{
		EnsureRefExists();
		toParent.transform.SetParent(selfRef.transform);
		return toParent;
	}
	
}
