using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Contador : MonoBehaviour
{
    public TextMeshProUGUI textContador;
    public float tiempoRestante = 3f;
    private bool cuentaActiva = false;

    // Start is called before the first frame update
    void Start()
    {
        // Empezamos la cuenta regresiva cuando comienza el juego
        cuentaActiva = true;
        textContador.gameObject.SetActive(true); // Asegurarse de que el texto esté visible
    }

    // Update is called once per frame
    void Update()
    {
        // Solo actualizar si la cuenta regresiva está activa
        if (cuentaActiva)
        {
            // Restamos el tiempo
            tiempoRestante -= Time.deltaTime;

            // Actualizamos el texto con el tiempo restante (convertido a entero)
            textContador.text = Mathf.Ceil(tiempoRestante).ToString();

            // Cuando la cuenta llegue a 0
            if (tiempoRestante <= 0f)
            {
                textContador.text = "";
                cuentaActiva = false;

                textContador.gameObject.SetActive(false);
            }
        }
    }
}
