using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int ancho;
    private int altura;
    private float tamanoCelda;
    private GridObject[,] gridObjects;

    public GridSystem(int ancho, int altura, float tamanoCelda)
    {
        this.ancho = ancho;
        this.altura = altura;
        this.tamanoCelda = tamanoCelda;

        gridObjects = new GridObject[ancho, altura];
        for (int x = 0; x < ancho; x++)
        {
            for (int z = 0; z < altura; z++)
            {
                gridObjects[x, z] = new GridObject(new GridPosition(x, z));
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        Vector3 worldPosition = new Vector3(gridPosition.x * tamanoCelda, 0, gridPosition.z * tamanoCelda);
        return worldPosition;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / tamanoCelda);
        int z = Mathf.RoundToInt(worldPosition.z / tamanoCelda);
        return new GridPosition(x, z);
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < ancho && gridPosition.z >= 0 && gridPosition.z < ancho;
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return IsValidGridPosition(gridPosition) ? gridObjects[gridPosition.x, gridPosition.z] : null;
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
