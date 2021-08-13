using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachinesController : MonoBehaviour
{
    public Core core;

    //  StateMachines
    public PlayerStateMachineChanger stateMachine;
    public PlayerIdleState idleState;
    public PlayerLookUpState lookingUpState;
    public PlayerLookingDownState lookingDownState;
    public PlayerTauntIdleState tauntIdleState;
    public PlayerChangeIdleDirectionState changeIdleDirectionState;
    public PlayerMoveState moveState;
    public PlayerSteepSlopeSlideState steepSlopeSlide;
    public PlayerRunningBreak runningBreakState;
    public PlayerJumpState jumpState;
    public PlayerLowLandState lowLandState;
    public PlayerHighLandState highLandState;
    public PlayerInAirState inAirState;
    public PlayerWallSlideState wallSlideState;
    public PlayerWallGrabState wallGrabState;
    public PlayerWallClimbState wallClimbState;
    public PlayerWallJumpState wallJumpState;
    public PlayerLedgeClimbState ledgeClimbState;
    public PlayerMonkeyBarGrabState monkeyBarGrab;
    public PlayerMonkeyBarMove monkeyBarMove;
    public PlayerMonkeyBarJumpState monkeyBarJump;
    public PlayerRopeStartGrabState ropeStartGrab;
    public PlayerRopeGrabSwingState ropeGrabSwing;
    public PlayerRopeClimbUpState ropeClimbUp;
    public PlayerRopeClimbDownState ropeClimbDown;
    public PlayerRopeJump ropeJumpState;
    public PlayerSwitchState switchPlayerState;
    public PlayerDashState playerDashState;
    public PlayerSprintState playerSprintState;
    public PlayerDodgeState playerDodgeState;
    public PlayerWeaponSwitchState weaponSwitchState;

    //  Switching Weapons
    [ReadOnly] public float lastShowWeaponSlotsTime;

    private void Awake()
    {
        stateMachine = new PlayerStateMachineChanger();
        idleState = new PlayerIdleState(this, stateMachine, core.playerRawData, "idle");
        lookingUpState = new PlayerLookUpState(this, stateMachine, core.playerRawData, "lookingUp");
        lookingDownState = new PlayerLookingDownState(this, stateMachine, core.playerRawData, "lookingDown");
        tauntIdleState = new PlayerTauntIdleState(this, stateMachine, core.playerRawData, "tauntIdle");
        changeIdleDirectionState = new PlayerChangeIdleDirectionState(this, stateMachine,
            core.playerRawData, "idleChangeDirection");
        moveState = new PlayerMoveState(this, stateMachine, core.playerRawData, "move");
        steepSlopeSlide = new PlayerSteepSlopeSlideState(this, stateMachine, core.playerRawData, "slopeSlide");
        runningBreakState = new PlayerRunningBreak(this, stateMachine, core.playerRawData, "runningBreak");
        jumpState = new PlayerJumpState(this, stateMachine, core.playerRawData, "inAir");
        inAirState = new PlayerInAirState(this, stateMachine, core.playerRawData, "inAir");
        lowLandState = new PlayerLowLandState(this, stateMachine, core.playerRawData, "lowLand");
        highLandState = new PlayerHighLandState(this, stateMachine, core.playerRawData, "highLand");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, core.playerRawData, "wallSlide");
        wallClimbState = new PlayerWallClimbState(this, stateMachine, core.playerRawData, "wallClimb");
        wallGrabState = new PlayerWallGrabState(this, stateMachine, core.playerRawData, "wallGrab");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, core.playerRawData, "inAir");
        ledgeClimbState = new PlayerLedgeClimbState(this, stateMachine, core.playerRawData, "ledgeClimbState");
        monkeyBarGrab = new PlayerMonkeyBarGrabState(this, stateMachine, core.playerRawData, "monkeyBarIdle");
        monkeyBarMove = new PlayerMonkeyBarMove(this, stateMachine, core.playerRawData, "monkeyBarMove");
        monkeyBarJump = new PlayerMonkeyBarJumpState(this, stateMachine, core.playerRawData, "inAir");
        ropeStartGrab = new PlayerRopeStartGrabState(this, stateMachine, core.playerRawData, "ropeStartGrab");
        ropeGrabSwing = new PlayerRopeGrabSwingState(this, stateMachine, core.playerRawData, "ropeGrabSwing");
        ropeClimbUp = new PlayerRopeClimbUpState(this, stateMachine, core.playerRawData, "ropeClimb");
        ropeClimbDown = new PlayerRopeClimbDownState(this, stateMachine, core.playerRawData, "ropeSlide");
        ropeJumpState = new PlayerRopeJump(this, stateMachine, core.playerRawData, "inAir");
        switchPlayerState = new PlayerSwitchState(this, stateMachine, core.playerRawData, "currentSwitching");
        playerDashState = new PlayerDashState(this, stateMachine, core.playerRawData, "chargeDash");
        playerSprintState = new PlayerSprintState(this, stateMachine, core.playerRawData, "sprinting");
        playerDodgeState = new PlayerDodgeState(this, stateMachine, core.playerRawData, "dodge");
        weaponSwitchState = new PlayerWeaponSwitchState(this, stateMachine, core.playerRawData,
            GameManager.instance.PlayerStats.GetSetWeaponEquipBoolInPlayerAnim);

        switchPlayerState.ResetSwitch();
        weaponSwitchState.ResetWeaponSwitch();
        playerDashState.ResetCanDash();
        playerDodgeState.ResetDodge();
        ledgeClimbState.ResetCanLedgeClimb();

        GameManager.instance.gameInputController.onSwitchWeaponInputChange += WeaponInputChange;
    }

    private void OnDisable()
    {
        GameManager.instance.gameInputController.onSwitchWeaponInputChange -= WeaponInputChange;
    }

    private void WeaponInputChange(object sender, EventArgs e)
    {
        if (GameManager.instance.gameInputController.GetWeaponSwitchInput > 0)
            lastShowWeaponSlotsTime = Time.time;
    }

    private void Start()
    {
        //Time.timeScale = 0.1f;
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        core.CurrentVelocitySetter();
        stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PhysicsUpdate();
    }
}
