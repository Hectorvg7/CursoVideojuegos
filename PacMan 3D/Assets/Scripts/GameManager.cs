using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    private GameObject playerActual;
    private GameObject[] dots;
    private int dotsRestantes;
    private GameObject[] enemies;
    private float tiempoRestante = 3f;

    //Inicializar variables.
    void Awake()
    {
        playerActual = GameObject.FindGameObjectWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        StartCoroutine(DetenerMovimiento());
    }

    // Update is called once per frame
    void Update()
    {
        ReaparecerPacMan();
        ContarPuntos();
    }

    void ContarPuntos()
    {
        dots = GameObject.FindGameObjectsWithTag("Dot");
        dotsRestantes = dots.Length;

        if (dotsRestantes <= 0)
        {
            //Código para WIN
            Debug.Log("¡Has ganado!");
        }
    }

    void ReaparecerPacMan()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerActual == null)
        {
            playerActual = Instantiate(player);
        }
    }

    IEnumerator DetenerMovimiento()
    {
        // Desactivo el movimiento de los objetos.
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<NavMeshAgent>().isStopped = true;
        }

        //Espera el tiempo del contador.
        yield return new WaitForSeconds(tiempoRestante);

        // Vuelvo a activar el movimiento de los objetos.
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }
}
