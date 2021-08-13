using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachineChanger
{
    public PlayerStatesController CurrentState { get; private set; }

    public void Initialize(PlayerStatesController startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerStatesController newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
