using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Update()
    {
        if (!TurnSystem.Instance.IsEnemyTurn()) return;

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                timer = 1f;
                state = State.TakingTurn;
                break;

            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryExecuteEnemyAction())
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // Fin del turno enemigo
                        TurnSystem.Instance.NextTurn();
                        state = State.WaitingForEnemyTurn;
                    }
                }
                break;

            case State.Busy:
                // Esperar a que termine la acción
                break;
        }
    }

    private bool TryExecuteEnemyAction()
    {
        foreach (Unit enemyUnit in UnitsController.Instance.GetEnemyUnitsList())
        {
            BaseAction bestAction = null;
            GridPosition bestPosition = new GridPosition();
            int bestScore = -1;

            foreach (BaseAction action in enemyUnit.GetBaseActionArray())
            {
                EnemyAIAction aiAction = action.GetBestEnemyAIAction();
                if (aiAction != null && aiAction.actionValue > bestScore)
                {
                    bestScore = aiAction.actionValue;
                    bestAction = action;
                    bestPosition = aiAction.gridPosition;
                }
            }

            if (bestAction != null)
            {
                bestAction.TakeAction(bestPosition, () => { state = State.TakingTurn; });
                return true;
            }
        }

        return false;
    }
}
