using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryUnit : Unit
{
    // Добавим уникальные параметры для пехоты
    [SerializeField] float attackRate = 2f;

    public InfantryUnit()
    {
        health = 15;

    }

    public override void Move(Vector3 newPosition)
    {
        // Реализация перемещения пехоты
        agent.SetDestination(newPosition);
    }

    public override void TakeDamage(Unit target)
    {
        // Логика атаки пехоты с учетом преимуществ и недостатков
        if (target is MageUnit) // Слабее против Магов
        {
            target.TakeDamage(target.damage * 2); // Наносим двойной урон
        }
        else
        {
            target.TakeDamage(target.damage); // Стандартный урон
        }
    }

    void Attack()
    {
        // Логика атаки пехоты
    }
}
