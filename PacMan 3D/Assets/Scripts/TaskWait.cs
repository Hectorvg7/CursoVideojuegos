using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;

public class TaskWait : Node
{
    GhostBT ghostBT;
    UnityEngine.AI.NavMeshAgent agent;
    private float tiempoEspera = 2f;
    private float tiempoActual = 0;

    public TaskWait(BTree btree)
        : base(btree)
    {
        ghostBT = bTree as GhostBT;
    }

    public override NodeState Evaluate()
    {
        if (tiempoActual < tiempoEspera)
        {
            Debug.Log("Está esperando");
            tiempoActual += Time.deltaTime;
            state = NodeState.RUNNING;
        }

        if (tiempoActual >= tiempoEspera)
        {
            Debug.Log("Ya ha esperado");
            //Corregir para que siga patrullando cuando acabe de esperar.
            bTree.SetData("wait", false);
            tiempoActual = 0;
            state = NodeState.SUCCESS;
            return state;
        }
        return state;
    }
}
