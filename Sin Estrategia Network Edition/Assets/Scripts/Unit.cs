using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : NetworkBehaviour
{
    public int maxPointsPerTurn = 10;
    public NetworkVariable<int> actionPoints = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);

    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public int maxHealth = 100;
    public GridPosition gridPosition;
    public BaseAction[] availableActions;
    public GameObject quads;
    private Animator animator;

    private HealthSystem healthSystem;
    public GameObject healthBar;
    private HealthBar healthBarActions;
    public Vector3 offset = new Vector3(0,2,0);

    [SerializeField] private Transform rifleMuzzle;
    public Transform GetRifleMuzzle() => rifleMuzzle;

    public GameObject ragdollAllyPrefab;
    public GameObject ragdollEnemyPrefab;


    public bool isEnemy;
    private bool isSelected = false;
    private bool isMoving = false;

    void Awake()
    {
        availableActions = GetComponents<BaseAction>();
        animator = GetComponent<Animator>();
        actionPoints.Value = maxPointsPerTurn;
        quads.SetActive(false);
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += HealthSystem_OnDamage;
        healthSystem.OnDead += HealthSystem_OnDead;

        GameObject hb = Instantiate(healthBar, transform.position + offset, Quaternion.identity);
        hb.transform.SetParent(GameObject.Find("CanvasHealth").transform); // Establecer el CanvasHealth como padre.
        healthBarActions = hb.GetComponent<HealthBar>();
        healthBarActions.Initialize(this); // Le pasamos esta unidad.
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
        return actionPoints.Value >= action.GetActionPointsCost();
    }

    public int GetActionPoints()
    {
        return actionPoints.Value;
    }

    public void ResetActionPoints()
    {
        if (IsServer)
        {
            actionPoints.Value = maxPointsPerTurn;
        }
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



    public void TakeDamage(int damageAmount)
    {
        if (!IsServer) return;

        healthSystem.Damage(damageAmount);
        currentHealth.Value -= damageAmount;
        if (currentHealth.Value < 0) currentHealth.Value = 0;
    }

    private void HealthSystem_OnDamage(object sender, EventArgs e)
    {

    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(this, gridPosition);

          // Instanciar el ragdoll solo desde el servidor
        if (IsServer) // Aseguramos que solo el servidor instancie el ragdoll
        {
            InstantiateRagdollClientRpc(transform.position, transform.rotation);
        }


        StartCoroutine(DestroyUnitAfterRpc());
    }

    [ClientRpc]
    private void InstantiateRagdollClientRpc(Vector3 position, Quaternion rotation)
    {
        GameObject ragdollPrefab = isEnemy ? ragdollEnemyPrefab : ragdollAllyPrefab;
        if (ragdollPrefab != null)
        {
            Instantiate(ragdollPrefab, position, rotation);
        }
    }


    private IEnumerator DestroyUnitAfterRpc()
    {
        // Espera un frame para asegurarse de que el RPC se haya enviado
        yield return new WaitForSeconds(0.05f);

        // Destruir la unidad
        OnNetworkDespawn(); // Desespawnea la unidad en la red
        Destroy(gameObject); // Destruye el GameObject

        // Actualizar las listas de unidades
        UnitsController.Instance.GetUnitsList();
        UnitsController.Instance.GetEnemyUnitsList();
    }
}
