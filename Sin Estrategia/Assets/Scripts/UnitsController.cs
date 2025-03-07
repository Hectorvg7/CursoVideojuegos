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

    void Awake()
    {
      Instance = this;  
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        Debug.Log("Set selected action: " + baseAction.ToString());
        selectedAction = baseAction;
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

            //Hacer rayo a suelo
            //Pasar posición de mundo (Vector3) a gridPosition -> 
            //gridPosition = LevelGrid.Instance.GetGridPosition(HierarchyType.point)
            //selectedAction.TakeAction(gridPosition, ClearBussy);

        }
    }

    private void ClearBussy()
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
                //selectedAction = selectedUnit.GetBaseActionArray()[1];
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
