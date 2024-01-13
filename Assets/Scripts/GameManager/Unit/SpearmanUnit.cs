using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearmanUnit : Unit
{
    // Добавим уникальные параметры для пехоты
    [SerializeField] private float delayBeforeAttack = 0.5f;

    protected SpearmanUnit()
    {
        health = 20;
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
        if (this.state == State.WalkToPoint)
        {
            agent.SetDestination(newPosition);
            /*            state = State.WalkToPoint;*/
        }
    }
    public override void Attack(Transform target, Transform attackUnit)
    {
        StartCoroutine(DelayedAttack(target, attackUnit)); // Запускаем корутину для выполнения атаки с задержкой
    }

    private IEnumerator DelayedAttack(Transform target,Transform attackUnit)
    {
        yield return new WaitForSeconds(delayBeforeAttack); // Ждем заданное количество времени

        // Логика атаки врага, когда конница достигает нужной дистанции
        if (target != null)
        {
            if (Vector3.Distance(attackUnit.position, target.position) <= attackRange)
            {
                Debug.Log(Vector3.Distance(transform.position, target.position));
                Unit targetUnit = target.GetComponent<Unit>();
                state = State.Attack;
                if (targetUnit is MageUnit) // Слабее против Магов
                {
                    this.TakeDamage(targetUnit, damage / 2); // Наносим половинный урон
                }
                else
                {
                    this.TakeDamage(targetUnit, damage); // Стандартный урон
                }
            }
        }

    }

}
