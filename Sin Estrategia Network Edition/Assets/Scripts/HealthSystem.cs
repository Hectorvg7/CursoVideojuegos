using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;

    [SerializeField] private int health = 100;

    private int healthMax;

    private void Start()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health = Mathf.Max(0, health - damageAmount);
        OnDamage?.Invoke(this, EventArgs.Empty);
        if (health == 0)
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
        return (float)health / (float)healthMax;
    }
}
