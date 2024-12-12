using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveState : IState
{
    private AutoPaddle player;
    public Rigidbody2D rbBola;

    // pass in any parameters you need in the constructors
    public ActiveState(AutoPaddle player)
    {
        this.player = player;
        this.rbBola = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        //1. Comprobar si debemos pasar a estado Iddle
        if (rbBola.velocity.x <= 0)
        {
            player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.iddleState);
        }
        //Hacer lo que haya que hacer en estado Activo
        SeguirBola();
    }

    private void SeguirBola()
    {
        float bolaY = rbBola.position.y;
        float paddleY = player.transform.position.y;

        if (bolaY > paddleY)
        {
            player.transform.position = new Vector3(player.transform.position.x, paddleY + player.speed * Time.deltaTime, player.transform.position.z);
        }
        else if (bolaY < paddleY)
        {
            player.transform.position = new Vector3(player.transform.position.x, paddleY - player.speed * Time.deltaTime, player.transform.position.z);
        }
    }
}
