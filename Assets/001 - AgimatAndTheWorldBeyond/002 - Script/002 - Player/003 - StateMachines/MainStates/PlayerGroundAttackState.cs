using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttackState : PlayerStatesController
{
    protected bool isGrounded;
    protected bool canWalkOnSlope;
    protected bool isFrontFootTouchSlope;
    protected bool canTransition;

    protected int lastFacingDirection;

    public PlayerGroundAttackState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData,
        string animBoolName, bool isBoolAnim) : base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        //  Environment Checkers
        isGrounded = statemachineController.core.groundPlayerController.CheckIfTouchGround;
        canWalkOnSlope = statemachineController.core.groundPlayerController.canWalkOnSlope;
        isFrontFootTouchSlope = statemachineController.core.groundPlayerController.CheckIfFrontTouchingSlope;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            //  In air not grounded or falling
            if (!isGrounded)
            {
                statemachineController.core.attackController.attackComboIndex = 0;
                statemachineChanger.ChangeState(statemachineController.inAirState);
            }

            //  Slope slide
            else if (!canWalkOnSlope && isFrontFootTouchSlope)
            {
                statemachineController.core.attackController.attackComboIndex = 0;
                statemachineChanger.ChangeState(statemachineController.steepSlopeSlide);
            }

            else if (canTransition)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                {
                    canTransition = false;
                    statemachineChanger.ChangeState(statemachineController.idleState);
                }

                else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                {
                    if (lastFacingDirection != GameManager.instance.gameplayController.GetSetMovementNormalizeX)
                     statemachineController.core.attackController.attackComboIndex = 0;

                    statemachineChanger.ChangeState(statemachineController.moveState);
                    canTransition = false;
                }
            }

            else if (statemachineController.core.attackController.canChangeDirectionWhileAttacking)
            {
                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0)
                {
                    if (lastFacingDirection != GameManager.instance.gameplayController.GetSetMovementNormalizeX)
                        statemachineController.core.attackController.attackComboIndex = 0;

                    statemachineController.core.attackController.canChangeDirectionWhileAttacking = false;
                    statemachineController.core.CheckIfShouldFlip(GameManager.instance.gameplayController.GetSetMovementNormalizeX);
                }
            }

            //  For cancel animation gameplay
            else if (statemachineController.core.attackController.canCancelAnimation)
            {
                if (GameManager.instance.gameplayController.jumpInput)
                {
                    statemachineController.core.attackController.canCancelAnimation = false;
                    statemachineController.core.attackController.attackComboIndex = 0;
                    statemachineChanger.ChangeState(statemachineController.jumpState);
                    GameManager.instance.gameplayController.UseJumpInput();
                }

                else if (GameManager.instance.gameplayController.dodgeInput)
                {
                    statemachineController.core.attackController.canCancelAnimation = false;
                    statemachineController.core.attackController.attackComboIndex = 0;
                    statemachineChanger.ChangeState(statemachineController.playerDodgeState);
                }

                else if (statemachineController.playerDashState.CheckIfCanDash() &&
                statemachineController.core.groundPlayerController.canWalkOnSlope &&
                GameManager.instance.gameplayController.dashInput &&
                !GameManager.instance.gameplayController.switchPlayerLeftInput)
                {
                    statemachineController.core.attackController.canCancelAnimation = false;
                    statemachineController.core.attackController.attackComboIndex = 0;
                    statemachineChanger.ChangeState(statemachineController.playerDashState);
                }
            }
        }
    }
}
