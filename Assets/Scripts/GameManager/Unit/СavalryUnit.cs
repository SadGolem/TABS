
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class СavalryUnit : Unit
{
    [SerializeField] float acceleration = 1f;

    private UnitManager unitManager;

    protected СavalryUnit()
    {
        health = 10;
        moveSpeed = 3;
        attackRange = 1;
        damage = 3;
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
            agent.speed = moveSpeed; // Устанавливаем скорость перемещения
            agent.acceleration = acceleration; // Устанавливаем ускорение
            agent.SetDestination(newPosition);
            state = State.WalkToPoint;
        }
    }


    public override void Attack(Transform target, Transform attackUnit)
    {
        // Логика атаки врага, когда конница достигает нужной дистанции
        if (Vector3.Distance(attackUnit.transform.position, target.position) <= attackRange)
        {
            Debug.Log(Vector3.Distance(transform.position, target.position));
            Unit targetUnit = target.GetComponent<Unit>();
            state = State.Attack;
            if (targetUnit is MageUnit || targetUnit is HealerUnit || targetUnit is ArcherUnit) // Преимущество над Магами, Лекарями, Лучниками
            {
                targetUnit.TakeDamage(targetUnit, damage * 2); // Наносим двойной урон
            }
            else if (targetUnit is SpearmanUnit) // Слабее против Копейщиков
            {
                targetUnit.TakeDamage(targetUnit, damage / 2); // Наносим половинный урон
            }
            else
            {
                targetUnit.TakeDamage(targetUnit, damage); // Стандартный урон
            }
        }
        else
        {
            state = State.WalkToPoint;
            Move(target.position);
        }
    }



}
