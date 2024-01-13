using System.Collections;
using UnityEngine;

public class MageUnit : Unit
{
    // �������������� �������������� ��� ����������� �����
    [Header("Mage Specific")]
    [SerializeField] protected int areaDamage = 5; // ���� �� �������
    [SerializeField] private GameObject projectilePrefab;

    protected MageUnit()
    {
        health = 10;
        moveSpeed = 1;
        attackRange = 40;
        damage = 1;
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

    private bool canShoot = true; // ���� ��� ������������ ����������� ��������

    private IEnumerator ShootProjectile(Transform target, float interval)
    {
        while (true)
        {
            if (canShoot)
            {
                canShoot = false; // ����� ������� �������� ������������� ���� � false

                // ������� � ��������� ������� ������ � ������� ����
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                if (projectileScript != null)
                {
                    Vector3 attackPosition = target.position;
                    projectileScript.Launch(attackPosition, this, target.GetComponent<Unit>(), damage);
                }
            }
            yield return new WaitForSeconds(interval); // ���� �������� ���������� ������� ����� ����������
        }
    }

    public override void Attack(Transform target, Transform attackUnit)
    {
        if (target != null)
        {
            state = State.Attack;

            StartCoroutine(ShootProjectile(target, 5f)); // ��������� �������� ��� �������� �������� � �������� ����������
        }
    }
/*
    // ������ ��� ����� �� �������
    protected void AreaAttack(Transform target)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        Vector3 attackPosition = target.position; // ��������� ������� ���� ��� �����

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag != "Terrain")
            {
                Unit unit = hitCollider.GetComponent<Unit>();
                if (unit != null && unit.team != this.team)
                {   
                    // ������� � ��������� ������� ������ � ������� ����
                    GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    Projectile projectileScript = projectile.GetComponent<Projectile>();
                    if (projectileScript != null)
                    {
                        projectileScript.Launch(attackPosition, this, unit, damage);
                        *//*this.TakeDamage(unit, areaDamage);*//*
                    }
                }
            }
        }
    }*/


    // ������ ��� �������� � ���� � ������� �����
    protected void MoveTowardsAndAttack(Transform target, Transform attackUnit)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.speed = moveSpeed; // ������������� �������� �����������
            agent.SetDestination(target.transform.position);
            /*state = State.WalkToPoint;*/
        }
    }

}

