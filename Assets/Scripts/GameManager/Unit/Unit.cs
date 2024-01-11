
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
    [SerializeField] public Team team;

    [SerializeField] protected float followDistance = 3f;
    [SerializeField] protected float attackDistance = 3f;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float attackRange = 3f;
    [HideInInspector] public float maxHP;

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
        maxHP = health;
    }

    private void Update()
    {
        CheckHealth();
    }

    public virtual void Move(Vector3 newPosition)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.SetDestination(newPosition);
/*            state = State.WalkToPoint;*/
        }
    }

    public virtual void TakeDamage(Unit target, int damage)
    {
        if (target.health <= 0)
        {
            
            Die(target);
        }
        else
            target.health -= damage;
    }

    protected virtual void Die(Unit target)
    {
        if (target != null)
        {
            UnitManager.instance.UnregisterUnit(target);
            UnitManager.instance.AddEnemyUnits();
            UnitManager.instance.AddFriendUnits();
            currentTarget = null;
            // Сначала удаляем объект, затем вызываем GameEndResult после того как убедились, что target больше не используется
            DestroyImmediate(target.gameObject, true);
        }
        // Проверяем, нет ли больше никаких ссылок на target, и только после этого вызываем GameEndResult
        GameManager.Instance.GameEndResult();
    }

protected Transform FindBestTarget()
    {
        List<Unit> enemies = UnitManager.instance.GetEnemyUnits();
        Debug.Log(enemies, this);
        List<Unit> friends = UnitManager.instance.GetFriendUnits();
        Debug.Log(friends, this);
        if (enemies.Count != 0 && friends.Count != 0)
        {
            List<Unit> unitsToSearch = team is Team.Friend ? enemies.Cast<Unit>().ToList() : friends.Cast<Unit>().ToList();
            Debug.Log(unitsToSearch);
            float closestDistance = Mathf.Infinity;
            Debug.Log(closestDistance);
            Transform bestTarget = null;

            foreach (Unit unit in unitsToSearch)
            {
                if (unit != null && unit != this)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (distanceToUnit < closestDistance)
                    {
                        closestDistance = distanceToUnit;
                        bestTarget = unit.transform;
                    }
                }
            }
            Debug.Log(bestTarget);
            currentTarget = bestTarget;
            return bestTarget;
        }
        else
        { 
            currentTarget = null;
            GameManager.Instance.GameEndResult();
            return null;
        }
    }

    protected int GiveDamage()
    {
        return damage;
    }

    public virtual void Attack(Transform target, Transform attackUnit)
    {
    }

    public void CheckHealth()
    {
        if (this.health <= 0)
        {
            Die(this);
        }
    }
}

