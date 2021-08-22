using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Agimat and the World Beyond/Player Data/Movement Data")]
public class PlayerRawData : ScriptableObject
{
    [Header("Physics Material")]
    public PhysicsMaterial2D lessFriction;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;
    public float colliderNormalOffsetY;
    public float colliderRopeOffsetY;

    [Header("Taunt Idle")]
    public float idleToTauntIdleTime = 10f;
    public float tauntIdleToIdleTime = 10f;

    [Header("Move State")]
    public float movementSpeed = 10f;
    public float sprintSpeed = 15f;
    public float maxVelocityXOnGround = 7.5f;
    public float pushForcePlayerWhenFootNotTouchingGround = 10f;

    [Header("Jump")]
    public float jumpStrength = 15f;
    public float movementSpeedOnAir = 5;
    public float maxVelocityXOnAir = 10f;
    public float maxJumpHeight = 25f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Ground Checker")]
    public float groundCheckRadius = 3f;
    public float raycastGroundDistance = 3f;
    public float floatShadowHeightOffset = 5f;
    public Vector2 feetOffset;
    public float floorCheckOffsetHeight = 0.01f;
    public float floorCheckOffsetWidth = 0.5f;
    public float maxFloorCheckDist = 1.0f;

    [Header("Slope")]
    public float slopeCheckDistance;
    public float maxSlopeAngle;
    public float onSlopeAngle;

    [Header("Wall Checker")]
    public Vector2 wallCheckRadius = new Vector2(0.1f, 0.1f);
    public float wallClimbCheckRadius = 1f;
    public float wallSlideVelocity = 3f;
    public float wallClimbVelocity = 3f;

    [Header("Wall Jump")]
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public float delayToUseDash = 0.25f;
    public float delayToCheckForGround = 0.3f;
    public float delayToCheckForLedge = 0.25f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Ledge climb state")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("MonkeyBar state")]
    public float monkeyBarRarCheckRadius = 1f;
    public float monkeyBarVelocity = 3f;
    public Vector2 mbStartOffset;
    public float monkeyBarJumpVelocity = 20f;
    public float delayToCheckForMBAfterJump = 0.25f;

    [Header("Rope State")]
    public float ropeCheckRadius = 0.1f;
    public float ropeClimbVelocity = 0.05f;
    public float ropeClimDownVelocity = 0.5f;
    public float ropeSwingVelocity = 2f;
    public float anchorX = 0.08f;
    public float rightAngle = -10f;
    public float leftAngle = -30;
    public float ropeJumpVelocity = 20f;
    public float ropeJumpTime = 0.4f;
    public Vector2 ropeJumpAngle = new Vector2(1, 2);

    [Header("Switching")]
    public float switchTime = 0.85f;
    public float switchCooldown = 2f;
    public float weaponSwitchTime = 0.15f;

    [Header("Dash")]
    public float dashCooldown = 1f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distanceBetweenAfterImages = 0.5f;

    [Header("Dodge")]
    public float dodgeVelocity = 5f;
    public float dodgeCooldown = 0.5f;

    [Header("Health")]
    public float healthRecoveryDelay = 1f;
    public float startingHealthRecoverPerSecond = 2.5f;

    [Header("Mana")]
    public float manaRecoveryDelay = 1f;
    public float startingManaRecoveryPerSecond = 1.5f;
    public float firstSkillUsePercentage = 33.33f;
    public float secondSkillUsePercentage = 66.66f;
    public float thirdSkillUsePercentage = 100f;

    [Header("Stamina")]
    public float staminaRecoveryDelay = 0.5f;
    public float staminaRecoverWhenActive = 15f;
    public float staminaRecoverWhenNotActive = 2f;
    public float sprintStamina = 7f;
    public float ledgeStamina = 5f;
    public float wallGrabHoldStamina = 10f;
    public float wallClimbingStamina = 15f;
    public float dodgePercentage = 0.25f;
    public float dashPercentage = 0.35f;
    public float wallJumpPercentage = 0.20f;
}
