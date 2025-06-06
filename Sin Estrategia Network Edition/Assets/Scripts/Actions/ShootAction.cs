using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
                    onActionComplete?.Invoke();
                });
            }
        }
    }

    public void ShootTo(GridPosition newGridPosition)
    {
        bool isValid = LevelGrid.Instance.IsValidGridPosition(newGridPosition);
        bool hasTarget = TurnSystem.Instance.IsPlayerTurn()
            ? LevelGrid.Instance.HasAnyEnemyUnitOnGridPosition(newGridPosition)
            : LevelGrid.Instance.HasAnyUnitOnGridPosition(newGridPosition);

        if (!isValid || !hasTarget)
        {
            Debug.Log("La casilla no es válida.");
            return;
        }

        Unit targetUnit = LevelGrid.Instance.GetUnitListAtGridPosition(newGridPosition)[0];
        if (targetUnit == null) return;

        Transform muzzleTransform = unit.GetRifleMuzzle();
        ShootServerRpc(muzzleTransform.position, targetUnit.transform.position);

        unit.actionPoints.Value -= GetActionPointsCost();
        targetUnit.TakeDamage(fireDamage);
    }

    [ServerRpc]
    public void ShootServerRpc(Vector3 spawnPosition, Vector3 targetPosition)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        var bulletNet = bullet.GetComponent<NetworkObject>();
        bulletNet.Spawn(); // hace que la bala se vea en todos los clientes

        bullet.GetComponent<Bullet>().SetTarget(targetPosition);
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

    public void RequestShoot(GridPosition gridPosition)
    {
        if (IsClient)
        {
            RequestShootActionServerRpc(gridPosition);
        }
        else if (IsServer)
        {
            TakeAction(gridPosition, () => { });
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void RequestShootActionServerRpc(GridPosition targetGridPosition, ServerRpcParams rpcParams = default)
    {
        TakeAction(targetGridPosition, () => { }); // Acción del Host
    }

}
