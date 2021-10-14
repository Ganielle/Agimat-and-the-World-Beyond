using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float inputHoldTime = 0.2f;
    [SerializeField] private float sprintDoubleTapTime = 0.15f;
    [SerializeField] private Camera mainCamera;

    [Header("Debugger")]
    //  MOVEMENT
    [ReadOnly] public Vector2 rawMovementVector;

    private event EventHandler movementNormalizeVectorXChange;
    public event EventHandler onMovementNormalizeVectorXChange
    {
        add
        {
            if (movementNormalizeVectorXChange == null || !movementNormalizeVectorXChange.GetInvocationList().Contains(value))
                movementNormalizeVectorXChange += value;
        }
        remove { movementNormalizeVectorXChange -= value; }
    }
    [ReadOnly] [SerializeField] int movementNormalizeX;
    [ReadOnly] public int movementNormalizeY;
    public int GetSetMovementNormalizeX
    {
        get { return movementNormalizeX; }
        set
        {
            movementNormalizeX = value;
            movementNormalizeVectorXChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  SPRINT
    [ReadOnly] [SerializeField] float sprintTapStartTime;
    [ReadOnly] public int sprintTapCount;
    [ReadOnly] public bool canDoubleTapSprint;

    //  JUMP
    [ReadOnly] public bool jumpInput;
    [ReadOnly] public bool jumpInputStop;
    [ReadOnly] [SerializeField] float jumpInputStartTime;

    //  WALL
    [ReadOnly] public bool grabWallInput;

    //  MONKEY BAR
    [ReadOnly] public bool grabMonkeyBarInput;

    //  ROPE
    [ReadOnly] public Vector2 rawRopeMovementX;
    [ReadOnly] public int ropeNormalizeMovementX;
    [ReadOnly] public bool ropeInput;

    //  SWITCH
    [ReadOnly] public bool switchPlayerLeftInput;
    [ReadOnly] public bool switchPlayerRightInput;

    //  DASH
    [ReadOnly] public Vector2 rawDashDirectionInput;
    [ReadOnly] public Vector2Int dashDirectionInput;
    [ReadOnly] public bool dashInput;
    [ReadOnly] public bool dashInputStop;
    [ReadOnly] [SerializeField] float dashInputStartTime;

    //  DODGE
    [ReadOnly] public bool dodgeInput;

    //  SWITCH WEAPON
    private event EventHandler switchWeaponInputChange;
    public event EventHandler onSwitchWeaponInputChange
    {
        add
        {
            if (switchWeaponInputChange == null || !switchWeaponInputChange.GetInvocationList().Contains(value))
                switchWeaponInputChange += value;
        }
        remove { switchWeaponInputChange -= value; }
    }
    [ReadOnly] [SerializeField] int switchWeaponInput;
    [ReadOnly] [SerializeField] float openSwitchWeaponTime;
    [ReadOnly] public bool canSwitchWeapon;
    public int GetWeaponSwitchInput
    {
        get { return switchWeaponInput; }
        private set
        {
            switchWeaponInput = value;
            switchWeaponInputChange?.Invoke(this, EventArgs.Empty);
        }
    }

    //  ATTACK
    [ReadOnly] public bool attackInput;

    //  Mana Charge
    [ReadOnly] public bool isSkillCurrentlyCharging;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
        SprintTimerDoubleTap();
    }

    #region MOVEMENT

    public void OnMovement(InputAction.CallbackContext context)
    {
        rawMovementVector = context.ReadValue<Vector2>();

        GetSetMovementNormalizeX = (int)(rawMovementVector.x * Vector2.right).normalized.x;
        movementNormalizeY = (int)(rawMovementVector.y * Vector2.up).normalized.y;

        if (context.started)
        {
            //  For sprinting
            if (rawMovementVector.x != 0 && sprintTapCount < 3)
            {
                if (sprintTapCount == 0)
                    sprintTapStartTime = Time.time;

                canDoubleTapSprint = true;
                sprintTapCount++;
            }
        }

        else if (context.canceled)
        {
            //  For sprinting
            if (sprintTapCount >= 2)
            {
                canDoubleTapSprint = false;
                sprintTapCount = 0;
            }
        }
    }

    private void SprintTimerDoubleTap()
    {
        if (canDoubleTapSprint)
        {
            if (Time.time > sprintTapStartTime + sprintDoubleTapTime &&
                sprintTapCount < 2)
            {
                canDoubleTapSprint = false;
                sprintTapCount = 0;
            }
        }
    }


    #endregion

    #region GRAB

    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            grabWallInput = true;
            grabMonkeyBarInput = true;
            ropeInput = true;
        }

        else if (context.canceled)
        {
            grabWallInput = false;
            grabMonkeyBarInput = false;
            ropeInput = false;
        }
    }

    public void UseGrabMonkeyBarInput() => grabMonkeyBarInput = false;

    #endregion

    #region ROPE

    public void OnRopeTurn(InputAction.CallbackContext context)
    {
        rawRopeMovementX = context.ReadValue<Vector2>();

        ropeNormalizeMovementX = (int)(rawRopeMovementX.x * Vector2.right).normalized.x;
    }

    #endregion

    #region JUMPING

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            jumpInput = true;
            jumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        else if (context.canceled)
            jumpInputStop = true;
    }

    public void UseJumpInput() => jumpInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            jumpInput = false;
            jumpInputStartTime = 0f;
        }
    }

    #endregion

    #region DASH

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            dashInput = true;
            dashInputStop = false;
            dashInputStartTime = Time.time;
        }

        else if (context.canceled)
        {
            dashInputStop = true;
        }
    }

    public void UseDashInput() => dashInput = false;

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            dashInput = false;
        }
    }

    public void OnDashDirection(InputAction.CallbackContext context)
    {
        rawDashDirectionInput = context.ReadValue<Vector2>();


        if (playerInput.currentControlScheme == "Keyboard")
        {

            rawDashDirectionInput = GameManager.instance.mouseCamera.ScreenToWorldPoint(
                new Vector3(rawDashDirectionInput.x, rawDashDirectionInput.y, 0f))
                - GameManager.instance.PlayerStats.GetSetPlayerCharacterObj.transform.position;
        }

        dashDirectionInput = Vector2Int.RoundToInt(rawDashDirectionInput.normalized);
    }

    #endregion

    #region SWITCH

    public void OnSwitchPlayerLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //  For Keyboard
            if (playerInput.currentControlScheme == "Keyboard")
            {
                switchPlayerLeftInput = true;
                switchPlayerRightInput = true;
            }
            //  For Gamepad
            else
                switchPlayerLeftInput = true;
        }

        if (context.canceled)
        {
            //  For Keyboard
            if (playerInput.currentControlScheme == "Keyboard")
            {
                switchPlayerLeftInput = false;
                switchPlayerRightInput = false;
            }
            //  For Gamepad
            else
                switchPlayerLeftInput = false;
        }
    }

    public void OnSwitchPlayerRight(InputAction.CallbackContext context)
    {
        if (context.performed)
            switchPlayerRightInput = true;

        if (context.canceled)
            switchPlayerRightInput = false;
    }

    #endregion

    #region DODGE

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
            dodgeInput = true;

        else if (context.canceled)
            dodgeInput = false;
    }

    #endregion

    #region SWITCH WEAPONS

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GetWeaponSwitchInput < 2)
            {
                GetWeaponSwitchInput++;
            }

            if (GetWeaponSwitchInput >= 2)
            {
                canSwitchWeapon = true;
                GetWeaponSwitchInput = 2;
            }
        }
    }

    public void ResetSwitchWeaponInput() => GetWeaponSwitchInput = 0;

    public void UseCanSwitchWeaponInput() => canSwitchWeapon = false;

    #endregion

    #region ATTACK

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attackInput = true;
        }

        if (context.canceled)
        {
            attackInput = false;
        }
    }

    public void UseAttackInput() => attackInput = false;

    #endregion
}
