using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;  // La barra de salud
    public float maxHealth = 100f;  // Salud máxima de la unidad
    private float currentHealth;  // Salud actual

    public Transform unit;
    public Vector3 offset = new Vector3(0,2,0);

    // Método para inicializar la barra de salud
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        transform.position = unit.position + offset;  
    }

    // Método para aplicar daño
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();
    }

    // Método para curar
    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // Método para actualizar la barra de salud
    private void UpdateHealthBar()
    {
        // Calculamos la proporción de la salud actual en relación a la salud máxima
        float healthPercentage = currentHealth / maxHealth;

        // Actualizamos el valor de la barra de salud
        healthBar.fillAmount = healthPercentage;
    }
}
