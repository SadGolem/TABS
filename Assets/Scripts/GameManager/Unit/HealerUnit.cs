using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealerUnit : Unit
{
    public int heal;
    [SerializeField] private float delayBeforeHeal = 2f;
    private Transform enemiesTarget;
    private int distanceForCheckWarFieldForHealer = 200;
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
        enemiesTarget = FindBestTarget();
        if (target != null)
        {
            if (getBackNOW)
            {
                state = State.WalkToPoint;
                GetBack(enemiesTarget);
                return;
            }
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

    public override void GetBack(Transform target)
    {
        if (target != null && !_isNotGetBack)
        {
            List<Unit> enemies = UnitManager.instance.GetEnemyUnits();
            List<Unit> friends = UnitManager.instance.GetFriendUnits();
            // Считаем количество врагов и союзников в определенном радиусе
            int enemyCount = 0;
            int friendCount = 0;
            if (this.team == Team.Enemy)
            {
                foreach (Unit unit in enemies)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team == this.team && distanceToUnit < distanceForCheckWarFieldForHealer && unit is not HealerUnit)
                    {
                        friendCount++;
                    }
                }

                foreach (Unit unit in friends)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team != this.team && distanceToUnit < distanceForCheckWarFieldForHealer)
                    {
                        enemyCount++;
                    }
                }
            }
            else
            {
                foreach (Unit unit in friends)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team == this.team && distanceToUnit < distanceForCheckWarFieldForHealer && unit is not HealerUnit)
                    {
                        friendCount++;
                    }
                }

                foreach (Unit unit in enemies)
                {
                    float distanceToUnit = Vector3.Distance(this.transform.position, unit.transform.position);
                    if (unit.team != this.team && distanceToUnit < distanceForCheckWarFieldForHealer)
                    {
                        enemyCount++;
                    }
                }
            }
            // Так как хилеры не атакуют, они убегают от вражеских юнитов, если нет ни одного союзника
            if (enemyCount > friendCount)
            {
                getBackNOW = true;
                Vector3 retreatDirection = CalculateRetreatDirection(enemies);
                Vector3 safePosition = transform.position + retreatDirection;
                Move(safePosition); // Отступаем к безопасной позиции
            }
            else
                getBackNOW = false;
        }
    }

    private Vector3 CalculateRetreatDirection(List<Unit> enemies)
    {
        Vector3 retreatVector = Vector3.zero;
        foreach (Unit enemy in enemies)
        {
            retreatVector += (transform.position - enemy.transform.position).normalized;
        }
        retreatVector /= enemies.Count; // Усредняем векторы отступления

        return retreatVector.normalized;
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
