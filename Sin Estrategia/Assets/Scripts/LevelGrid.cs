using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : Singleton<LevelGrid>
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
    }
    
//  CORREGIR
    public void AddUnitAtGridPosition()
    {

    }

//  CORREGIR
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.gridObjects;
    }

//  CORREGIR
    public void RemoveUnitAtGridPosition(GridPosition gridPosition)
    {

    }

    public GridPosition GetGridPosition(GridObject gridObject)
    {
        return gridObject.gridPosition;
    }

//  CORREGIR
    public GetWorldPosition()
    {
        gridSystem.GetWorldPosition(gridSystem.GetGridObject());
    }

//  CORREGIR
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        return bool;
    }


//  CORREGIR
    public bool IsValidGridPosition()
    {
        return bool;
    }


    public float GetWidth()
    {
        return gridSystem.GetWidth();
    }

    public float GetHeight()
    {
        return gridSystem.GetHeight();
    }

//  CORREGIR
    public GridObject GetObjectAtGridPosition(GridPosition gridPosition)
    {
        
    }
}
