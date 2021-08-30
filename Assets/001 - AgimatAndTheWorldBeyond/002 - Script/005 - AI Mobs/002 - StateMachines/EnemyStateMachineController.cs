using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachineController : MonoBehaviour
{
    public EnemyCore enemyCore;

    //DECLARE ALL OTHER STATES HERE

    public EnemyStateMachineChanger enemyStateMachineChanger;

    private void Awake()
    {
        enemyStateMachineChanger = new EnemyStateMachineChanger();
    }

    private void Start()
    {
        //Time.timeScale = 0.1f;
        // TODO: create states for each movement. Start with idle
        //enemyStateMachineChanger.Initialize(idleState);
    }

    private void Update()
    {
        enemyCore.CurrentVelocitySetter();
        enemyStateMachineChanger.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        enemyStateMachineChanger.CurrentState.PhysicsUpdate();

    }
}
