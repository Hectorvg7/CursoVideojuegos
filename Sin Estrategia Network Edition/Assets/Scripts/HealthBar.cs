using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    private Image healthBarImage;
    private TextMeshProUGUI healthBarText;

    public Unit unit;
    public Vector3 offset = new Vector3(0,2,0);
    

    // Método para inicializar la barra de salud
    public void Initialize(Unit owningUnit)
    {
        unit = owningUnit;
        healthBarImage = transform.GetComponent<Image>();
        healthBarText = GetComponentInChildren<TextMeshProUGUI>();

        UpdateHealthBar(unit.currentHealth.Value);

        // Suscribirse al cambio de vida
        unit.currentHealth.OnValueChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        if (unit != null)
        {
            unit.currentHealth.OnValueChanged -= OnHealthChanged;
        }
    }

    private void OnHealthChanged(int previousValue, int newValue)
    {
        UpdateHealthBar(newValue);
    }

    void Update()
    {
        if (unit != null)
        {
            transform.position = unit.transform.position + offset;  
        }
    }

    [ClientRpc]
    public void HideHealthBarClientRpc()
    {
        // Desactivar o destruir la barra de vida en el cliente
        healthBarImage.gameObject.SetActive(false);
        healthBarText.gameObject.SetActive(false);
        Debug.Log("Se han borrado las barras de vida.");
    }


    // Método para actualizar la barra de salud
    private void UpdateHealthBar(int currentHealth)
    {
        // Calculamos la proporción de la salud actual en relación a la salud máxima
        float healthPercentage = (float)currentHealth / unit.maxHealth;

        // Actualizamos el valor de la barra de salud
        healthBarImage.fillAmount = healthPercentage;
        healthBarText.text = currentHealth.ToString();

        if (currentHealth <= 0)
        {
            healthBarText.text = "";

            if (IsServer)
            {
                HideHealthBarClientRpc();
                unit.OnNetworkDespawn();
            }
        }
    }
}
