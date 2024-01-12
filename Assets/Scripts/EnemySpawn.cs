
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyUnitPrefabs; // Массив префабов вражеских юнитов
    public Terrain terrain;
    public float offset = 5f;

    public static EnemySpawn Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void SpawnEnemyUnit()
    {
        if (enemyUnitPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, enemyUnitPrefabs.Length);
            GameObject randomEnemyUnitPrefab = enemyUnitPrefabs[randomIndex];


            // Генерируем рандомную точку на навмеше для спавна юнита
            float x = Random.Range(terrain.terrainData.size.x / 2 - terrain.terrainData.size.x / 16, terrain.terrainData.size.x / 2 + terrain.terrainData.size.x / 16);
            float z = Random.Range(terrain.terrainData.size.z / 2 + terrain.terrainData.size.z / 4, terrain.terrainData.size.z - terrain.terrainData.size.z / 16);
            float y = terrain.SampleHeight(new Vector3(x, 0, z)) + terrain.transform.position.y + offset;

            GameObject prefabToPlace = enemyUnitPrefabs[Random.Range(0, enemyUnitPrefabs.Length)];


            GameObject newPrefab = Instantiate(prefabToPlace, new Vector3(x, y, z), Quaternion.identity);
            newPrefab.transform.SetParent(this.transform); 
            UnitManager.instance.RegisterUnit(newPrefab.GetComponent<Unit>());
            Debug.Log("юнит врагов добавлен");
        }
        else
        {
            Debug.LogError("Массив префабов вражеских юнитов пуст или навигационная поверхность не установлена.");
        }
    }
}
