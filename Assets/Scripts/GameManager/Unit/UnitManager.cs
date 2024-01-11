using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    [SerializeField] List<FriendlyUnit> friendUnits = new List<FriendlyUnit>();
    [SerializeField] List<Unit> allUnits = new List<Unit>();

    public static UnitManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Unit[] unitsArray = FindObjectsOfType<Unit>(); // Находим все юниты в текущей сцене
        allUnits = new List<Unit>(unitsArray);
        AddEnemyUnits();
        AddFriendUnits();
    }

    public List<EnemyUnit> AddEnemyUnits()
    {
        foreach (Unit unit in allUnits)
        {
            if (unit is EnemyUnit)
            {
                enemyUnits.Add((EnemyUnit)unit);
            }
        }

        return enemyUnits;
    }

    public List<EnemyUnit> GetEnemyUnits()
    {
        return enemyUnits;
    }

    public List<FriendlyUnit> GetFriendUnits()
    {
        return friendUnits;
    }

    public List<FriendlyUnit> AddFriendUnits()
    {
        foreach (Unit unit in allUnits)
        {
            if (unit is FriendlyUnit)
            {
                friendUnits.Add((FriendlyUnit)unit);
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