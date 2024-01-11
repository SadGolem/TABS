using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealerUnit : Unit
{
    public int heal;
    protected HealerUnit()
    {
        health = 5;
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
        List<Unit> enemies = UnitManager.instance.GetEnemyUnits();
        Debug.Log(enemies, this);
        List<Unit> friends = UnitManager.instance.GetFriendUnits();
        Debug.Log(friends, this);
        if (enemies.Count != 0 && friends.Count != 0)
        {
            List<Unit> unitsToSearch = team is Team.Friend ? enemies.Cast<Unit>().ToList() : friends.Cast<Unit>().ToList();
            Debug.Log(unitsToSearch);
            float health = Mathf.Infinity;
            Transform bestTarget = null;

            foreach (Unit unit in unitsToSearch)
            {
                if (unit != null && unit != this)
                {
                    float healthMin = unit.health;
                    if (healthMin < health)
                    {
                        health = healthMin;
                        bestTarget = unit.transform;
                    }
                }
            }
            Debug.Log(bestTarget);
            currentTarget = bestTarget;
            return bestTarget;
        }
        else { return null; }
    }

    // Метод для осуществления лечения вблизи
    protected void Heal(Transform target)
    {
        Unit fr = target.GetComponent<Unit>();
        fr.health += heal;
    }

    // Если нужно, можно также переопределить другие методы родительского класса Unit для специфических действий лекаря

}
