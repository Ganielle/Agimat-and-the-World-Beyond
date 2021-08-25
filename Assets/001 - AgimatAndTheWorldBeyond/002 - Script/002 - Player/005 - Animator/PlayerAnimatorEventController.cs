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

    public void LastAttackCombo() => playerCore.onLastAttackCombo = true;

    public void CanAttackCancelAnimation() => playerCore.canCancelAnimation = true;

    public void CancelAttackCancelAnimation() => playerCore.canCancelAnimation = false;

    public void AxeAttackMovementVelocity() => playerCore.SetVelocityX(playerCore.playerRawData.axeAttackMovementSpeed *
        playerCore.GetFacingDirection, playerCore.GetCurrentVelocity.y);

    public void ResetVelocity() => playerCore.SetVelocityZero();
}
