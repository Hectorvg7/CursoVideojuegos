using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
using Unity.VisualScripting;

public class TaskPatrol : Node
{
    GhostBT ghostBT;
    UnityEngine.AI.NavMeshAgent agent;
    Vector2 posGhost;
    Vector2 posWP;
    int currentWP = 0;

    public TaskPatrol(BTree btree) : base(btree)
    {
        ghostBT = bTree as GhostBT;
        agent = ghostBT.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        agent.destination = ghostBT.wayPoints[currentWP].position;

        posGhost = new Vector2(ghostBT.transform.position.x, ghostBT.transform.position.z);
        posWP = new Vector2(ghostBT.wayPoints[currentWP].position.x, ghostBT.wayPoints[currentWP].position.z);

        if (Vector2.Distance(posGhost, posWP) < 0.1f)
        {
            currentWP++;
            //Debug.Log(currentWP);
            //Debug.Log(ghostBT.wayPoints.Count);
        }

        if (currentWP >= ghostBT.wayPoints.Count)
        {
            currentWP = 0;
        }

        state = NodeState.RUNNING;
        return state;
    }

}