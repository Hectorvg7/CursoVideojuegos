using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthSystem : NetworkBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;

    private NetworkVariable<int> health;

    [SerializeField]
    private int healthMax = 100;

    private void Awake()
    {
        health = new NetworkVariable<int>(healthMax);
    }

    public void Damage(int damageAmount)
    {
        SetHealth(Mathf.Max(0, health.Value - damageAmount));
        OnDamage?.Invoke(this, EventArgs.Empty);
        if (health.Value == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health.Value / (float)healthMax;
    }

    private void SetHealth(int newHealth)
    {
        if (IsServer)
        {
            health.Value = newHealth;
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            health.OnValueChanged += Health_OnValueChanged;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer)
        {
            Die();
            health.OnValueChanged -= Health_OnValueChanged;
        }
    }

    private void Health_OnValueChanged(int previousValue, int newValue)
    {
        if (newValue < previousValue)
        {
            OnDamage?.Invoke(this, EventArgs.Empty);
        }
        if (newValue == 0)
        {
            Die();
        }
    }
}
