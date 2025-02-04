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
        //Creamos un nodo selector en el que hay 2 secuencias.
        //La primera secuencia comprueba si PacMan está en el rango; si es así va a por él, si no pasa a la siguiente secuencia.
        //La segunda secuencia irá al primer punto de patrulla y se esperará 2 segundos para ir al siguiente punto.
        return new Selector(
            this,
            new List<Node>
            {
                new Sequence(
                    this,
                    new List<Node> { new TaskPacmanIsOnRange(this), new TaskGoToTarget(this) }
                ),
                new Sequence(this, new List<Node> { new TaskWait(this), new TaskPatrol(this) }),
            }
        );
    }
}
