using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] List<Unit> enemyUnits = new List<Unit>();
    [SerializeField] List<Unit> friendUnits = new List<Unit>();
    [SerializeField] List<Unit> allUnits = new List<Unit>();

    public static UnitManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        allUnits = FindObjectsOfType<Unit>().ToList(); // Находим все юниты в текущей сцене
        AddEnemyUnits();
        AddFriendUnits();
    }

    public List<Unit> AddEnemyUnits()
    {
        foreach (Unit unit in allUnits)
        {
            if (unit.team == Team.Enemy && !enemyUnits.Contains(unit))
            {
                enemyUnits.Add(unit);
            }
        }

        return enemyUnits;
    }

    public List<Unit> GetEnemyUnits()
    {
        return enemyUnits;
    }

    public List<Unit> GetFriendUnits()
    {
        return friendUnits;
    }

    public List<Unit> AddFriendUnits()
    {
        foreach (Unit unit in allUnits)
        {
            if (unit.team == Team.Friend && !friendUnits.Contains(unit))
            {
                friendUnits.Add(unit);
            }
        }

        return friendUnits;
    }
    public List<Unit> AddFriendUnits2(Unit unit)
    {
        if (unit.team == Team.Friend)
        {
            friendUnits.Add(unit);
        }

        return friendUnits;
    }

    public List<Unit> AddEnemyUnits2(Unit unit)
    {
        if (unit.team == Team.Enemy)
        {
            enemyUnits.Add(unit);
        }

        return enemyUnits;
    }

    public void RegisterUnit(Unit unit)
    {
        allUnits.Add(unit);
        AddFriendUnits2(unit);
        AddEnemyUnits2(unit);
    }

    public void UnregisterUnit(Unit unit)
    {
        if (allUnits.Contains(unit))
        {
            allUnits.Remove(unit);
            UnregisterUnitFriend(unit);
            UnregisterUnitEnemy(unit);
        }
    }

    public void UnregisterUnitEnemy(Unit unit)
    {
        if (enemyUnits.Contains(unit))
        {
            enemyUnits.Remove(unit);
        }
    }

    public void UnregisterUnitFriend(Unit unit)
    {
        if (friendUnits.Contains(unit))
        {
            friendUnits.Remove(unit);
        }
    }

    public List<Unit> GetAllUnits()
    {
        return allUnits;
    }

}