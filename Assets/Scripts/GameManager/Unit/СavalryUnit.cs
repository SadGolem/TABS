
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class �avalryUnit : Unit
{
    [SerializeField] float acceleration = 1f;

    private UnitManager unitManager;

    protected �avalryUnit()
    {
        health = 10;
        moveSpeed = 3;
        attackRange = 1;
        damage = 3;
    }

    void Update()
    {
        // ������ ������ ����� � �������� ������ ����� ����� �����
        if (currentTarget == null)
        {
            currentTarget = FindBestTarget();
            Debug.Log(currentTarget);
        }

        if (currentTarget != null)
        {
            // �������� �� �����, ���� �� � �������� �����
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                state = State.Attack;
                Attack(currentTarget, this.transform);
            }
            else
            {
                state = State.WalkToPoint;
                Move(currentTarget.position);
            }
        }
    }

    private void Start()
    {
        unitManager = UnitManager.instance;
    }

    public void Place()
    {

    }

    public override void Move(Vector3 newPosition)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.speed = moveSpeed; // ������������� �������� �����������
            agent.acceleration = acceleration; // ������������� ���������
            agent.SetDestination(newPosition);
            state = State.WalkToPoint;
        }
    }


    public override void Attack(Transform target, Transform attackUnit)
    {
        // ������ ����� �����, ����� ������� ��������� ������ ���������
        if (Vector3.Distance(attackUnit.transform.position, target.position) <= attackRange)
        {
            Debug.Log(Vector3.Distance(transform.position, target.position));
            Unit targetUnit = target.GetComponent<Unit>();
            state = State.Attack;
            if (targetUnit is MageUnit || targetUnit is HealerUnit || targetUnit is ArcherUnit) // ������������ ��� ������, ��������, ���������
            {
                targetUnit.TakeDamage(targetUnit, damage * 2); // ������� ������� ����
            }
            else if (targetUnit is SpearmanUnit) // ������ ������ ����������
            {
                targetUnit.TakeDamage(targetUnit, damage / 2); // ������� ���������� ����
            }
            else
            {
                targetUnit.TakeDamage(targetUnit, damage); // ����������� ����
            }
        }
        else
        {
            state = State.WalkToPoint;
            Move(target.position);
        }
    }



}
