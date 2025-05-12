using System;
using Unity.Netcode;
using UnityEngine;

public class TurnSystem : NetworkBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    NetworkVariable<bool> isPlayerTurn = new NetworkVariable<bool>(true);
    private int turnNumber = 1;

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

    [Rpc(SendTo.Server)]
    public void NextTurnServerRpc()
    {
        isPlayerTurn.Value = !isPlayerTurn.Value;

        if (isPlayerTurn.Value)
        {
            turnNumber++;
        }

        // Notificar a todos los clientes que el turno ha cambiado
        OnTurnChangedClientRpc(isPlayerTurn.Value);
        
        OnTurnChanged?.Invoke(this, EventArgs.Empty); 
    }

    // Actualiza el estado del turno en todos los clientes
    [Rpc(SendTo.NotServer)]
    private void OnTurnChangedClientRpc(bool playerTurn)
    {
        isPlayerTurn.Value = playerTurn;
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
        return turnNumber;
    }


}
