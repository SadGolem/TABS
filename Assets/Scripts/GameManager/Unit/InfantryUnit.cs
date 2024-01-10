using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryUnit : Unit
{
    // ������� ���������� ��������� ��� ������
    [SerializeField] float attackRate = 2f;

    public InfantryUnit()
    {
        health = 15;

    }

    public override void Move(Vector3 newPosition)
    {
        // ���������� ����������� ������
        agent.SetDestination(newPosition);
    }

    public override void TakeDamage(Unit target)
    {
        // ������ ����� ������ � ������ ����������� � �����������
        if (target is MageUnit) // ������ ������ �����
        {
            target.TakeDamage(target.damage * 2); // ������� ������� ����
        }
        else
        {
            target.TakeDamage(target.damage); // ����������� ����
        }
    }

    void Attack()
    {
        // ������ ����� ������
    }
}
