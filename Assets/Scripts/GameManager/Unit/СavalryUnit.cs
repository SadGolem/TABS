
using UnityEngine;
using UnityEngine.AI;

public class �avalryUnit : Unit
{
    /*[SerializeField] NavMeshAgent navMeshAgent;*/
    [SerializeField] Vector3 targetPoint;
    [SerializeField] Unit targetUnit;
    [SerializeField] float acceleration = 2f;

    private Transform currentTarget;

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
        }

        if (currentTarget != null)
        {
            // �������� �� �����, ���� �� � �������� �����
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                Attack(currentTarget);
            }
        }

        // ���������� �������� ������� ��� ������������
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    Transform FindBestTarget()
    {
        // ����� ����� ������ ������ ������� ����� (������ �����, �������, �������� � �������������� ����������)
        // ...
        return null; // ��������
    }

    public void Place()
    {

    }

    public override void Move(Vector3 newPosition)
    {
        // ���������� ����������� �������, ����������� �������� � ���������
        agent.acceleration = acceleration;
        agent.SetDestination(newPosition);
        state = State.WalkToPoint;
    }

    public override void TakeDamage(Unit target)
    {
        // ������ ����� ����� � ������ ����������� � �����������
        if (target is MageUnit || target is HealerUnit || target is ArcherUnit) // ������������ ��� ������, ��������, ���������
        {
            target.TakeDamage(target.damage * 2); // ������� ������� ����
        }
        else if (target is SpearmanUnit) // ������ ������ ����������
        {
            target.TakeDamage(damage / 2); // ������� ���������� ����
        }
        else
        {
            target.TakeDamage(damage); // ����������� ����
        }
    }


    void Attack(Transform target)
    {
        // ������ ����� �����
        // ...
    }
}
