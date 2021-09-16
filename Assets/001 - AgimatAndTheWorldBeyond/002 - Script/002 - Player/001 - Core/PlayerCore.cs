using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [Header("SETTINGS")]
    public PlayerRawData playerRawData;
    public Rigidbody2D playerRB;
    public Collider2D playerCollider;
    public Collider2D feetOffsetCollider;
    public Transform parentPlayer;
    public Transform childPlayer;
    public Transform envCheckerXRot;

    [Header("SCRIPT REFERENCES")]
    public WeaponChangerController weaponChangerController;
    public StaminaController staminaController;
    public RopePlayerController ropePlayerController;
    public GroundPlayerController groundPlayerController;
    public PlayerStateMachinesController statemachineController;
    public AttackController attackController;

    [Space]
    public GameObject shadowPlayer;

    [Space]
    public Vector2 colliderSize;

    [Header("BATTLE")]
    [SerializeField] private float battleStateCooldownToAdventure;

    [Header("MONKEY BAR")]
    public LayerMask whatIsMonkeyBar;
    public Transform monkeyBarCheck;
    public Transform monkeyBarFrontCheck;

    [Header("DASH")]
    public Transform dashDirectionIndicator;

    [Header("STAMINA")]
    [SerializeField] private Transform staminaPanel;

    [Header("DEBUGGER")]
    [ReadOnly] public Vector2 GetCurrentVelocity;
    [ReadOnly] public Vector2 GetWorkspace;
    [ReadOnly] public Vector3 GetRotationWorkspace;
    [ReadOnly] public Vector2 lastAfterImagePosition;
    [ReadOnly] public int GetFacingDirection;
    [ReadOnly] public float distanceToGroundRaycast;
    [ReadOnly] public float heightError;
    [ReadOnly] public float lastBattleState;

    //  PRIVATE VARIABLES
    private RaycastHit2D hitInfo;

    private void Awake()
    {
        FlipCheckerOnStart();

        //Time.timeScale = 0.25f;
    }

    private void Update()
    {
        BattleToAdventure();
    }

    public void ChangePlayerColliderOffsetY(float y)
    {
        playerCollider.offset = new Vector2(playerCollider.offset.x, y);
    }

    #region BATTLE STATE CHANGER

    /* 
     *  TODO:
     *  REFACTOR THE BATTLE STATE CHANGER
     */

    public void ChangeBattleState()
    {
        GameManager.instance.PlayerStats.GetSetBattleState = PlayerStats.PlayerBattleState.BATTLE;
        lastBattleState = Time.time;
    }

    public void BattleToAdventure()
    {
        if (Time.time >= lastBattleState + battleStateCooldownToAdventure && 
            GameManager.instance.PlayerStats.GetSetBattleState != PlayerStats.PlayerBattleState.ADVENTURING)
        {
            GameManager.instance.PlayerStats.GetSetBattleState = PlayerStats.PlayerBattleState.ADVENTURING;
        }
    }

    #endregion

    #region PHYSICS

    public void CurrentVelocitySetter() => GetCurrentVelocity = playerRB.velocity;

    public void SetVelocityZero()
    {
        playerRB.velocity = Vector2.zero;
        GetCurrentVelocity = Vector2.zero;
    }

    public void SetVelocityDash(float velocity, Vector2 direction)
    {
        GetWorkspace = direction * velocity;
        playerRB.velocity = GetWorkspace;
        GetCurrentVelocity = GetWorkspace;
    }

    public void SetVelocityWallJump(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        GetWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        playerRB.velocity = GetWorkspace;
        GetCurrentVelocity = GetWorkspace;
    }

    public void SetVelocityX(float velocityX, float velocityY)
    {
        GetWorkspace.Set(velocityX, velocityY);
        playerRB.velocity = GetWorkspace;
        GetCurrentVelocity = GetWorkspace;
    }

    public void SetVelocityY(float velocity)
    {
        GetWorkspace.Set(GetCurrentVelocity.x, velocity);
        playerRB.velocity = GetWorkspace;
        GetCurrentVelocity = GetWorkspace;
    }

    public void SetVelocityRopeY(float x, float y)
    {
        GetWorkspace.Set(x, y);
        playerRB.velocity = GetWorkspace;
        GetCurrentVelocity = GetWorkspace;
    }

    #endregion

    #region ENVIRONMENT

    public bool CheckIfTouchingMonkeyBar
    {
        get => Physics2D.OverlapCircle(monkeyBarCheck.position, playerRawData.monkeyBarRarCheckRadius,
            whatIsMonkeyBar);
    }

    public bool CheckIfTouchingMonkeyBarFront
    {
        get => Physics2D.OverlapCircle(monkeyBarFrontCheck.position, playerRawData.monkeyBarRarCheckRadius,
            whatIsMonkeyBar);
    }

    public Transform MonkeyBarPosition()
    {
        Collider2D collider = Physics2D.OverlapCircle(monkeyBarCheck.position, playerRawData.monkeyBarRarCheckRadius,
            whatIsMonkeyBar);

        if (collider == null)
            return null;

        return collider.transform;
    }

    public void ShadowCaster(bool isEnabled)
    {
        hitInfo = Physics2D.Raycast(transform.position, Vector2.down, playerRawData.
            raycastGroundDistance, groundPlayerController.whatIsGround);

        if (isEnabled)
        {
            if (hitInfo.collider != null)
            {
                shadowPlayer.transform.position = new Vector2(hitInfo.point.x, hitInfo.point.y +
                    playerRawData.floatShadowHeightOffset);
                shadowPlayer.SetActive(true);
            }
            else
            {
                shadowPlayer.SetActive(false);
            }
        }
        else
        {
            shadowPlayer.SetActive(false);
        }
    }

    #endregion

    #region PLAYER SPRITE

    public void CheckIfShouldFlip(int direction)
    {
        if (direction != 0 && direction != GetFacingDirection &&
            GameManager.instance.PlayerStats.GetSetAnimatorStateInfo !=
            PlayerStats.AnimatorStateInfo.HIGHLAND)
            PlayerFlip();
    }

    private void PlayerFlip()
    {
        GetFacingDirection *= -1;
        childPlayer.Rotate(0f, 180f, 0f);
        envCheckerXRot.Rotate(0f, 180f, 0f);
        //staminaPanel.Rotate(0f, 180f, 0f);
    }


    //  FACING RIGHT = 1
    //  FACING LEFT = -1
    public void FlipCheckerOnStart()
    {
        if (childPlayer.rotation.x == 0)
            GetFacingDirection = 1;
        else
            GetFacingDirection = -1;
    }

    //  AURA EFFECT
    public void CheckIfShouldPlaceAfterImage()
    {
        if (Vector2.Distance(statemachineController.transform.position, lastAfterImagePosition) >= playerRawData.distanceBetweenAfterImages)
            PlaceAfterImage();
    }

    public void PlaceAfterImage()
    {
        GameManager.instance.afterImagePooler.GetFromPool();
        lastAfterImagePosition = statemachineController.transform.position;
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        //  Monkey bar
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(monkeyBarCheck.position, playerRawData.monkeyBarRarCheckRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(monkeyBarFrontCheck.position, playerRawData.monkeyBarRarCheckRadius);
    }
}
