using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount = 0f;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        totalSpinAmount += spinAddAmount;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        if (totalSpinAmount >= 90f)
        {
            transform.eulerAngles -= new Vector3(0, totalSpinAmount - 90f, 0);
            isActive = false;
            onActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition)
    {
        isActive = true;
        totalSpinAmount = 0;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        return new List<GridPosition> { unit.GetGridPosition() };
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}
