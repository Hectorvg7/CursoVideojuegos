using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour
{

    public Transform puntoDeDestino;  // El punto de destino para el teletransporte

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (puntoDeDestino.position.x < 0)
            {
                other.GetComponent<NavMeshAgent>().Warp(puntoDeDestino.position + Vector3.right);
            }

            if (puntoDeDestino.position.x > 0)
            {
                other.GetComponent<NavMeshAgent>().Warp(puntoDeDestino.position + Vector3.left);
            }
            // Teletransportar al jugador al otro lado
            
            Debug.Log("Teleport loading...");
        }
    }
}
