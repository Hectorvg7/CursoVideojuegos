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

    private bool isSelected = false;

    void Awake()
    {
        availableActions = GetComponents<BaseAction>();
        actionPoints = maxPointsPerTurn;
        quads.SetActive(false);
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

    public void TryTakeAction(BaseAction action)
    {
        if (CanSpendPointsToTakeAction(action))
        {
            action.TakeAction(gridPosition, ClearBusy);
            actionPoints -= action.GetActionPointsCost();
        } 
        else 
        {
            Debug.Log("No tienes suficientes puntos para realizar la acción.");
        }
    }

    private void ClearBusy(){}
}
