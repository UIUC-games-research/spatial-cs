using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTimer : MonoBehaviour
{
	// Set in inspector when placing this object into scenes.
	public string sceneName;
	float timer = 0f;

	void OnEnable()
	{
		timer = 0f;
	}

	void Update()
	{
		timer += Time.deltaTime;
	}

	void OnDisable()
	{
		// Must spend at least one second in a level, prevents a bit of log spam from the scene juggling.
		if (timer > 1f)
			SimpleData.WriteStringToFile("TimeSpent.txt", Time.time + ",TIMESPENT_INLEVEL," + sceneName + "," + timer);

		timer = 0f;
	}

}
