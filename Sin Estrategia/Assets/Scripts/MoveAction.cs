using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAction : BaseAction
{
    private int moveRange = 5; // Limite de movimiento

    
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

        //Lógica para parar la transición.
        // Comienza a verificar si el agente ha llegado a su destino
        StartCoroutine(WaitUntilArrived());
    }

    private IEnumerator WaitUntilArrived()
    {
        NavMeshAgent agente = unit.GetComponent<NavMeshAgent>();
        
        // Esperar hasta que el agente llegue a su destino
        while (agente.pathPending || agente.remainingDistance > 0.1f)
        {
            yield return null; // Espera un frame
        }

        // Una vez que el agente ha llegado a su destino, detener la animación de movimiento y volver a pintar las casillas.
        unit.StopMoving();
        UnitsController.Instance.PintarCasillas();

        // Llamar al callback cuando la acción se complete
        onActionComplete?.Invoke();
    }
    


    public void MoveTo(GridPosition newGridPosition)
    {
        if (LevelGrid.Instance.IsValidGridPosition(newGridPosition) && !LevelGrid.Instance.HasAnyUnitOnGridPosition(newGridPosition))
        {
            // Eliminar la unidad de la posición actual en la rejilla
            LevelGrid.Instance.RemoveUnitAtGridPosition(unit, unit.gridPosition);
            
            // Actualizar la posición de la unidad
            NavMeshAgent agente = unit.GetComponent<NavMeshAgent>();
            Vector3 targetWorldPosition = LevelGrid.Instance.GetWorldPosition(newGridPosition);
            agente.SetDestination(targetWorldPosition);

            // Agregar la unidad a la nueva posición en la rejilla
            LevelGrid.Instance.AddUnitAtGridPosition(unit, newGridPosition);
        }
        else
        {
            Debug.Log("La casilla no es válida.");
        }
    }

}
