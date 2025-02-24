using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    private GridSystem gridSystem;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Hay más de una instancia del LevelGrid");
            return;
        }
        Instance = this;

        gridSystem = new GridSystem(7, 14, 1f);
    }
    

    public void AddUnitAtGridPosition(Unit unit, GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject?.AddUnit(unit);
    }


    public void RemoveUnitAtGridPosition(Unit unit, GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject?.RemoveUnit(unit);
    }


    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject?.GetUnitList();
    }


    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return gridSystem.GetGridPosition(worldPosition);
    }


    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return gridSystem.GetWorldPosition(gridPosition);
    }


    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        return GetUnitListAtGridPosition(gridPosition).Count > 0;
    }


    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridSystem.IsValidGridPosition(gridPosition);
    }


    public float GetWidth()
    {
        return gridSystem.GetWidth();
    }

    public float GetHeight()
    {
        return gridSystem.GetHeight();
    }

}
