using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEventController : MonoBehaviour
{
    [SerializeField] private PlayerCore playerCore;
    [SerializeField] private PlayerStateMachinesController movementController;
    [SerializeField] private Collider2D playerHitbox;
    [SerializeField] private SpriteRenderer sr;

    public void AnimationTrigger() => movementController.statemachineChanger.CurrentState.AnimationTrigger();

    public void AnimationFinishTrigger() => movementController.statemachineChanger.CurrentState.AnimationFinishTrigger();

    public void DisableEnvironmentCollider(bool stats) => playerHitbox.enabled = stats;

    public void DodgeState(Sprite sprite) => sr.sprite = sprite;

    public void SwitchCharacter()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            GameManager.instance.PlayerStats.GetSetPlayerCharacter = PlayerStats.PlayerCharacter.LILY;

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            GameManager.instance.PlayerStats.GetSetPlayerCharacter = PlayerStats.PlayerCharacter.LUKAS;
    }

    public void LastAttackCombo() => playerCore.attackController.onLastAttackCombo = true;

    public void CanAttackCancelAnimation() => playerCore.attackController.canCancelAnimation = true;

    public void CancelAttackCancelAnimation() => playerCore.attackController.canCancelAnimation = false;

    public void CanChangeDirectionAttack(int value)
    {
        if (value != 0) playerCore.attackController.canChangeDirectionWhileAttacking = true;
        else playerCore.attackController.canChangeDirectionWhileAttacking = false;
    }

    public void GroundAttackMovementVelocity(float value) => playerCore.playerRB.AddForce(Vector2.right * value * playerCore.GetFacingDirection, ForceMode2D.Impulse);

    public void ResetVelocity() => playerCore.SetVelocityZero();

    public void CanDelayNextAttack(int value)
    {
        if (value != 0) playerCore.attackController.canEnterDelayAttack = true;
        else playerCore.attackController.canEnterDelayAttack = false;
    }

    public void EnterDelayAttackTime() => playerCore.attackController.enterDelayAttackTime = Time.time;


}
