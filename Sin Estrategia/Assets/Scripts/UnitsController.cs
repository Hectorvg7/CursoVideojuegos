using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitsController : MonoBehaviour
{
    public static UnitsController Instance { get; private set; }
    
    private BaseAction selectedAction;
    private Unit selectedUnit;


    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (selectedUnit != null)
            {
                MoveUnitToClick();
            }
            else
            {
                SelectUnit();
            }
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

    private void SelectUnit()
    {
        // Lanzar un raycast para seleccionar la unidad
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                selectedUnit = unit;
                GridVisualizer.Instance.SetSelectedUnit(unit);
                selectedUnit.SelectUnit(); // Llamar al método de selección de unidad
            }
        }
        
    }

     public void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.DeselectUnit(); // Llamar al método de deselección de unidad
            selectedUnit = null;
        }
    }

    public void MoveTo(GridPosition newGridPosition)
    {
        if (LevelGrid.Instance.IsValidGridPosition(newGridPosition))
        {
            // Eliminar la unidad de la posición actual en la rejilla
            LevelGrid.Instance.RemoveUnitAtGridPosition(selectedUnit, selectedUnit.gridPosition);

            // Actualizar la posición de la unidad
            selectedUnit.gridPosition = newGridPosition;

            // Agregar la unidad a la nueva posición en la rejilla
            LevelGrid.Instance.AddUnitAtGridPosition(selectedUnit, selectedUnit.gridPosition);

            // Mover la unidad a la nueva posición en el mundo
            selectedUnit.transform.position = LevelGrid.Instance.GetWorldPosition(newGridPosition);
        }
    }
    
}
