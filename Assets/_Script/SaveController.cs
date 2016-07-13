using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public class SaveController : MonoBehaviour
{
	// Saves a file containing all game options which are in the above. 
	public static void Save()
	{
		// Prepare for data IO.
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(WhereIsData());

		// Create and write to a new container.
		SaveContainer data = new SaveContainer();

		//! Fields go here.
		data.rocketBoots = RocketBoots.GetBootsActive();
		

		// Save and close safely.
		bf.Serialize(file, data);
		file.Close();

		Debug.Log("Saved all options successfully.");
	}

	// Applies all options from the saved file to the locally created variables.
	public static void Load()
	{
		if (File.Exists(WhereIsData()))
		{
			// Prepare for data IO.
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(WhereIsData(), FileMode.Open);

			// Deserialize the data.
			SaveContainer data = (SaveContainer)bf.Deserialize(file);
			file.Close();

			//! Fields go here.
			if (data.rocketBoots)
				RocketBoots.ActivateBoots();

			Debug.Log("Loaded all options successfully.");
		}
		else
		{
			// No file exists? We should make the default one!
			Debug.Log("No save found, creating default file.");
			Save();
		}
	}

	// Returns the path to the options file.
	public static string WhereIsData()
	{
		return Application.persistentDataPath + "/Save.dat";
	}

}

[Serializable]
class SaveContainer
{
	//! Fields go here.
	public bool rocketBoots = false;
}