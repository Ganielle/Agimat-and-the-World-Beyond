using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerStatesController
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isFootTouchGround;
    protected bool isFrontFootTouchSlope;
    protected Vector2 checkPos;

    private bool isTouchingLedge;

    public PlayerGroundState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, 
        string animBoolName) : base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = statemachineController.core.groundPlayerController.CheckIfTouchGround;
        isFootTouchGround = statemachineController.core.groundPlayerController.CheckIfFrontFootTouchGround;
        isFrontFootTouchSlope = statemachineController.core.groundPlayerController.CheckIfFrontTouchingSlope;
        isTouchingWall = statemachineController.core.groundPlayerController.CheckIfTouchClimbWall;
        isTouchingLedge = statemachineController.core.groundPlayerController.CheckIfTouchingLedge;
        checkPos = statemachineController.transform.position - (Vector3)(new Vector2(0f,
            statemachineController.core.colliderSize.y / 2));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        AnimationChanger();

        statemachineController.core.weaponChangerController.DoneSwitchingWeapon();
        statemachineController.core.weaponChangerController.SwitchWeapon();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //  Slope Calculation
        statemachineController.core.groundPlayerController.CalculateSlopeForward();
        statemachineController.core.groundPlayerController.CalculateGroundAngle();

        statemachineController.core.groundPlayerController.SlopeChecker();
    }

    private void AnimationChanger()
    {
        if (!isExitingState)
        {
            #region ATTACK COMBOS
            if (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != PlayerStats.AnimatorStateInfo.LOOKINGUP &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != PlayerStats.AnimatorStateInfo.LOOKINGDOWN &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != PlayerStats.AnimatorStateInfo.SWITCHING &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != PlayerStats.AnimatorStateInfo.DASHCHARGE &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != PlayerStats.AnimatorStateInfo.DASHBURST &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo != PlayerStats.AnimatorStateInfo.HIGHLAND)
            {
                if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
                {
                    if (GameManager.instance.PlayerInventory.GetLukasWeapons[GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex].CurrentWeaponType
                        == PlayerWeaponRawData.WeaponType.AXE && GameManager.instance.gameplayController.attackInput)
                    {
                        statemachineController.core.attackComboIndex++;
                        statemachineController.normalAttackState.SetComboIndexParameter("axeAttackCombo");
                        statemachineChanger.ChangeState(statemachineController.normalAttackState);
                        GameManager.instance.gameplayController.UseAttackInput();
                    }
                }
                else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
                {
                    //  TODO LILY ATTACK COMBO
                    if (GameManager.instance.PlayerInventory.GetLilyWeapons[GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex].CurrentWeaponType ==
                        PlayerWeaponRawData.WeaponType.WHIP && GameManager.instance.gameplayController.attackInput)
                    {
                        statemachineController.core.attackComboIndex++;
                        statemachineController.normalAttackState.SetComboIndexParameter("whipAttackCombo");
                        statemachineChanger.ChangeState(statemachineController.normalAttackState);
                        GameManager.instance.gameplayController.UseAttackInput();
                    }
                }
            }

            #endregion

            if (!isGrounded)
                statemachineChanger.ChangeState(statemachineController.inAirState);

            else if (isGrounded && GameManager.instance.gameplayController.sprintTapCount == 2
                && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND &&
                GameManager.instance.PlayerStats.GetSetCurrentStamina > 0f)
                statemachineChanger.ChangeState(statemachineController.playerSprintState);

            // TODO: coyote time for JumpState
            else if (isTouchingWall && GameManager.instance.gameplayController.grabWallInput &&
                GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND && isTouchingLedge &&
                GameManager.instance.PlayerStats.GetSetCurrentStamina >=
                movementData.wallGrabHoldStamina)
                statemachineChanger.ChangeState(statemachineController.wallGrabState);

            else if (statemachineController.playerDashState.CheckIfCanDash() &&
                statemachineController.core.groundPlayerController.canWalkOnSlope &&
                GameManager.instance.gameplayController.dashInput &&
                !GameManager.instance.gameplayController.switchPlayerLeftInput &&
                (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.HIGHLAND && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.LOOKINGDOWN && GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
                PlayerStats.AnimatorStateInfo.LOOKINGUP))
                statemachineChanger.ChangeState(statemachineController.playerDashState);
        }
    }
}
