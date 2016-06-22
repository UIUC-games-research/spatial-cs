using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuseAttributes {
	// dictionary of attachment names : correct fuse rotations
	private Dictionary<string, Quaternion> fuseRotations; 

	//dictionary of attachment names : acceptable rotations
	private Dictionary<string, Quaternion[]> acceptablePositions;

	//dictionary of attachment names: correct fuse positions
	private Dictionary<string, Vector3> fusePositions;


	public FuseAttributes(Dictionary<string, Vector3> positions, Dictionary<string, Quaternion> rotations, Dictionary<string, Quaternion[]> acceptableR) {
		fuseRotations = rotations;
		fusePositions = positions;
		acceptablePositions = acceptableR;
	}
	
	public Vector3 getFuseLocation(string attachment) {
		return fusePositions[attachment];
	}
	
	public Quaternion getFuseRotation(string attachment) {
		return fuseRotations[attachment];
	}

	public Quaternion[] getAcceptableRotations(string s) {
		return acceptablePositions[s];
	}

	//pairings of all places the selected object can be fused to
	public string[] getAcceptableLocations() {
		Dictionary<string, Quaternion[]>.KeyCollection keys = acceptablePositions.Keys;
		string[] locations = new string[keys.Count];
		keys.CopyTo(locations, 0);
		return locations;
	}


}

