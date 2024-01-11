using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherUnit : Unit
{
    [SerializeField] private GameObject projectilePrefab;
    protected ArcherUnit()
    {
        health = 5;
        moveSpeed = 2.5f;
        damage = 5;
    }

    public override void Move(Vector3 newPosition)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.speed = moveSpeed; // Устанавливаем скорость перемещения
            agent.SetDestination(newPosition);
            state = State.WalkToPoint;
        }
    }

    void Update()
    {
        // Логика выбора врага и просчета лучшей атаки будет здесь
        if (currentTarget == null)
        {
            currentTarget = FindBestTarget();
            Debug.Log(currentTarget);
        }

        if (currentTarget != null)
        {
            // Нападаем на врага, если он в пределах атаки
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

    public override void Attack(Transform target, Transform attackUnit)
    {
        state = State.Attack;
        ArrowAttack(target);
    }

    protected void ArrowAttack(Transform target)
    {
        Vector3 attackPosition = target.position; // Сохраняем позицию цели для атаки

        Unit unit = target.GetComponent<Unit>();
        if (unit != null && unit.team != this.team)
        {
            // Создаем и запускаем летящий объект в сторону цели
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Launch(attackPosition);
                while (!projectileScript.isDoletelo)
                {           
                    System.Threading.Thread.Sleep(100); 
                }
                this.TakeDamage(unit, damage);
            }
        }
    }

}
