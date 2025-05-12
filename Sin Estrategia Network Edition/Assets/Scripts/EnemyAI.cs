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
                    StartCoroutine(ExecuteEnemyActions());
                    state = State.Busy;
                }
                break;

            case State.Busy:
                // Esperar a que termine la acción
                break;
        }
    }

    private IEnumerator ExecuteEnemyActions()
    {
        List<Unit> enemyUnits = new List<Unit>(UnitsController.Instance.GetEnemyUnitsList()); // Copiar la lista de unidades enemigas

        foreach (Unit enemyUnit in enemyUnits)
        {
            while (enemyUnit.GetActionPoints() > 0)
            {
                EnemyAIAction bestAIAction = null;
                BaseAction bestAction = null;

                // Buscar la mejor acción disponible entre todas las acciones de la unidad
                foreach (BaseAction action in enemyUnit.GetBaseActionArray())
                {
                    // Verifica si la unidad tiene suficientes puntos para realizar esta acción
                    if (enemyUnit.GetActionPoints() < action.GetActionPointsCost())
                        continue;

                    EnemyAIAction aiAction = action.GetBestEnemyAIAction();
                    if (aiAction != null && (bestAIAction == null || aiAction.actionValue > bestAIAction.actionValue))
                    {
                        bestAIAction = aiAction;
                        bestAction = action;
                    }
                }

                if (bestAIAction != null && bestAction != null)
                {
                    bool isActionComplete = false;

                    bestAction.TakeAction(bestAIAction.gridPosition, () =>
                    {
                        isActionComplete = true;
                    });

                    while (!isActionComplete)
                    {
                        yield return null;
                    }

                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    break; // No hay más acciones posibles
                }
            }
        TurnSystem.Instance.NextTurnServerRpc();
        state = State.WaitingForEnemyTurn;
        }
    }
}
