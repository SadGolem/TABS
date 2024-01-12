
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    /*public GameObject treePrefab;
    public GameObject bushPrefab;
    public GameObject rockPrefab;
    public int numberOfTrees;
    public int numberOfBushes;
    public int numberOfRocks;

    public static EnvironmentGenerator instance;
    void Awake()
    {
        instance = this;
    }

    public void GenerateEnvironment()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector3 randomPos = GenerateRandomPosition();
            Instantiate(treePrefab, randomPos, Quaternion.identity);
        }

        for (int i = 0; i < numberOfBushes; i++)
        {
            Vector3 randomPos = GenerateRandomPosition();
            Instantiate(bushPrefab, randomPos, Quaternion.identity);
        }

        for (int i = 0; i < numberOfRocks; i++)
        {
            Vector3 randomPos = GenerateRandomPosition();
            Instantiate(rockPrefab, randomPos, Quaternion.identity);
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        float x, z;
        bool positionFound = false;
        Vector3 randomPos = Vector3.zero;

        while (!positionFound)
        {
            x = Random.Range(-50f, 50f);
            z = Random.Range(-50f, 50f);

            randomPos = new Vector3(x, 0, z);

            
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(randomPos.x, 100, randomPos.z), Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag != "EnvironmentObject")
                {
                    positionFound = true; // Если луч не пересекается с объектом, значит, позиция свободна
                }
            }
        }

        return randomPos;
    }*/

}
