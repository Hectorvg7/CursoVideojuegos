using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class TaskPacmanIsOnRange : Node
{
    GhostBT ghostBT;

    public float rangoDeteccion = 4f;
    LayerMask pacmanLayer;

    public TaskPacmanIsOnRange(BTree btree)
        : base(btree)
    {
        ghostBT = bTree as GhostBT;
        pacmanLayer = ghostBT.pacmanLayer;
    }

    public override NodeState Evaluate()
    {
        Collider[] colliders = Physics.OverlapSphere(
            ghostBT.transform.position,
            rangoDeteccion,
            pacmanLayer
        );

        if (colliders.Length > 0)
        {
            Debug.Log("PacMan ha sido detectado");
            GameObject pacman = colliders[0].gameObject;
            bTree.SetData("target", pacman);
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
        }

        return state;
    }
}
