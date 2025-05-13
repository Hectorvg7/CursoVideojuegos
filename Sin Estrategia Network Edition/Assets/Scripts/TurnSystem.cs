using System;
using Unity.Netcode;
using UnityEngine;

public class TurnSystem : NetworkBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    NetworkVariable<bool> isPlayerTurn = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> turnNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Hay más de un TurnSystem en la escena");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        isPlayerTurn.OnValueChanged += OnTurnChangedCallback;
    }

    private void OnDisable()
    {
        isPlayerTurn.OnValueChanged -= OnTurnChangedCallback;
    }

    private void OnTurnChangedCallback(bool previousValue, bool newValue)
    {
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }
    

    [ServerRpc(RequireOwnership = false)]
    public void NextTurnServerRpc()
    {
        isPlayerTurn.Value = !isPlayerTurn.Value;
        
        if (!isPlayerTurn.Value)
        {
            turnNumber.Value++;
        }
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn.Value;
    }

    public bool IsEnemyTurn()
    {
        return !isPlayerTurn.Value;
    }

    public int GetTurnNumber()
    {
        return turnNumber.Value;
    }
}
