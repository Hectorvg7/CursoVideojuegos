using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private float ancho;
    private float altura;
    private Vector3 tamanoCelda;
    private List<GridObject> gridObjects;

    public Vector3 GetWorldPosition(GridObject gridObject)
    {
        Vector3 worldPosition = new Vector3(gridObject.gridPosition.x, 0, gridObject.gridPosition.z);
        return worldPosition;
    }

    /*
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
    
    }
    */

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjects[0];
    }

    public float GetWidth()
    {
        return ancho;
    }

    public float GetHeight()
    {
        return altura;
    }



}
