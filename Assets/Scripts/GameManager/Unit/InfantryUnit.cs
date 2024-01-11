
using UnityEngine;

public class InfantryUnit : Unit
{
    // ������� ���������� ��������� ��� ������

    protected InfantryUnit()
    {
        health = 20;
        moveSpeed = 4f;
        attackRange = 1;
        damage = 2;
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

    public override void Move(Vector3 newPosition)
    {
        // ���������� ����������� ������
        agent.SetDestination(newPosition);
    }

    public override void Attack(Transform target, Transform attackUnit)
    {
        // ������ ����� �����, ����� ������� ��������� ������ ���������
        if (Vector3.Distance(attackUnit.transform.position, target.position) <= attackRange)
        {
            Debug.Log(Vector3.Distance(transform.position, target.position));
            Unit targetUnit = target.GetComponent<Unit>();
            state = State.Attack;
            if (targetUnit is MageUnit) // ������ ������ �����
            {
                targetUnit.TakeDamage(targetUnit, damage /2); // ������� ������� ����
            }
            else
            {
                targetUnit.TakeDamage(targetUnit, damage); // ����������� ����
            }
        }
    }
}
