
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnitSpawner : MonoBehaviour
{
    public GameObject[] enemyUnitPrefabs; // ������ �������� ��������� ������

    public static EnemyUnitSpawner Instance;
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

            // ���������� ��������� ����� �� ������� ��� ������ �����
            do
            {
                randomPoint = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)); // ��������� ����� � �������� ��������
            } while (!NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)); // ��������, ��� ����� �������� ��� ������

            // ������� ��������� ���������� ����� �� ��������� ����� �� �������
            Instantiate(randomEnemyUnitPrefab, hit.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("������ �������� ��������� ������ ���� ��� ������������� ����������� �� �����������.");
        }
    }
}
