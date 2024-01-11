
using System.Collections;
using UnityEngine;

public class InfantryUnit : Unit
{
    [SerializeField] private float delayBeforeAttack = 2f;
    // Добавим уникальные параметры для пехоты

    protected InfantryUnit()
    {
        health = 30;
        moveSpeed = 4f;
        attackRange = 1;
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
                currentTarget = null;
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
        StartCoroutine(PerformDelayedAttack(target)); // Запускаем корутину для выполнения атаки с задержкой
    }

    private IEnumerator PerformDelayedAttack(Transform target)
    {
        yield return new WaitForSeconds(delayBeforeAttack); // Ждем заданное количество времени

        Unit unit = target.GetComponent<Unit>();
        state = State.Attack;
        this.TakeDamage(unit, damage); // Наносим урон цели
    }

    public override void Move(Vector3 newPosition)
    {
        if (this.state == State.WalkToPoint)
        {
            agent.SetDestination(newPosition);
            /*            state = State.WalkToPoint;*/
        }
    }
}
