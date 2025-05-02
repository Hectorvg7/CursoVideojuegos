using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private int fireRange = 3; //Rango máximo de disparo
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
        if (LevelGrid.Instance.IsValidGridPosition(newGridPosition) && LevelGrid.Instance.HasAnyEnemyUnitOnGridPosition(newGridPosition))
        {
            Unit targetUnit = LevelGrid.Instance.GetUnitListAtGridPosition(newGridPosition)[0];

            if (targetUnit != null)
            {
                Debug.Log("Has disparado");

                // Instanciar la bala en la punta del rifle
                Transform muzzleTransform = unit.GetRifleMuzzle();
                GameObject bulletGO = GameObject.Instantiate(bulletPrefab, muzzleTransform.position, Quaternion.identity);

                // Mover la bala hacia la unidad enemiga
                Bullet bullet = bulletGO.GetComponent<Bullet>();
                bullet.SetTarget(targetUnit.transform.position);

                unit.actionPoints -= GetActionPointsCost();
                targetUnit.DisminuirVida(25);
            }
        }
        else
        {
            Debug.Log("La casilla no es válida.");
        }
    }
}
