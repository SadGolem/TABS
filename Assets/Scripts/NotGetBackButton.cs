using UnityEngine;

public class NotGetBackButton : MonoBehaviour
{
    private bool turner = false;

    public void NotGetBackTurner()
    {
        turner = !turner;
        ModifyIsNotGetBackForAllUnits(turner);
    }

    private void ModifyIsNotGetBackForAllUnits(bool newValue)
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        foreach (Unit unit in allUnits)
        {
            unit.IsNotGetBack = newValue;
        }
    }
}
