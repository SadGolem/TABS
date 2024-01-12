
using System.Collections;
using UnityEngine;

public class InfantryUnit : Unit
{
    [SerializeField] private float delayBeforeAttack = 2f;
    // ������� ���������� ��������� ��� ������

    protected InfantryUnit()
    {
        health = 30;
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
                currentTarget = null;
            }
            else
            {
                state = State.WalkToPoint;
                Move(currentTarget.position);
            }
        }
    }


    public override void Attack(Transform target, Transform attackUnit)
    {
        StartCoroutine(PerformDelayedAttack(target)); // ��������� �������� ��� ���������� ����� � ���������
    }

    private IEnumerator PerformDelayedAttack(Transform target)
    {
        yield return new WaitForSeconds(delayBeforeAttack); // ���� �������� ���������� �������

        Unit unit = target.GetComponent<Unit>();
        state = State.Attack;
        this.TakeDamage(unit, damage); // ������� ���� ����
    }

    public override void Move(Vector3 newPosition)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.SetDestination(newPosition);
            /*            state = State.WalkToPoint;*/
        }
    }
}
