
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class �avalryUnit : Unit
{
    [SerializeField] float acceleration = 1f;
    [SerializeField] float confidenceThreshold = 0.6f; // Declaring the confidence attribute
    [SerializeField] float rangeBattleField = 50f;
    private UnitManager unitManager;

    List<Unit> unitsNearTheCavalry = new List<Unit>();
    private bool isConfidenceAttack = false;

    protected �avalryUnit()
    {
        health = 10;
        moveSpeed = 3;
        attackRange = 1;
        damage = 3;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckConfidence());
    }

    void Update()
    {
        // Logic for enemy selection and determining the best attack will be here
        if (currentTarget == null)
        {
            currentTarget = FindBestTarget();
            Debug.Log(currentTarget);
        }

        if (currentTarget != null)
        {
            // Attack the enemy if within range
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                state = State.Attack;
                // ��������� ����������� ����� ������
                if (isConfidenceAttack)
                {
                    Attack(currentTarget, this.transform);
                }
                else
                {
                    GetBack(currentTarget);
                }
                currentTarget = null;
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
    public override void GetBack(Transform target) //�����
    {
        Vector3 directionToTarget = transform.position - target.position;
        Vector3 newPosition = transform.position + directionToTarget.normalized;

        Move(newPosition);
    }

    public override void Attack(Transform target, Transform attackUnit)
    {
        StartCoroutine(AttackWithDelay(target, attackUnit));
    }

    private IEnumerator AttackWithDelay(Transform target, Transform attackUnit)
    {
        // ���� ��������� ����� ����� ������
        yield return new WaitForSeconds(2f);
        if (target != null)
        {
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
        else
            state = State.Idle;
    }

    private IEnumerator CheckConfidence()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            GetListAllUnits();
                // ������������ ����������� �� ������ ����� ��������������� �������
                int enemyCount = CalculateEnemyCount(); // ������������ ���������� ������, ������� �����������
                int allyCount = CalculateFriendCount();  // ������������ ���������� ����������� ���������
                float enemyAdvantage = CalculateEnemyAdvantage(enemyCount); // ������������ ������������ ����� (0.2 �� ������� �����)
                float selfAdvantage = CalculateSelfAdvantage(currentTarget); // ������������ ������������ ��� �������� ����� (0.2)

                float calculatedConfidence = 1 /*- (0.2f * enemyCount)*/ + (0.2f * allyCount) - enemyAdvantage + selfAdvantage;

                Debug.Log("������������ �����������: " + calculatedConfidence);

                // ��������� ������� � ����������� �� ������ �����������
                if (calculatedConfidence > confidenceThreshold)
                {
                    isConfidenceAttack = true;// ��������� �����
                }
                else if (calculatedConfidence == confidenceThreshold) { /* ����� �������� ��� 50/50 */ isConfidenceAttack = true; }
                else
                {
                    isConfidenceAttack = false; // ������������ ���� ��� ������� ������ ����
                }
            /*}*/
        }
    }

        

    private void GetListAllUnits()
    {
        if (unitsNearTheCavalry.Count > 0)
        {
            unitsNearTheCavalry.Clear();
        }
        // �������� ���� ���������� � ������� �����
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, rangeBattleField);
        foreach(Collider col in hitColliders)
        {
            if (!col.CompareTag("Terrain"))
            {
                Unit unit = col.GetComponent<Unit>();
                unitsNearTheCavalry.Add(unit);
            }
        }
    }
    int CalculateEnemyCount()
    {
        int enemyCount = 0;

        // ���������� ��������� ����������
        foreach (Unit unit in unitsNearTheCavalry)
        {
            // ���������, �������� �� ������ ������
            if (unit != null && unit.tag != this.tag )
            {
                enemyCount++;
            }
        }

        return enemyCount;
    }

    int CalculateFriendCount()
    {
        int friendsCount = 0;

        // ���������� ��������� ����������
        foreach (Unit unit in unitsNearTheCavalry)
        {
            // ���������, �������� �� ������ �������������
            if (unit != null && unit.tag == this.tag)
            {
                friendsCount++;
            }
        }

        return friendsCount;
    }

    float CalculateEnemyAdvantage(int enemyCount)
    {
        float enemyAdvantage = 0.0f;
        // ������������ ���������� ������ ������� ����
/*        int mageCount = 0;*/
        int healerCount = 0;
/*        int archerCount = 0;*/
        int spearmanCount = 0;
        int otherCount = 0;

        foreach (Unit unit in unitsNearTheCavalry)
        {
            if (unit != null && unit.tag != this.tag)
            {
                // ���������, �������� �� ������ ������ ������� ����
                if (unit is MageUnit)
                {
                    /*mageCount++;*/
                }
                else if (unit is HealerUnit && enemyCount > 1 ) // ������� ������ ������, ��� � �������
                {
                    healerCount++;
                }
                else if (unit is ArcherUnit)
                {
                   /* archerCount++;*/
                }
                else if (unit is SpearmanUnit)
                {
                    spearmanCount++;
                }
                else
                {
                    otherCount++;
                }
            }
        }

        // ������: ������� ����� ������������ ����� �� ������ ���������� ������ ������� ����
        enemyAdvantage = (float)(0.2f * otherCount + 0.4f * spearmanCount + 0.4 * healerCount);

        return enemyAdvantage;
    }


    float CalculateSelfAdvantage(Transform target)
    {
        float selfAdvantage = 0.0f;
        if (target != null)
        {
            Unit unit = target.transform.GetComponent<Unit>();

            if (unit != null)
            {
                if (unit is HealerUnit)
                {
                    selfAdvantage += 0.4f;
                }
                else if (unit is MageUnit)
                { selfAdvantage += 0.4f; }
                else if (unit is ArcherUnit) { selfAdvantage += 0.4f; }
                else { selfAdvantage += 0.2f; }

                // ������ ����������� ������������ �������� ����� ��� �������� �����
            }
        }
        return selfAdvantage;
    }
}
