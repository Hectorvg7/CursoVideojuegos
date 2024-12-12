using System;

public class AutoPaddleFSM
{
    public IState CurrentState { get; private set; }
    public IddleState iddleState { get; private set; }
    public ActiveState activeState { get; private set; }

    public AutoPaddleFSM(AutoPaddle player)
    {
        this.iddleState = new IddleState(player);
        this.activeState = new ActiveState(player);
    }

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    public void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.FixedUpdate();
        }
    }
}
