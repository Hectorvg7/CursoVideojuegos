using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;

public class TaskPatrol : Node
{
    GhostBT ghostBT;
    UnityEngine.AI.NavMeshAgent agent;
    Vector2 posGhost;
    Vector2 posWP;
    int currentWP = 0;
    public bool isWaiting = false;

    public TaskPatrol(BTree btree)
        : base(btree)
    {
        ghostBT = bTree as GhostBT;
        agent = ghostBT.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        isWaiting = bTree.GetData("wait") as bool? ?? false;
        //Si está esperando nos aseguramos de que no se mueva.
        if (isWaiting)
        {
            state = NodeState.RUNNING;
            return state;
        }

        //Establecemos el destino.
        agent.destination = ghostBT.wayPoints[currentWP].position;

        posGhost = new Vector2(ghostBT.transform.position.x, ghostBT.transform.position.z);
        posWP = new Vector2(
            ghostBT.wayPoints[currentWP].position.x,
            ghostBT.wayPoints[currentWP].position.z
        );

        //Si hemos llegado al punto de patrulla cambiamos el punto de patrulla y pasamos a esperar.
        if (Vector2.Distance(posGhost, posWP) < 0.1f)
        {
            currentWP++;
            isWaiting = true;
            bTree.SetData("wait", isWaiting);
            state = NodeState.SUCCESS;
        }

        if (currentWP >= ghostBT.wayPoints.Count)
        {
            currentWP = 0;
        }

        return state;
    }
}
