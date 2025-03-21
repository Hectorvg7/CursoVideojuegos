using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    public GridPosition gridPosition;
    private List<Unit> unitList;

    public GridObject(GridPosition position)
    {
        this.gridPosition = position;
        this.unitList = new List<Unit>();
    }

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
        if (!unitList.Contains(unit))
        {    
            unitList.Add(unit);
        }
    }
}
