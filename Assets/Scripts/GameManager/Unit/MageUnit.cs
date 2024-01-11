using UnityEngine;

public class MageUnit : Unit
{
    // Добавим уникальные параметры для мага
    [SerializeField] float attackRadius = 5f;

    public MageUnit()
    {
    }

    public override void Move(Vector3 newPosition)
    {
        // Реализация перемещения мага с учётом избегания союзников и атаки по дуге
        Vector3 direction = (newPosition - transform.position).normalized;
        agent.Move(direction * walkSpeed * Time.deltaTime);
    }

/*    public override void TakeDamage(Unit target, float damage)
    {
        // Логика атаки мага с учетом преимуществ и недостатков
        if (target is InfantryUnit || target is SpearmanUnit) // Преимущество над Пехотой, Копейщиками
        {
            target.TakeDamage(damage * 2); // Наносим двойной урон
        }
        else if (target is ArcherUnit || target is СavalryUnit) // Слабее против Лучников, Конницы
        {
            target.TakeDamage(damage / 2); // Наносим половинный урон
        }
        else
        {
            target.TakeDamage(damage); // Стандартный урон
        }
    }*/

/*    void AreaDamage()
    {
        // Логика атаки мага по области (дуговая атака)
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (var hitCollider in hitColliders)
        {
            Unit unit = hitCollider.GetComponent<Unit>();
            if (unit != null && unit != this && IsEnemy(unit)) // Проверяем, является ли это вражеским юнитом
            {
                unit.TakeDamage(this);
            }
        }
    }*/

    bool IsEnemy(Unit otherUnit)
    {
        // Логика определения, является ли другой юнит врагом
        return true;
    }
}

