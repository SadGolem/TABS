using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public EnemyUnit enemyUnit;
    public FriendlyUnit friendlyUnit;

    void Start()
    {
/*        // ������ ������� ������� ���������� � ���������� ������
        GameObject enemyUnitObject = GameObject.Find("EnemyUnit");
        enemyUnit = enemyUnitObject.GetComponent<EnemyUnit>();

        GameObject friendlyUnitObject = GameObject.Find("FriendlyUnit");
        friendlyUnit = friendlyUnitObject.GetComponent<FriendlyUnit>();*/

        // ������� ���������� NavMeshAgent ��� ���������� ������������ �� �������


    }
    private void Update()
    {
        NavMeshAgent enemyNavMeshAgent = enemyUnit.GetComponent<NavMeshAgent>();
        NavMeshAgent friendlyNavMeshAgent = friendlyUnit.GetComponent<NavMeshAgent>();
        // �������� ���������� ����� � ������� ���������� �����
        enemyNavMeshAgent.SetDestination(friendlyUnit.transform.position);

        // �������� ���������� ����� � ������� ���������� �����
        friendlyNavMeshAgent.SetDestination(enemyUnit.transform.position);
    }
}
