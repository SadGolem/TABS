
using System.Collections.Generic;
using System.Data;
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
    private int distanceForCheckWarField = 200;
    public bool IsNotGetBack { get { return _isNotGetBack; } set { _isNotGetBack = value; } }
    protected bool _isNotGetBack =false;
    protected bool getBackNOW = false;

    public static Unit instance;
    private void Start()
    {
        instance = this;
        agent.speed = moveSpeed;
        maxHP = health;
    }

    private void FixedUpdate()
    {
        CheckHealth();
        if (_isNotGetBack)
        {
            getBackNOW = false;
        }
    }

    public virtual void Move(Vector3 newPosition)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.SetDestination(newPosition);
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
            // ������� ������� ������, ����� �������� GameEndResult ����� ���� ��� ���������, ��� target ������ �� ������������
            DestroyImmediate(target.gameObject, true);
        }
        // ���������, ��� �� ������ ������� ������ �� target, � ������ ����� ����� �������� GameEndResult
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
            GetBack(bestTarget);
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

    public virtual void GetBack(Transform target)
    {
        if (target != null && !_isNotGetBack)
        {
            List<Unit> enemies = UnitManager.instance.GetEnemyUnits();
            List<Unit> friends = UnitManager.instance.GetFriendUnits();
            // ������� ���������� ������ � ��������� � ������������ �������
            int enemyCount = 0;
            int friendCount = 0;
            if (this.team == Team.Enemy)
            { 
                foreach (Unit unit in enemies)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team == this.team && distanceToUnit < distanceForCheckWarField)
                    {
                        friendCount++;
                    }
                }

                foreach (Unit unit in friends)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team != this.team && distanceToUnit < distanceForCheckWarField)
                    {                      
                        enemyCount++;
                    }
                }
            }
            else
            {
                foreach (Unit unit in friends)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team == this.team && distanceToUnit < distanceForCheckWarField)
                    {
                        friendCount++;
                    }
                }

                foreach (Unit unit in enemies)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team != this.team && distanceToUnit < distanceForCheckWarField)
                    {
                        enemyCount++;
                    }
                }
            }
            // ���� ������ ������, ��� ��������� � ������� � ��������� � ��������������� ������� �� ������
            if (enemyCount > friendCount)
            {
                getBackNOW = true;
                Vector3 retreatDirection = CalculateRetreatDirection(enemies);
                Vector3 safePosition = transform.position + retreatDirection;
                Move(safePosition); // ��������� � ���������� �������
            }
            else
                getBackNOW = false;
        }
    }

    private Vector3 CalculateRetreatDirection(List<Unit> enemies)
    {
        Vector3 retreatVector = Vector3.zero;
        foreach (Unit enemy in enemies)
        {
            retreatVector += (transform.position - enemy.transform.position).normalized;
        }
        retreatVector /= enemies.Count; // ��������� ������� �����������

        return retreatVector.normalized;
    }

    public void CheckHealth()
    {
        if (this.health <= 0)
        {
            Die(this);
        }
    }
}

