using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RandomTreeSpawner : MonoBehaviour
{
    public Terrain terrain;

    public TreeInstance tree;
    public int numberOfTrees = 1000;
    public float minHeightScale = 0.8f;  
    public float maxHeightScale = 1.2f;  
    public float treeSpacing = 10.0f;
    public bool isResetTree = true;

    void Start()
    {
        PlantTrees();
    }

    void PlantTrees()
    {
        if (isResetTree)
        {
            terrain.terrainData.treeInstances = new TreeInstance[0];
        }
        for (int i = 0; i < numberOfTrees; i++)
        {
            float x = Random.Range(0, terrain.terrainData.size.x);
            float z = Random.Range(0, terrain.terrainData.size.z);
            float y = terrain.SampleHeight(new Vector3(x, 0, z));

            float heightScale = Random.Range(minHeightScale, maxHeightScale);  
                                                                               
            if ((i == 0) || (Vector3.Distance(new Vector3(x, y, z), terrain.terrainData.GetTreeInstance(i - 1).position) > treeSpacing))
            {
                TreeInstance tree = new TreeInstance
                {
                    position = new Vector3(x / terrain.terrainData.size.x, y, z / terrain.terrainData.size.z),
                    prototypeIndex = (int)Random.Range(0, 7), 
                    widthScale = 1,
                    heightScale = heightScale,
                    color = Color.white,
                    lightmapColor = Color.white
                };

                terrain.AddTreeInstance(tree);
            }
            else
            {
                i--;
            }
        }
        terrain.terrainData.treeInstances = terrain.terrainData.treeInstances;
    }
    void Update()
    {

    }

}
