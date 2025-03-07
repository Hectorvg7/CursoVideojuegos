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

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        /*REVISAR*/ GridVisualizer.Instance.SetSelectedUnit(unit);

        isActive = true;
        this.onActionComplete = onActionComplete;

        StartCoroutine(WaitForClick());
    }

    private IEnumerator WaitForClick()
    {
        while (isActive)
        {
            if (Input.GetMouseButtonDown(0)) // Si el jugador hace clic en el mapa
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Obtener la GridPosition del lugar donde hacemos clic
                    GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(hit.point);

                    // Comprobar si la casilla es válida
                    if (LevelGrid.Instance.IsValidGridPosition(targetGridPosition))
                    {
                        // Mover la unidad a la casilla seleccionada
                        MoveTo(targetGridPosition);
                        isActive = false; // Dejar de esperar después de mover
                    }
                    else
                    {
                        Debug.Log("Casilla inválida. Por favor, seleccione una casilla válida.");
                    }
                }
            }
            yield return null; // Esperar hasta el siguiente frame
        }

        // Una vez que se ha completado la acción, llamamos a onActionComplete (si se proporcionó)
        onActionComplete?.Invoke();
    }

    public void MoveTo(GridPosition newGridPosition)
    {
        if (LevelGrid.Instance.IsValidGridPosition(newGridPosition))
        {
            // Eliminar la unidad de la posición actual en la rejilla
            //LevelGrid.Instance.RemoveUnitAtGridPosition(unit, unit.gridPosition);
            
            // Actualizar la posición de la unidad
            NavMeshAgent agente = unit.GetComponent<NavMeshAgent>();
            Vector3 targetWorldPosition = LevelGrid.Instance.GetWorldPosition(newGridPosition);
            agente.SetDestination(targetWorldPosition);

            // Agregar la unidad a la nueva posición en la rejilla
            LevelGrid.Instance.AddUnitAtGridPosition(unit, unit.gridPosition);
        }
    }

    private void MoveUnitToClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Obtener la GridPosition del lugar donde hacemos clic
            GridPosition targetGridPosition = LevelGrid.Instance.GetGridPosition(hit.point);

            // Mover la unidad seleccionada a esa GridPosition
            MoveTo(targetGridPosition);
        }
    }
}
