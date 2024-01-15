using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealerUnit : Unit
{
    public int heal;
    [SerializeField] private float delayBeforeHeal = 2f;
    private Transform enemiesTarget;
    private int distanceForCheckWarFieldForHealer = 200;
    protected HealerUnit()
    {
        health = 20;
        moveSpeed = 2.0f;
        attackRange = 2.0f;
        // � ������� ��� ������� �����, ������� ���� �� �������������
        damage = 0;
    }

    private void Update()
    {
        // ������ ������ ���� � ������� ����� �����
        Transform target = FindBestHealTarget();
        enemiesTarget = FindBestTarget();
        if (target != null)
        {
            if (getBackNOW)
            {
                state = State.WalkToPoint;
                GetBack(enemiesTarget);
                return;
            }
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                state = State.Attack;
                Heal(target);
            }
            else
            {
                state = State.WalkToPoint;
                Move(target.position);
            }
        }
    }

    // ������ ������ ���� ��� �������
    protected Transform FindBestHealTarget()
    {
        List<Unit> enemies = UnitManager.instance.GetEnemyUnits(); // �������� ���������� ������
        List<Unit> friends = UnitManager.instance.GetFriendUnits(); // �������� ������������� ������

        List<Unit> unitsToSearch = team is Team.Friend ? friends : enemies; // ����������� ������ ��� ������

        float minHealth = Mathf.Infinity; // ��������� �������� ��� ������ ������������ ��������
        Transform bestTarget = null; // ���� � ���������� ���������
        foreach (Unit unit in unitsToSearch) // �������� �� ���� ������
        {
            if (unit != null && unit != this) // ���������, ��� ���� ���������� � �� �������� ������� ������
            {
                if (unit.health < minHealth) // ���� �������� ����� ������ �������� ������������ ��������
                {
                    minHealth = unit.health; // ��������� ����������� ��������
                    bestTarget = unit.transform; // ������������� ����� ��� ������ ����
                }
            }
        }
        return bestTarget; // ���������� ������ ����
    }

    // ����� ��� ������������� ������� ������
    protected void Heal(Transform target)
    {
        StartCoroutine(HealWithDelay(target));
    }

    public override void GetBack(Transform target)
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
                    if (unit.team == this.team && distanceToUnit < distanceForCheckWarFieldForHealer && unit is not HealerUnit)
                    {
                        friendCount++;
                    }
                }

                foreach (Unit unit in friends)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team != this.team && distanceToUnit < distanceForCheckWarFieldForHealer)
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
                    if (unit.team == this.team && distanceToUnit < distanceForCheckWarFieldForHealer && unit is not HealerUnit)
                    {
                        friendCount++;
                    }
                }

                foreach (Unit unit in enemies)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team != this.team && distanceToUnit < distanceForCheckWarFieldForHealer)
                    {
                        enemyCount++;
                    }
                }
            }
            // ��� ��� ������ �� �������, ��� ������� �� ��������� ������, ���� ��� �� ������ ��������
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

    private IEnumerator HealWithDelay(Transform target)
    {

        if (target != null)
        {
            yield return new WaitForSeconds(delayBeforeHeal); // ���� �������� ���������� �������

            Unit fr = target.GetComponent<Unit>();
            if (fr.health < fr.maxHP)
            {
                fr.health += heal;
            }
        }
    }

    // ���� �����, ����� ����� �������������� ������ ������ ������������� ������ Unit ��� ������������� �������� ������

}
