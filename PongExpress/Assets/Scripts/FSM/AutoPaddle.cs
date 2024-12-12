using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class AutoPaddle : Paddle
{
    public AutoPaddleFSM PlayerStateMachine { get; private set; }

    void OnEnable()
    {
        PlayerStateMachine = new AutoPaddleFSM(this);
        PlayerStateMachine.Initialize(PlayerStateMachine.iddleState);
    }

    private void Update()
    {
        PlayerStateMachine.Update();
    }

    private void FixedUpdate()
    {
        PlayerStateMachine.FixedUpdate();
    }
}
