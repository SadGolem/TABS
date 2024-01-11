using UnityEngine;

public class MageUnit : Unit
{
    // ������� ���������� ��������� ��� ����
    [SerializeField] float attackRadius = 5f;

    public MageUnit()
    {
    }

    public override void Move(Vector3 newPosition)
    {
        // ���������� ����������� ���� � ������ ��������� ��������� � ����� �� ����
        Vector3 direction = (newPosition - transform.position).normalized;
        agent.Move(direction * walkSpeed * Time.deltaTime);
    }

/*    public override void TakeDamage(Unit target, float damage)
    {
        // ������ ����� ���� � ������ ����������� � �����������
        if (target is InfantryUnit || target is SpearmanUnit) // ������������ ��� �������, �����������
        {
            target.TakeDamage(damage * 2); // ������� ������� ����
        }
        else if (target is ArcherUnit || target is �avalryUnit) // ������ ������ ��������, �������
        {
            target.TakeDamage(damage / 2); // ������� ���������� ����
        }
        else
        {
            target.TakeDamage(damage); // ����������� ����
        }
    }*/

/*    void AreaDamage()
    {
        // ������ ����� ���� �� ������� (������� �����)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (var hitCollider in hitColliders)
        {
            Unit unit = hitCollider.GetComponent<Unit>();
            if (unit != null && unit != this && IsEnemy(unit)) // ���������, �������� �� ��� ��������� ������
            {
                unit.TakeDamage(this);
            }
        }
    }*/

    bool IsEnemy(Unit otherUnit)
    {
        // ������ �����������, �������� �� ������ ���� ������
        return true;
    }
}

