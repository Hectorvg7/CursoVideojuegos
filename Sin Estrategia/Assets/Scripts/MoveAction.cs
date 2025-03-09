using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAction : BaseAction
{
    public override string GetActionName()
    {
        return "Move";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        return new List<GridPosition> { unit.GetGridPosition() };
    }

    public override int GetActionPointsCost()
    {
        return 1;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        unit.StartMoving();
        MoveTo(gridPosition);
        onActionComplete = unit.StopMoving;
    }


    public void MoveTo(GridPosition newGridPosition)
    {
        if (LevelGrid.Instance.IsValidGridPosition(newGridPosition))
        {
            // Eliminar la unidad de la posición actual en la rejilla
            LevelGrid.Instance.RemoveUnitAtGridPosition(unit, unit.gridPosition);
            
            // Actualizar la posición de la unidad
            NavMeshAgent agente = unit.GetComponent<NavMeshAgent>();
            Vector3 targetWorldPosition = LevelGrid.Instance.GetWorldPosition(newGridPosition);
            agente.SetDestination(targetWorldPosition);

            // Agregar la unidad a la nueva posición en la rejilla
            LevelGrid.Instance.AddUnitAtGridPosition(unit, unit.gridPosition);
        }
        else
        {
            Debug.Log("La casilla no es válida.");
        }
    }

}
