using System.Collections.Generic;
using UnityEngine;

public class ObjectScatterer : MonoBehaviour {
	public float limitY = 8;
	public float maxX = 400;
	public float maxYOffSet = 2;
	public float maxZ = 400;
	//Default parameter for the scatterer. X and Z defines the area on the terain object will be scattered. 
	//limitY defines the max height after which the object won't spwan. This is here to prevent scaterring objects to unreachable places. 
	//Offsets defines the distance away from terrian when a object spawn.
	public float minX = 150;
	public float minYOffSet = 1;
	public float minZ = 150;
	public List<int> scatterCount;

	//List of object that will be scattered
	public List<int> scatterObjIndexList;
	void Start () {
		for (var i = 0; i < scatterObjIndexList.Count; i++) {
			for (var j = 0; j < scatterCount[i]; j++) {
				var temp = ObjectPooler.Instance().GetPooledObject(scatterObjIndexList[i]);
				temp.transform.position = RandomPosition(minX, maxX, minZ, maxZ, limitY, minYOffSet, maxYOffSet);
				temp.SetActive(true);
			}
		}
	}
	public Vector3 ReturnRandomPosition () {return RandomPosition(minX, maxX, minZ, maxZ, limitY, minYOffSet, maxYOffSet);}
	static Vector3 RandomPosition (float minX, float maxX, float minZ, float maxZ, float limitY, float minYOffSet, float maxYOffSet) {
		float x, y, z;
		x = Random.Range(minX, maxX);
		z = Random.Range(minZ, maxZ);
		y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0.0f, z));
		for (var i = 1; i < 5000; i++) {
			x = Random.Range(minX, maxX);
			z = Random.Range(minZ, maxZ);
			y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0.0f, z));
			if (y < limitY) {
				return new Vector3(x, y + Random.Range(minYOffSet, maxYOffSet), z);
			}
		}
		return new Vector3(x, y + Random.Range(minYOffSet, maxYOffSet), z);
	}
}
