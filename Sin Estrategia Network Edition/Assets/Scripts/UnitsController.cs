using System;
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
    private List<Unit> unitsList = new List<Unit>();
    private List<Unit> enemyUnitsList = new List<Unit>();

    //Pintar casillas disponibles
    public GameObject validMoveColor;
    public GameObject invalidMoveColor;
    private float alturaCasilla = 0.1f;
    private Quaternion rotacionCasilla = Quaternion.Euler(90f, 0f, 0f);
    private GridSystem gridSystem;


    //Sistema de eventos
    public event EventHandler OnActionSelected;


    void Awake()
    {
      Instance = this;
      gridSystem = LevelGrid.Instance.gridSystem;
      GetUnitsList();
      TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }


    void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (selectedAction == null || selectedAction.GetActionName() != "Shoot")
            {
                SelectUnit();
            }

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
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
                        if (selectedAction is MoveAction moveAction)
                        {
                            moveAction.RequestMove(gridPosition);
                        }
                        else if (selectedAction is ShootAction shootAction)
                        {
                            shootAction.RequestShoot(gridPosition);
                        }
                        else
                        {
                            selectedAction.TakeAction(gridPosition, ClearBusy);
                        }
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
                DeselectUnit();
                selectedUnit = unit;
                selectedUnit.SelectUnit(); // Llamar al método de selección de unidad
                ShowActionsForSelectedUnit();
            }
        }
    }

    public void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            selectedAction = null;
            selectedUnit.DeselectUnit(); // Llamar al método de deselección de unidad
            selectedUnit = null;
            BorrarQuads();
            
        foreach (Transform child in grupoBotones.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public List<Unit> GetUnitsList()
    {
        unitsList.Clear();

        GameObject[] unidadesEncontradas = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unidadGO in unidadesEncontradas)
        {
            Unit unidad = unidadGO.GetComponent<Unit>();
            if (unidad != null)
            {
                unitsList.Add(unidad);
            }
        }
        
        return unitsList;
    }

    public List<Unit> GetEnemyUnitsList()
    {
        enemyUnitsList.Clear();

        GameObject[] unidadesEncontradas = GameObject.FindGameObjectsWithTag("EnemyUnit");

        foreach (GameObject unidadGO in unidadesEncontradas)
        {
            Unit unidad = unidadGO.GetComponent<Unit>();
            if (unidad != null)
            {
                enemyUnitsList.Add(unidad);
            }
        }

        return enemyUnitsList;
    }

    // Método para obtener la unidad más cercana
    public Unit GetClosestPlayerUnitTo(Unit enemyUnit)
    {
        Unit closestUnit = null;
        float closestDistance = float.MaxValue; // Inicializamos con un valor muy alto

        // Comprobamos todas las unidades aliadas
        foreach (Unit playerUnit in unitsList)
        {
            if (playerUnit == null) // Verificamos si la unidad es nula o ha muerto
            continue;

            float distance = Vector3.Distance(enemyUnit.transform.position, playerUnit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestUnit = playerUnit;
            }
        }

        return closestUnit;
    }

    public void PintarCasillas()
    {
        
        // Limpiar cualquier quad instanciado.
        BorrarQuads();

        int rango = 3;

        if (selectedUnit == null)
        {
            return;
        }

        // Obtener la posición de la unidad seleccionada
        GridPosition selectedUnitPosition = selectedUnit.GetGridPosition();

        // Iteramos por la rejilla y dibujamos solo las celdas dentro del rango de movimiento
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GridObject gridObject = gridSystem.GetGridObject(gridPosition);

                if (gridObject == null) continue;

                // Comprobar si la celda está dentro del rango de movimiento
                int distance = Mathf.Abs(gridPosition.x - selectedUnitPosition.x) + Mathf.Abs(gridPosition.z - selectedUnitPosition.z);
                bool isValidMove = false;
                
                if (selectedAction.GetActionName() == "Move")
                {
                    Debug.Log("Acción Move en curso");
                    isValidMove = distance <= rango && !LevelGrid.Instance.HasAnyUnitOnGridPosition(gridPosition);
                }
                else if (selectedAction.GetActionName() == "Shoot")
                {
                    Debug.Log("Acción Shoot en curso");
                    isValidMove = distance <= rango && LevelGrid.Instance.HasAnyEnemyUnitOnGridPosition(gridPosition);
                }
                
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

    private void ActionButton_OnActionSelected(object sender, EventArgs e)
    {
        PintarCasillas();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        DeselectUnit();
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

        ActionButton button = boton.GetComponent<ActionButton>();
        button.SetBaseAction(action);
        button.OnActionSelected += ActionButton_OnActionSelected;
    }




    public void DevolverPuntos(Unit unit)
    {
        if (unit != null)
        {
            unit.actionPoints.Value = unit.maxPointsPerTurn;
        }
    }
    
    
}
