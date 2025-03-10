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
    public LayerMask unitLayer;

    //Pintar casillas disponibles
    public GameObject validMoveColor;
    public GameObject invalidMoveColor;
    private float alturaCasilla = 0.1f;
    private Quaternion rotacionCasilla = Quaternion.Euler(90f, 0f, 0f);
    private GridSystem gridSystem;
    void Awake()
    {
      Instance = this;
      gridSystem = LevelGrid.Instance.gridSystem;
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
        }
    }


    public void SetSelectedAction(BaseAction baseAction)
    {
        Debug.Log("Set selected action: " + baseAction.ToString());
        selectedAction = baseAction;
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

        if (Physics.Raycast(ray, out hit, float.MaxValue, unitLayer))
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (unit != null)
            {
                selectedUnit = unit;
                selectedUnit.SelectUnit(); // Llamar al método de selección de unidad
                ShowActionsForSelectedUnit();
                PintarCasillas();
            }
        }
    }

    public void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.DeselectUnit(); // Llamar al método de deselección de unidad
            selectedUnit = null;
            BorrarQuads();
            
        foreach (Transform child in grupoBotones.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void PintarCasillas()
    {
        
        // Limpiar cualquier quad instanciado.
        BorrarQuads();

        // Obtenemos el rango de movimiento de la unidad seleccionada
        int moveRange = 20;


        // Iteramos por la rejilla y dibujamos solo las celdas dentro del rango de movimiento
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GridObject gridObject = gridSystem.GetGridObject(gridPosition);

                if (gridObject == null) continue;

                // Comprobar si la celda está dentro del rango de movimiento
                int distance = Mathf.Abs(gridPosition.x - selectedUnit.GetGridPosition().x) + Mathf.Abs(gridPosition.z - selectedUnit.GetGridPosition().z);
                bool isValidMove = distance <= moveRange;

                GameObject casilla = isValidMove ? validMoveColor : invalidMoveColor;

                // Instanciar el prefab en la posición correspondiente
                Vector3 position = LevelGrid.Instance.GetWorldPosition(gridPosition) + new Vector3(0f, alturaCasilla, 0f);
                if (casilla != null)
                {
                    Instantiate(casilla, position, rotacionCasilla);
                }
            }
        }
    }

    public void BorrarQuads()
    {
        GameObject[] antiguosQuads = GameObject.FindGameObjectsWithTag("MoveQuad");

        foreach (var quad in antiguosQuads)
        {
            Destroy(quad);
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




    public void DevolverPuntos(Unit unit)
    {
        if (unit != null)
        {
            unit.actionPoints = unit.maxPointsPerTurn;
        }
    }
    
    
}
