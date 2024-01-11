using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanUnit : Unit
{
    // Добавим уникальные параметры для пехоты

    protected SpearmanUnit()
    {
        health = 7;
        moveSpeed = 4f;
        attackRange = 2;
        damage = 2;
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

    public override void Move(Vector3 newPosition)
    {
        // Реализация перемещения пехоты
        agent.SetDestination(newPosition);
    }

    public override void Attack(Transform target, Transform attackUnit)
    {
        // Логика атаки врага, когда конница достигает нужной дистанции
        if (Vector3.Distance(attackUnit.transform.position, target.position) <= attackRange)
        {
            Debug.Log(Vector3.Distance(transform.position, target.position));
            Unit targetUnit = target.GetComponent<Unit>();
            state = State.Attack;
            if (targetUnit is MageUnit) // Слабее против Магов
            {
                targetUnit.TakeDamage(targetUnit, damage / 2); // Наносим двойной урон
            }
            else
            {
                targetUnit.TakeDamage(targetUnit, damage); // Стандартный урон
            }
        }
    }
}
