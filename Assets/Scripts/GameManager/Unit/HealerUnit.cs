using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealerUnit : Unit
{
    public int heal;
    [SerializeField] private float delayBeforeHeal = 2f;
    protected HealerUnit()
    {
        health = 20;
        moveSpeed = 2.0f;
        attackRange = 2.0f;
        // У лекарей нет навыков атаки, поэтому урон не устанавливаем
        damage = 0;
    }

    private void Update()
    {
        // Логика выбора цели и лечения будет здесь
        Transform target = FindBestHealTarget();
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                state = State.Attack;
                Heal(target);
            }
            else
            {
                state = State.WalkToPoint;
                Move(target.position);
            }
        }
    }

    // Логика выбора цели для лечения
    protected Transform FindBestHealTarget()
    {
        List<Unit> enemies = UnitManager.instance.GetEnemyUnits(); // Получаем враждебных юнитов
        List<Unit> friends = UnitManager.instance.GetFriendUnits(); // Получаем дружественных юнитов

        List<Unit> unitsToSearch = team is Team.Friend ? friends : enemies; // Определение группы для поиска

        float minHealth = Mathf.Infinity; // Начальное значение для поиска минимального здоровья
        Transform bestTarget = null; // Цель с наименьшим здоровьем
        foreach (Unit unit in unitsToSearch) // Проходим по всем юнитам
        {
            if (unit != null && unit != this) // Проверяем, что юнит существует и не является текущим юнитом
            {
                if (unit.health < minHealth) // Если здоровье юнита меньше текущего минимального здоровья
                {
                    minHealth = unit.health; // Обновляем минимальное здоровье
                    bestTarget = unit.transform; // Устанавливаем юнита как лучшую цель
                }
            }
        }
        return bestTarget; // Возвращаем лучшую цель
    }

    // Метод для осуществления лечения вблизи
    protected void Heal(Transform target)
    {
        StartCoroutine(HealWithDelay(target));
    }

    private IEnumerator HealWithDelay(Transform target)
    {

        if (target != null)
        {
            yield return new WaitForSeconds(delayBeforeHeal); // Ждем заданное количество времени

            Unit fr = target.GetComponent<Unit>();
            if (fr.health < fr.maxHP)
            {
                fr.health += heal;
            }
        }
    }

    // Если нужно, можно также переопределить другие методы родительского класса Unit для специфических действий лекаря

}
