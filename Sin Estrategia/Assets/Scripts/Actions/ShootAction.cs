using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private int fireRange = 2; //Rango máximo de disparo

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        return new List<GridPosition> { unit.GetGridPosition() };
    }

    public override int GetActionPointsCost()
    {
        return 3;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        UnitsController.Instance.PintarCasillas();

        // Verificamos si la casilla está dentro del rango de 3 casillas
        int distance = Mathf.Abs(gridPosition.x - unit.GetGridPosition().x) + Mathf.Abs(gridPosition.z - unit.GetGridPosition().z);
        if (distance <= fireRange) // Solo permitimos disparar dentro de un rango de 3 casillas
        {
            ShootTo(gridPosition);
            unit.Shoot();
        }
    }

    public void ShootTo(GridPosition newGridPosition)
    {
        if (LevelGrid.Instance.IsValidGridPosition(newGridPosition) && LevelGrid.Instance.HasAnyEnemyUnitOnGridPosition(newGridPosition))
        {
            Debug.Log("Has disparado");
            unit.actionPoints -= GetActionPointsCost();
        }
        else
        {
            Debug.Log("La casilla no es válida.");
        }
    }
}
