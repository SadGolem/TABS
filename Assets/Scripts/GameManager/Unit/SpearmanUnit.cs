using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanUnit : Unit
{
    // ������� ���������� ��������� ��� ������
    [SerializeField] private float delayBeforeAttack = 0.5f;

    protected SpearmanUnit()
    {
        health = 20;
        moveSpeed = 4f;
        attackRange = 2;
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
        if (this.state == State.WalkToPoint)
        {
            agent.SetDestination(newPosition);
            /*            state = State.WalkToPoint;*/
        }
    }
    public override void Attack(Transform target, Transform attackUnit)
    {
        StartCoroutine(DelayedAttack(target, attackUnit)); // ��������� �������� ��� ���������� ����� � ���������
    }

    private IEnumerator DelayedAttack(Transform target,Transform attackUnit)
    {
        yield return new WaitForSeconds(delayBeforeAttack); // ���� �������� ���������� �������

        // ������ ����� �����, ����� ������� ��������� ������ ���������
        if (target != null)
        {
            if (Vector3.Distance(attackUnit.position, target.position) <= attackRange)
            {
                Debug.Log(Vector3.Distance(transform.position, target.position));
                Unit targetUnit = target.GetComponent<Unit>();
                state = State.Attack;
                if (targetUnit is MageUnit) // ������ ������ �����
                {
                    this.TakeDamage(targetUnit, damage / 2); // ������� ���������� ����
                }
                else
                {
                    this.TakeDamage(targetUnit, damage); // ����������� ����
                }
            }
        }

    }

}
