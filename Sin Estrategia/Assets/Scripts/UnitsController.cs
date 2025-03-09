using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitsController : MonoBehaviour
{
    public static UnitsController Instance { get; private set; }
    
    private BaseAction selectedAction;
    public Unit selectedUnit;
    public GameObject prefabBoton;
    public GameObject grupoBotones;
    private bool isBusy = false;
    public LayerMask groundLayer;

    void Awake()
    {
      Instance = this;
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        Debug.Log("Set selected action: " + baseAction.ToString());
        selectedAction = baseAction;
    }

    public void DevolverPuntos(Unit unit)
    {
        unit.actionPoints = unit.maxPointsPerTurn;
    }

    void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (selectedUnit == null)
            {
                SelectUnit();
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, groundLayer))
            {
                var  position = hit.point;
                GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(position);

                if (selectedUnit != null && selectedAction != null)
                {
                    if (selectedUnit.CanSpendPointsToTakeAction(selectedAction))
                    {
                        selectedAction.TakeAction(gridPosition, ClearBusy);
                        selectedUnit.actionPoints -= selectedAction.GetActionPointsCost();
                    }
                    else 
                    {
                        Debug.Log("No tienes suficientes puntos para realizar la acción.");
                    }
                }
            }
            //Pasar posición de mundo (Vector3) a gridPosition -> 
            //gridPosition = LevelGrid.Instance.GetGridPosition(hit.point)
            //selectedAction.TakeAction(gridPosition, ClearBusy);

        }
    }

    private void ClearBusy()
    {
        isBusy = false;
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
                selectedUnit.SelectUnit(); // Llamar al método de selección de unidad
                GridVisualizer.Instance.SetSelectedUnit(unit);
                ShowActionsForSelectedUnit();
            }
        }
        
    }

    private void ShowActionsForSelectedUnit()
    {
        foreach (Transform child in grupoBotones.transform)
        {
            Destroy(child.gameObject);
        }

        if (selectedUnit != null)
        {
            BaseAction[] actions = selectedUnit.availableActions;

            foreach (BaseAction action in actions)
            {
                CreateButton(action);
            }
        }
    }

    private void CreateButton(BaseAction action)
    {
        GameObject boton = Instantiate(prefabBoton, grupoBotones.transform);

        CreateButton button = boton.GetComponent<CreateButton>();
        button.SetBaseAction(action);
    }

     public void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.DeselectUnit(); // Llamar al método de deselección de unidad
            selectedUnit = null;
            
        foreach (Transform child in grupoBotones.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    
    
}
