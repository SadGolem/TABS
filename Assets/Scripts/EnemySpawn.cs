
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyUnitPrefabs; // Массив префабов вражеских юнитов

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

            NavMeshHit hit;
            Vector3 randomPoint;

            // Генерируем рандомную точку на навмеше для спавна юнита
            do
            {
                randomPoint = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)); // Рандомная точка в заданных пределах
            } while (!NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)); // Проверка, что точка доступна для спавна

            // Создаем экземпляр вражеского юнита на найденной точке на навмеше
            var prefab = Instantiate(randomEnemyUnitPrefab, hit.position, Quaternion.identity);
            UnitManager.instance.RegisterUnit(prefab.GetComponent<Unit>());
            Debug.Log("юнит врагов добавлен");
        }
        else
        {
            Debug.LogError("Массив префабов вражеских юнитов пуст или навигационная поверхность не установлена.");
        }
    }
}
