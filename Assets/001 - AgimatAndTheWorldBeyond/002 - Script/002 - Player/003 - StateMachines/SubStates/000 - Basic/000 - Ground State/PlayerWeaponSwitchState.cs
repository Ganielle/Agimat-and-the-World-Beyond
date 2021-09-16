using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSwitchState : PlayerGroundState
{
    private bool doneAnimation;
    private string animationBool;
    private bool canSwitch;

    public PlayerWeaponSwitchState(PlayerStateMachinesController movementController, 
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName, bool isBoolAnim) :
        base(movementController, stateMachine, movementData, animBoolName, isBoolAnim)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        doneAnimation = true;
        canSwitch = true;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.SWITCHWEAPON;

        animationBool = animBoolName;
        doneAnimation = false;
    }

    public override void Exit()
    {
        base.Exit();

        if (!doneAnimation)
        {
            canSwitch = true;
            doneAnimation = true;
            GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool(animationBool, false);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityZero();

        if (!isExitingState)
        {
            if (!doneAnimation)
            {
                /*
                 * animations that can do animation cancel
                 */

                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX != 0f)
                    statemachineChanger.ChangeState(statemachineController.moveState);

                else if (GameManager.instance.gameplayController.jumpInput)
                {
                    statemachineChanger.ChangeState(statemachineController.jumpState);
                    GameManager.instance.gameplayController.UseJumpInput();
                }

                else if (GameManager.instance.gameplayController.dodgeInput &&
                    statemachineController.playerDodgeState.CheckIfCanDodge() &&
                    GameManager.instance.PlayerStats.GetSetCurrentStamina >=
                    statemachineController.core.staminaController.dodgeStaminaPercentage)
                    statemachineChanger.ChangeState(statemachineController.playerDodgeState);

                else if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0 &&
                    GameManager.instance.gameplayController.switchPlayerLeftInput &&
                    GameManager.instance.gameplayController.switchPlayerRightInput &&
                    statemachineController.switchPlayerState.CheckIfCanSwitch())
                    statemachineChanger.ChangeState(statemachineController.switchPlayerState);
            }
            else
            {
                /*
                 * animation that can't do animation cancel 
                 */

                if (GameManager.instance.gameplayController.GetSetMovementNormalizeX == 0)
                    statemachineChanger.ChangeState(statemachineController.idleState);
            }
        }
    }

    public bool CheckIfCanWeaponSwitch()
    {
        return canSwitch && Time.time >= lastChangeWeaponTime +
            movementData.weaponSwitchTime;
    }

    public void ResetWeaponSwitch() => canSwitch = true;
}
