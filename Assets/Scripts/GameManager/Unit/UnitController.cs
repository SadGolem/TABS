using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public EnemyUnit enemyUnit;
    public FriendlyUnit friendlyUnit;

    void Start()
    {
/*        // Найдем игровые объекты дружеского и вражеского юнитов
        GameObject enemyUnitObject = GameObject.Find("EnemyUnit");
        enemyUnit = enemyUnitObject.GetComponent<EnemyUnit>();

        GameObject friendlyUnitObject = GameObject.Find("FriendlyUnit");
        friendlyUnit = friendlyUnitObject.GetComponent<FriendlyUnit>();*/

        // Получим компоненты NavMeshAgent для управления перемещением по навмешу


    }
    private void Update()
    {
        NavMeshAgent enemyNavMeshAgent = enemyUnit.GetComponent<NavMeshAgent>();
        NavMeshAgent friendlyNavMeshAgent = friendlyUnit.GetComponent<NavMeshAgent>();
        // Направим вражеского юнита к позиции дружеского юнита
        enemyNavMeshAgent.SetDestination(friendlyUnit.transform.position);

        // Направим дружеского юнита к позиции вражеского юнита
        friendlyNavMeshAgent.SetDestination(enemyUnit.transform.position);
    }
}
