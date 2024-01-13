using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealerUnit : Unit
{
    public int heal;
    [SerializeField] private float delayBeforeHeal = 2f;
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
        if (target != null)
        {
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
