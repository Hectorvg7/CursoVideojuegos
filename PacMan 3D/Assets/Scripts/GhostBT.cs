using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class GhostBT : BTree
{
    public List<Transform> wayPoints;
    public LayerMask pacmanLayer;

    protected override Node SetupTree()
    {
        //Creamos un nodo selector en el que hay una secuencia y el nodo Patrol.
        //La secuencia comprueba si PacMan está en el rango; si es así va a por él, si no sigue patrullando.
        return new Selector(
            this,
            new List<Node>
            {
                new Sequence(
                    this,
                    new List<Node> { new TaskPacmanIsOnRange(this), new TaskGoToTarget(this) }
                ),
                new TaskPatrol(this),
            }
        );
    }
}
