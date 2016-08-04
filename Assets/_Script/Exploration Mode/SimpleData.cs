using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public class SimpleData : MonoBehaviour
{
	// This script relies on the fact that the player object is never deleted.
	// Don't delete the player object, please!
	// Also relies on the fact that the player is activated before any construction scenes are,
	// This will likely never change, as the game will always begin in exploration mode.

	// Folder name for this session. Set in Awake using system time.
	static string folder;

	// Writing
	public float dataInterval = 1f;
	static float timer = 0f;
	static StreamWriter sw_Position;

	// Reading
	public string fileToLoad;
	static StreamReader sr_Position;
	static List<Vector3> points = new List<Vector3>();

	void Awake ()
	{
		// Create the initial folder for this session.
		folder = Application.persistentDataPath + "/" + "Player_at_time_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
		Directory.CreateDirectory(folder);

		// Create initial position tracking file.
		sw_Position = File.CreateText(folder + "/" + "PositionData_" + SceneManager.GetActiveScene().name + ".txt");
	}

	// Called when entering new exploration scenes to keep the data separate.
	public static void CreateNewPositionFile()
	{
		sw_Position.Close();
		sw_Position = File.CreateText(folder + "/" + "PositionData_" + SceneManager.GetActiveScene().name + ".txt");
	}
	
	void Update ()
	{
		// Press minus to load and display data.		
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			LoadData();
		}
		
		// Timer for recording positional data points.
		timer += Time.deltaTime;
		if (timer >= dataInterval)
		{
			sw_Position.WriteLine(transform.position.x + "," + transform.position.y + "," + transform.position.z);
			timer = 0f;
		}
	}

	void LoadData()
	{
		// Clear stuff.
		points.Clear();

		// Read all lines.
		List<string> lines = new List<string>();
		sr_Position = new StreamReader(Application.persistentDataPath + "/" + fileToLoad);
		while (!sr_Position.EndOfStream)
		{
			lines.Add(sr_Position.ReadLine());
		}
		Debug.Log(lines.Count);

		// Convert lines to vectors and add to list.
		foreach (string ss in lines)
		{
			string[] axes = ss.Split(new char[] { '|', ',' });
			Vector3 point = new Vector3(float.Parse(axes[0]), float.Parse(axes[1]), float.Parse(axes[2]));
			points.Add(point);
		}
		Debug.Log(points.Count);

		// Visualize points.
		for (int i = 0; i < points.Count - 1; i++)
		{
			Color theColor = new Color((float)i / points.Count, 0f, 0f);
			Debug.DrawLine(points[i], points[i + 1], theColor, 99999f);
		}
	}

	void OnDestroy ()
	{
		sw_Position.Close();
	}

	// Static functions for easy data storage.
	// Here's a catch-all! Appends a line to a file by name.
	public static void WriteStringToFile(string filename, string toWrite)
	{
		StreamWriter sw_temp = new StreamWriter(folder + "/" + filename, true);
		sw_temp.WriteLine(toWrite);
		sw_temp.Close();
	}
}
