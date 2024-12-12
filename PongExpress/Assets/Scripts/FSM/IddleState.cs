using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IddleState : IState
{
    private AutoPaddle player;
    public Rigidbody2D rbBola;

    // pass in any parameters you need in the constructors
    public IddleState(AutoPaddle player)
    {
        this.player = player;
        this.rbBola = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        //1. Comprobar si debemos pasar a estado Activo
        if (rbBola.velocity.x > 0)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.activeState);
        }
        //Hacer lo que haya que hacer en estado iddle
        MoverCentro();
    }

    private void MoverCentro()
    {
        float centroY = 0f;
        float palaY = player.transform.position.y;

        if (palaY < centroY)
        {
            player.transform.position = new Vector3(player.transform.position.x, palaY + player.speed * Time.deltaTime, player.transform.position.z);
        }
        else if (palaY > centroY)
        {
            player.transform.position = new Vector3(player.transform.position.x, palaY - player.speed * Time.deltaTime, player.transform.position.z);
        }
    }
}
