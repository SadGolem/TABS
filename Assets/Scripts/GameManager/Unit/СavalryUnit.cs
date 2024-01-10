
using UnityEngine;
using UnityEngine.AI;

public class СavalryUnit : Unit
{
    /*[SerializeField] NavMeshAgent navMeshAgent;*/
    [SerializeField] Vector3 targetPoint;
    [SerializeField] Unit targetUnit;
    [SerializeField] float acceleration = 2f;

    private Transform currentTarget;

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
        }

        if (currentTarget != null)
        {
            // Нападаем на врага, если он в пределах атаки
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
            if (distanceToTarget <= attackRange)
            {
                Attack(currentTarget);
            }
        }

        // Простейшее движение конницы для демонстрации
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    Transform FindBestTarget()
    {
        // Здесь будет логика выбора лучшего врага (против Магов, Лекарей, Лучников и противостояние Копейщикам)
        // ...
        return null; // Заглушка
    }

    public void Place()
    {

    }

    public override void Move(Vector3 newPosition)
    {
        // Реализация перемещения конницы, учитывающая скорость и ускорение
        agent.acceleration = acceleration;
        agent.SetDestination(newPosition);
        state = State.WalkToPoint;
    }

    public override void TakeDamage(Unit target)
    {
        // Логика атаки юнита с учетом преимуществ и недостатков
        if (target is MageUnit || target is HealerUnit || target is ArcherUnit) // Преимущество над Магами, Лекарями, Лучниками
        {
            target.TakeDamage(target.damage * 2); // Наносим двойной урон
        }
        else if (target is SpearmanUnit) // Слабее против Копейщиков
        {
            target.TakeDamage(damage / 2); // Наносим половинный урон
        }
        else
        {
            target.TakeDamage(damage); // Стандартный урон
        }
    }


    void Attack(Transform target)
    {
        // Логика атаки врага
        // ...
    }
}
