using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxPointsPerTurn = 4;
    public int actionPoints;
    public GridPosition gridPosition;
    public BaseAction[] availableActions;

    public GameObject quads;
    private Animator animator;
    private Transform transform;


    [SerializeField] bool isEnemy;
    private bool isSelected = false;
    private bool isMoving = false;

    void Awake()
    {
        availableActions = GetComponents<BaseAction>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        actionPoints = maxPointsPerTurn;
        quads.SetActive(false);
    }

    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(this, gridPosition);
    }

    public void SelectUnit()
    {
        isSelected = true;
        quads.SetActive(true);
    }

    public void DeselectUnit()
    {
        isSelected = false;
        quads.SetActive(false);
    }

    public void SetGridPosition(GridPosition newGridPosition)
    {
        gridPosition = newGridPosition;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return availableActions;
    }

    public bool CanSpendPointsToTakeAction(BaseAction action)
    {
        return actionPoints >= action.GetActionPointsCost();
    }

    public int GetActionPoints()
    {
        return actionPoints;
    }

    public void ResetActionPoints()
    {
        actionPoints = maxPointsPerTurn;
    }

     // Método para indicar si la unidad se está moviendo
    public void StartMoving()
    {
        isMoving = true;
        // Cambiar el parámetro del Animator para indicar que la unidad se mueve
        animator.SetBool("isMoving", true);
    }

    public void StopMoving()
    {
        isMoving = false;
        // Cambiar el parámetro del Animator para indicar que la unidad está parada
        animator.SetBool("isMoving", false);
    }
}
