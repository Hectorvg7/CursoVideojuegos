using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
GameObject player;
NavMeshAgent agente;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) agente.destination = player.transform.position;
    }
}
