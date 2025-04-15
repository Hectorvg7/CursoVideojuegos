using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxPointsPerTurn = 10;
    public int actionPoints;
    public int health;
    public GridPosition gridPosition;
    public BaseAction[] availableActions;
    public GameObject quads;
    private Animator animator;
    private HealthSystem healthSystem;
    [SerializeField] private Transform rifleMuzzle;

    public Transform GetRifleMuzzle() => rifleMuzzle;


    public bool isEnemy;
    private bool isSelected = false;
    private bool isMoving = false;

    void Awake()
    {
        availableActions = GetComponents<BaseAction>();
        animator = GetComponent<Animator>();
        actionPoints = maxPointsPerTurn;
        quads.SetActive(false);
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += HealthSystem_OnDamage;
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

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void LookAtSmooth(Vector3 targetPosition, float rotationSpeed = 5f, Action onComplete = null)
    {
        StartCoroutine(RotateTowards(targetPosition, rotationSpeed, onComplete));
    }

    private IEnumerator RotateTowards(Vector3 targetPosition, float speed, Action onComplete)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            onComplete?.Invoke();
            yield break;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation; // Asegura rotación final exacta

        onComplete?.Invoke(); // Llama a la acción una vez terminada la rotación
}

    public void DisminuirVida(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    private void HealthSystem_OnDamage(object sender, EventArgs e)
    {
    }
}
