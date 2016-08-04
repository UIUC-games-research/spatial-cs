using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public class SimpleData : MonoBehaviour
{
	// Writing
	public float dataInterval = 1f;
	float timer = 0f;
	StreamWriter sw;

	// Reading
	public string fileToLoad;
	StreamReader sr;
	List<Vector3> points = new List<Vector3>();

	void Start ()
	{
		sw = File.CreateText("exp_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".txt");
	}
	
	void Update ()
	{
		// Press minus to load and display data.
		
		if (Input.GetKeyDown(KeyCode.Minus))
		{
			LoadData();
		}
		

		timer += Time.deltaTime;
		if (timer >= dataInterval)
		{
			RecordDataPoint();
			timer = 0f;
		}
	}

	void RecordDataPoint()
	{
		sw.WriteLine(transform.position.x + "|" + transform.position.y + "|" + transform.position.z);
	}

	void LoadData()
	{
		// Clear stuff.
		points.Clear();

		// Read all lines.
		List<string> lines = new List<string>();
		sr = new StreamReader(fileToLoad);
		while (!sr.EndOfStream)
		{
			lines.Add(sr.ReadLine());
		}
		Debug.Log(lines.Count);

		// Convert lines to vectors and add to list.
		foreach (string ss in lines)
		{
			string[] axes = ss.Split(new char[] { '|' });
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
		sw.Close();
	}
}
