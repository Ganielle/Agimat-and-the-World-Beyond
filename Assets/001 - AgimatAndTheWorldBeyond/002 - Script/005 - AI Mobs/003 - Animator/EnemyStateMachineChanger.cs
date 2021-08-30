using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachineChanger 
{
    public EnemyStatesController CurrentState { get; private set; }

    public void Initialize(EnemyStatesController startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(EnemyStatesController newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
