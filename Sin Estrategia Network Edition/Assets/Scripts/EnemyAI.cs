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
            foreach (Unit enemyUnit in UnitsController.Instance.GetEnemyUnitsList())
            {
                // Mientras la unidad tenga puntos de acción, sigue actuando
                while (enemyUnit.GetActionPoints() > 2)
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
                        bool isActionComplete = false;

                        bestAction.TakeAction(bestPosition, () =>
                        {
                            isActionComplete = true;
                        });

                        // Esperar a que termine la acción
                        while (!isActionComplete)
                        {
                            yield return null;
                        }

                        // Pequeña pausa entre acciones (opcional)
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        break; // No hay más acciones útiles para esta unidad
                    }
                }
            }
        // Cuando todas las unidades ya no pueden actuar
        TurnSystem.Instance.NextTurn();
        state = State.WaitingForEnemyTurn;
        }
}
