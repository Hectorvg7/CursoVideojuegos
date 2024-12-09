using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneradorNivel : MonoBehaviour
{
    public int dificultad;  // Este valor se incrementa cada vez que se supera un nivel
    public GameObject ladrilloPrefab; // El prefab del ladrillo
    public float espacioEntreLadrillos = 1.5f; // Espacio entre ladrillos
    public int filasMinimas = 3;
    public int filasMaximas = 8;
    public int ladrillosMinimosPorFila = 3;
    public int ladrillosMaximosPorFila = 8;
    public float alturaMinima = -1f;
    public float alturaMaxima = 4f;
    public int minVidas = 1;
    public int maxVidas = 4;
    //public Bloques bloquesManager;

    void Start()
    {
        GenerarNivel(dificultad);
    }

    void Update()
    {
        
        if(GameManager.Instance.subirDificultad || Input.GetKeyDown(KeyCode.R))
        {
            GameManager.Instance.BorrarBloques();
            GameManager.Instance.subirDificultad = false;
            dificultad++;
            GenerarNivel(dificultad);
        }


    }

    public void GenerarNivel(int dificultad)
    {
        // Determinar el número de filas de ladrillos en función de la dificultad
        int numFilas = Mathf.Clamp(filasMinimas + dificultad, filasMinimas, filasMaximas);

        // Iterar sobre las filas de ladrillos
        for (int i = 0; i < numFilas; i++)
        {
            // Determinar el número de ladrillos en esta fila
            int numLadrillos = Mathf.Clamp(ladrillosMinimosPorFila + dificultad, ladrillosMinimosPorFila, ladrillosMaximosPorFila);

            // Determinar el color de la fila de ladrillos (puedes usar colores aleatorios o una paleta definida)
            Color colorFila = new Color(Random.value, Random.value, Random.value); // Color aleatorio

            // Determinar la posición Y de la fila, de acuerdo con el índice de la fila
            float yPos = Mathf.Lerp(alturaMinima, alturaMaxima, (float)i / (numFilas - 1)); // Espaciado uniforme entre las filas

            // Crear los ladrillos de esta fila
            CrearFilaDeLadrillos(numLadrillos, colorFila, yPos);
        }
    }

    void CrearFilaDeLadrillos(int numLadrillos, Color color, float yPos)
    {
        // Calcular el rango X para distribuir los ladrillos uniformemente
        float minXPos = -8f;
        float maxXPos = 8f;
        
        // Distribuir los ladrillos de manera uniforme
        for (int i = 0; i < numLadrillos; i++)
        {
            // Calcular la posición X de cada ladrillo de forma interpolada
            float xPos = Mathf.Lerp(minXPos, maxXPos, (float)i / (numLadrillos - 1));

            // Crear el ladrillo en la posición calculada
            GameObject ladrillo = Instantiate(ladrilloPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
            Renderer renderer = ladrillo.GetComponent<Renderer>();
            renderer.material.color = color; // Aplicar el color a la fila

            // Asignar las vidas al ladrillo, que aumentarán con la dificultad
            float porcentajeAltura = Mathf.InverseLerp(alturaMinima, alturaMaxima, yPos);
            int vidasLadrillo = Mathf.RoundToInt(Mathf.Lerp(minVidas, maxVidas, porcentajeAltura));

            Bloques bloqueScript = ladrillo.GetComponent<Bloques>();
            bloqueScript.vidasRestantes = vidasLadrillo;

            Debug.Log($"Bloque en Y:{yPos} tiene {vidasLadrillo} vidas.");
        }
    }
}
