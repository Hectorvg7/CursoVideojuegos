using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskGoToTarget : Node
{
    GhostBT ghostBT;
    UnityEngine.AI.NavMeshAgent agent;

    public TaskGoToTarget(BTree btree)
        : base(btree)
    {
        ghostBT = bTree as GhostBT;
        agent = ghostBT.transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public override NodeState Evaluate()
    {
        GameObject target = (GameObject)bTree.GetData("target");
        if (target != null)
        {
            agent.destination = target.transform.position;
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
        }

        return state;
    }
}
