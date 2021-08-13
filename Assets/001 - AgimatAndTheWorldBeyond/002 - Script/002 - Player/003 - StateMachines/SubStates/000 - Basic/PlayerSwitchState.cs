using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchState : PlayerAbilityState
{
    private bool isCurrentlySwitching;
    private bool canSwitch;
    private bool holdPosition;
    private float lastSwitchTime;
    private Transform switchEffect;

    public PlayerSwitchState(PlayerStateMachinesController movementController,
        PlayerStateMachineChanger stateMachine, PlayerRawData movementData, string animBoolName) :
        base(movementController, stateMachine, movementData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        FinishSwitching();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        PlayerSwitch();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.PlayerStats.GetSetAnimatorStateInfo = PlayerStats.AnimatorStateInfo.SWITCHING;

        StartSwitchingState();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        statemachineController.core.SetVelocityZero();

        HoldPoisitonAfterSwitch();
        CancelSwitching();
    }

    #region SWITCHING FUNCTIONS

    private void StartSwitchingState()
    {
        isCurrentlySwitching = true;
        isAbilityDone = false;

        PlaceSwitchEffect();
    }

    private void CancelSwitching()
    {
        if (!isExitingState)
        {
            if (Time.time < (startTime + movementData.switchTime) &&
                (!GameManager.instance.gameInputController.switchPlayerLeftInput ||
                !GameManager.instance.gameInputController.switchPlayerRightInput) &&
                isCurrentlySwitching)
            {
                StopSwitchEffect();
                statemachineChanger.ChangeState(statemachineController.idleState);
                isAbilityDone = true;
                isCurrentlySwitching = false;
            }
        }
    }

    private void PlaceSwitchEffect()
    {
        GameManager.instance.switchEffectPooler.GetFromPool();
        switchEffect = GameManager.instance.switchEffectPooler.currentObjSelectedOnPool.transform;

        if (statemachineController.core.GetFacingDirection == 1)
            GameManager.instance.switchEffectPooler.currentObjSelectedOnPool.transform.position =
                new Vector2(statemachineController.transform.position.x,
                statemachineController.transform.position.y);
        else
            GameManager.instance.switchEffectPooler.currentObjSelectedOnPool.transform.position =
                new Vector2(statemachineController.transform.position.x,
                statemachineController.transform.position.y);
    }

    private void StopSwitchEffect()
    {
        switchEffect = null;

        GameManager.instance.switchEffectPooler.currentObjSelectedOnPool.
            GetComponent<SwitchPlayerParticleController>().isCanceled = true;

        GameManager.instance.CoroutineRunner(GameManager.instance.switchEffectPooler
            .currentObjSelectedOnPool.GetComponent<SwitchPlayerParticleController>().StopParticles());
    }

    private void PlayerSwitch()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            GameManager.instance.PlayerStats.GetSetPlayerCharacter = PlayerStats.PlayerCharacter.LILY;

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            GameManager.instance.PlayerStats.GetSetPlayerCharacter = PlayerStats.PlayerCharacter.LUKAS;

        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("doneSwitching", true);

        holdPosition = true;
        GameManager.instance.gameInputController.ResetSwitchWeaponInput();
    }

    private void FinishSwitching()
    {
        holdPosition = false;
        isAbilityDone = true;
        isCurrentlySwitching = false;
        lastSwitchTime = Time.time;
        GameManager.instance.PlayerStats.GetSetPlayerAnimator.SetBool("doneSwitching", false);
    }

    private void HoldPoisitonAfterSwitch()
    {
        if (!holdPosition)
            return;

        //  FOR HOLDING POSITION
        statemachineController.transform.position = new Vector3(switchEffect.position.x,
            switchEffect.position.y + 1f, 0f);
    }

    public bool CheckIfCanSwitch()
    {
        if (!canSwitch && Time.time >= lastSwitchTime + movementData.switchCooldown)
            canSwitch = true;
        else
            canSwitch = false;

        return canSwitch;
    }

    public void ResetSwitch() => canSwitch = true;

    #endregion
}
