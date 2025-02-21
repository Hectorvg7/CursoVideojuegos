using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    public GridPosition gridPosition;
    private List<Unit> unitList;

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }
}
