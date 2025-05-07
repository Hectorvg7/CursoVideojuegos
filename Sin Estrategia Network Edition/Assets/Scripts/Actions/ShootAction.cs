using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private int fireRange = 3; //Rango máximo de disparo
    private int fireDamage = 25;
    [SerializeField] private GameObject bulletPrefab;

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidGridPositionList()
    {
        return new List<GridPosition> { unit.GetGridPosition() };
    }

    public override int GetActionPointsCost()
    {
        return 3;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        // Verificamos si la casilla está dentro del rango de 3 casillas
        int distance = Mathf.Abs(gridPosition.x - unit.GetGridPosition().x) + Mathf.Abs(gridPosition.z - unit.GetGridPosition().z);
        if (distance <= fireRange) // Solo permitimos disparar dentro de un rango de 3 casillas
        {
            Unit targetUnit = LevelGrid.Instance.GetUnitListAtGridPosition(gridPosition)[0];
            if (targetUnit != null)
            {
                // Primero giramos suavemente y luego disparamos
                unit.LookAtSmooth(targetUnit.transform.position, 10f, () =>
                {
                    ShootTo(gridPosition);
                    unit.Shoot(); // animación u otros efectos
                });
            }
        }
    }

    public void ShootTo(GridPosition newGridPosition)
    {
        //Turno del Player
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            if (LevelGrid.Instance.IsValidGridPosition(newGridPosition) && LevelGrid.Instance.HasAnyEnemyUnitOnGridPosition(newGridPosition))
            {
                Unit targetUnit = LevelGrid.Instance.GetUnitListAtGridPosition(newGridPosition)[0];

                if (targetUnit != null)
                {
                    // Instanciar la bala en la punta del rifle
                    Transform muzzleTransform = unit.GetRifleMuzzle();
                    GameObject bulletGO = GameObject.Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);

                    // Mover la bala hacia la unidad enemiga
                    Bullet bullet = bulletGO.GetComponent<Bullet>();
                    bullet.SetTarget(targetUnit.transform.position);

                    unit.actionPoints -= GetActionPointsCost();
                    targetUnit.DisminuirVida(fireDamage);
                }
            }
            else
            {
                Debug.Log("La casilla no es válida.");
            }
        }
        //Turno de EnemyAI
        else
        {
            if (LevelGrid.Instance.IsValidGridPosition(newGridPosition) && LevelGrid.Instance.HasAnyUnitOnGridPosition(newGridPosition))
            {
                Unit targetUnit = LevelGrid.Instance.GetUnitListAtGridPosition(newGridPosition)[0];

                if (targetUnit != null)
                {
                    // Instanciar la bala en la punta del rifle
                    Transform muzzleTransform = unit.GetRifleMuzzle();
                    GameObject bulletGO = GameObject.Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);

                    // Mover la bala hacia la unidad enemiga
                    Bullet bullet = bulletGO.GetComponent<Bullet>();
                    bullet.SetTarget(targetUnit.transform.position);

                    unit.actionPoints -= GetActionPointsCost();
                    targetUnit.DisminuirVida(fireDamage);
                }
            }
        }
    }

    public override EnemyAIAction GetBestEnemyAIAction()
    {
        EnemyAIAction bestAction = null;

        // Obtener todas las unidades aliadas del mapa (enemigo solo dispara a aliados)
        List<Unit> allUnits = UnitsController.Instance.GetUnitsList();
        foreach (Unit targetUnit in allUnits)
        {
            if (targetUnit.isEnemy) continue; // Saltar enemigos (solo queremos aliados)

            int distance = Mathf.Abs(targetUnit.GetGridPosition().x - unit.GetGridPosition().x) + Mathf.Abs(targetUnit.GetGridPosition().z - unit.GetGridPosition().z);

            // Solo considerar si está en rango de disparo
            if (distance <= fireRange)
            {
                int score = 100;

                if (bestAction == null || score > bestAction.actionValue)
                {
                    bestAction = new EnemyAIAction
                    {
                        gridPosition = targetUnit.GetGridPosition(),
                        actionValue = score,
                    };
                }
            }
        }
        return bestAction;
    }

}
