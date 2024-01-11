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
            if (unit.team == Team.Enemy)
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
            if (unit.team == Team.Friend)
            {
                friendUnits.Add(unit);
            }
        }

        return friendUnits;
    }

    public void RegisterUnit(Unit unit)
    {
        allUnits.Add(unit);
    }

    public void UnregisterUnit(Unit unit)
    {
        if (allUnits.Contains(unit))
        {
            allUnits.Remove(unit);
        }
    }

    public List<Unit> GetAllUnits()
    {
        return allUnits;
    }

}