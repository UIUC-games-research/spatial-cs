using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectScatterer : MonoBehaviour
{
    //Default parameter for the scatterer. X and Z defines the area on the terain object will be scattered. 
    //limitY defines the max height after which the object won't spwan. This is here to prevent scaterring objects to unreachable places. 
    //Offsets defines the distance away from terrian when a object spawn.
    public float minX = 150;
    public float maxX = 400;
    public float minZ = 150;
    public float maxZ = 400;
    public float limitY = 8;
    public float minYOffSet = 1;
    public float maxYOffSet = 2;

    //List of object that will be scattered
    public List<int> scatterObjIndexList;
    public List<int> scatterCount;


    void Start()
    {
        for (int i = 0; i < scatterObjIndexList.Count; i++)
        {
            for (int j = 0; j < scatterCount[i]; j++)
            {
                GameObject temp = ObjectPooler.Instance().GetPooledObject(scatterObjIndexList[i]);
                temp.transform.position = ObjectScatterer.RandomPosition(minX, maxX, minZ, maxZ, limitY, minYOffSet, maxYOffSet);
                temp.SetActive(true);
            }
        }
    }
    public Vector3 ReturnRandomPosition() {
        return RandomPosition(minX, maxX, minZ, maxZ, limitY, minYOffSet, maxYOffSet);
    }
    private static Vector3 RandomPosition(float minX, float maxX, float minZ, float maxZ, float limitY, float minYOffSet, float maxYOffSet)
    {
        float x, y, z;
        x = Random.Range(minX, maxX);
        z = Random.Range(minZ, maxZ);
        y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0.0f, z));

        for (int i = 1; i < 5000; i++)
        {
            x = Random.Range(minX, maxX);
            z = Random.Range(minZ, maxZ);
            y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0.0f, z));

            if (y < limitY)
            {
                return new Vector3(x, y + Random.Range(minYOffSet, maxYOffSet), z);
            }
        }
        return new Vector3(x, y + Random.Range(minYOffSet, maxYOffSet), z);
    }




}
