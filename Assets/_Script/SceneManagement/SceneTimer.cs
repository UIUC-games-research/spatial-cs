using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTimer : MonoBehaviour
{
	// Set in inspector when placing this object into scenes.
	public string sceneName;
	float timer = 0f;
	Transform playerPos;
	bool highland = false;
	void Start(){
		playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	void Update()
	{
		timer += Time.deltaTime;
		//enter highland
		if (!highland) {
			if (sceneName == "Canyon2" && playerPos.position.y >= 33) {
				Debug.Log("here");
				SimpleData.WriteStringToFile ("TimeSpent.txt", Time.time + ",TIMESPENT_INLEVEL," + sceneName + "," + timer);
				timer = 0f;
				highland = true;
			}
		}
	}

	void OnDisable()
	{
		// Separate write for highland
		if (sceneName == "Canyon2" && highland) {
			SimpleData.WriteStringToFile ("TimeSpent.txt", Time.time + ",TIMESPENT_INLEVEL," + "Highland" + "," + timer);
		}

		// Must spend at least one second in a level, prevents a bit of log spam from the scene juggling.
		else if (timer > 1f) {
			SimpleData.WriteStringToFile ("TimeSpent.txt", Time.time + ",TIMESPENT_INLEVEL," + sceneName + "," + timer);
			// If this next line is commented, system is additive, and will print the
			// total time spent in a level every time the object is disabled.
			//timer = 0f;
		}
	}


		


}
