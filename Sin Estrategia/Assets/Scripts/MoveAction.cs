using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        isActive = true;
        this.onActionComplete = onActionComplete;;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
