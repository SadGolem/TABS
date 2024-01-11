
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum State
{
    Idle,
    Attack,
    WalkToPoint
}

public enum Team
{
    Enemy,
    Friend
}
public class Unit : MonoBehaviour
{
    [Header("Unit")]

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected new Collider collider;
    Vector3 targetPoint;
    [SerializeField] protected State state;
    [SerializeField] protected State Team;

    [SerializeField] protected float followDistance = 3f;
    [SerializeField] protected float attackDistance = 3f;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float attackRange = 3f;

    [SerializeField] protected float walkSpeed = 3f;
    [SerializeField] protected Transform currentTarget;

    [HideInInspector]
    public UnityEvent OnSelect;
    [HideInInspector]
    public UnityEvent OnUnselect;

    public int health = 10;
    public int damage = 1;


    private void Start()
    {
        agent.speed = moveSpeed;
    }

    void Update()
    {
        // Логика выбора врага и просчета лучшей атаки будет здесь
        if (currentTarget == null)
        {
            currentTarget = FindBestTarget();
            Debug.Log(currentTarget);
        }

        if (currentTarget != null)
        {
            // Нападаем на врага, если он в пределах атаки
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                Attack(currentTarget);
            }
            else
            {
                Move(currentTarget.position);
            }
        }
    }

    public virtual void Move(Vector3 newPosition)
    {
        agent.SetDestination(newPosition);
        state = State.WalkToPoint;
    }

    public virtual void TakeDamage(Unit target, int damage)
    {
        target.health -= damage;
        if (target.health <= 0)
        {
            Die(target);
        }
    }

    protected virtual void Die(Unit target)
    {
        if (target != null)
        {
            UnitManager.instance.UnregisterUnit(target);
            Destroy(target.gameObject);
        }
    }

    protected Transform FindBestTarget()
    {
        List<EnemyUnit> enemies = UnitManager.instance.GetEnemyUnits();
        Debug.Log(enemies, this);
        List<FriendlyUnit> friends = UnitManager.instance.GetFriendUnits();
        Debug.Log(friends, this);
        List<Unit> unitsToSearch = this is FriendlyUnit ? enemies.Cast<Unit>().ToList() : friends.Cast<Unit>().ToList();
        Debug.Log(unitsToSearch);
        float closestDistance = Mathf.Infinity;
        Debug.Log(closestDistance);
        Transform bestTarget = null;

        foreach (Unit unit in unitsToSearch)
        {
            float distanceToUnit = Vector3.Distance(transform.position, unit.transform.position);
            if (distanceToUnit < closestDistance)
            {
                closestDistance = distanceToUnit;
                bestTarget = unit.transform;
            }
        }
        Debug.Log(bestTarget);
        currentTarget = bestTarget;
        return bestTarget;
    }

    protected int GiveDamage()
    {
        return damage;
    }

    public virtual void Attack(Transform target)
    {
    }
}

