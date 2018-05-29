using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// This Timer class is used to track how long players spend in explortation levels.
// The tracking for construction levels is done separately, and is in FuseEvent.cs.

public class SceneTimer : MonoBehaviour
{
	// Set in inspector when placing this object into scenes.
	public string sceneName;
	float timer = 0f;
	Transform playerPos;
	public static bool highland = false;
	void Start(){
		playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	void Update()
	{
		timer += Time.deltaTime;
		//enter highland
        // TODO: change this so that separation is more definition
		if (!highland) {
			if (sceneName == "Canyon2" && playerPos.position.y >= 33) {
				//Debug.Log("here");
				SimpleData.WriteDataPoint("Left_Scene", "", "", "", "", "");
				//SimpleData.WriteStringToFile ("TimeSpent.txt", Time.time + ",TIMESPENT_INLEVEL," + sceneName + "," + timer);
				timer = 0f;
				highland = true;
			}
		}
	}

	void OnDisable()
	{
		// Must spend at least one second in a level, prevents a bit of log spam from the scene juggling.
		if (timer > 1f)
		{
			SimpleData.WriteDataPoint("Left_Scene", "", "", "", "", "");
			//SimpleData.WriteStringToFile ("TimeSpent.txt", Time.time + ",TIMESPENT_INLEVEL," + sceneName + "," + timer);
			// If the next line is commented, system is additive, and will print the
			// total time spent in a level every time the object is disabled.
			timer = 0f;
		}
	}


		


}
