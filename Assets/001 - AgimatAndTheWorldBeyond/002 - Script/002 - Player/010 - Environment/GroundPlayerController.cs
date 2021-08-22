using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlayerController : MonoBehaviour
{
    //  TODO: CHECK SLOPE CONTROLLER

    [SerializeField] private Core core;
    [SerializeField] private PlayerRawData playerRawData;
    [SerializeField] private Rigidbody2D playerRB;

    [Header("Ground")]
    public float maxSlopeAngle;
    public float minimumSlopeAngle;
    public LayerMask whatIsGround;
    public Transform playerPlatformHeightCheck;
    public Transform groundCheck;
    public Transform groundFrontFootCheck;
    public Transform groundBackFootCheck;
    public Transform slopeCheck;

    [Header("Wall")]
    public Transform wallCheck;
    public Transform wallClimbCheck;
    public Transform ledgeCheck;
    
    [Header("ReadOnly")]
    [ReadOnly] public Vector2 slopeForward;
    [ReadOnly] public float groundAngle;
    //[ReadOnly] public Vector2 slopeNormalPerp;
    //[ReadOnly] public bool isOnSlope;
    //[ReadOnly] public bool canWalkOnSlope;
    //[ReadOnly] public float slopeDownAngle;
    //[ReadOnly] public float slopeDownAngleOld;
    //[ReadOnly] public float slopeSlideAngle;

    //  PRIVATE VARIABLES
    //private RaycastHit2D hitInfo;
    //private RaycastHit2D slopeHit;
    //private RaycastHit2D slopeHitFront;
    //private RaycastHit2D slopeHitBack;

    #region PHYSICS

    public void PhysicsMaterialChanger(PhysicsMaterial2D mat) => playerRB.sharedMaterial = mat;

    #endregion

    #region ENVIRONMENT

    public bool PlayerToPlatformHeightCheck
    {
        get => Physics2D.Raycast(playerPlatformHeightCheck.position, Vector2.right *
            core.GetFacingDirection, playerRawData.wallClimbCheckRadius, whatIsGround);
    }

    public bool CheckIfTouchingLedge
    {
        get => Physics2D.Raycast(ledgeCheck.position, Vector2.right *
            core.GetFacingDirection, playerRawData.wallClimbCheckRadius, whatIsGround);
    }

    public bool CheckIfTouchClimbWall
    {
        get => Physics2D.Raycast(wallClimbCheck.position, Vector2.right *
            core.GetFacingDirection, playerRawData.wallClimbCheckRadius, whatIsGround);
    }

    public bool CheckIfTouchWall
    {
        get => Physics2D.OverlapBox(wallCheck.position, playerRawData.wallCheckRadius,
            0f, whatIsGround);
    }

    public bool CheckIfTouchGround
    {
        get => Physics2D.OverlapCircle(groundCheck.position, playerRawData.groundCheckRadius,
            whatIsGround);
    }

    public bool CheckIfFrontFootTouchGround
    {
        get => Physics2D.OverlapCircle(groundFrontFootCheck.position, playerRawData.groundCheckRadius,
            whatIsGround);
    }

    public bool CheckIfBackFootTouchGround
    {
        get => Physics2D.OverlapCircle(groundBackFootCheck.position, playerRawData.groundCheckRadius,
            whatIsGround);
    }

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallClimbCheck.position, Vector2.right * core.GetFacingDirection,
            playerRawData.wallClimbCheckRadius, whatIsGround);
        float xDist = xHit.distance;
        core.GetWorkspace.Set(xDist * core.GetFacingDirection, 0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(core.GetWorkspace),
            Vector2.down, ledgeCheck.position.y - wallClimbCheck.position.y, whatIsGround);
        float yDist = yHit.distance;

        core.GetWorkspace.Set(wallClimbCheck.position.x + (xDist * core.GetFacingDirection),
            ledgeCheck.position.y - yDist);
        return core.GetWorkspace;
    }

    public void CalculateSlopeForward()
    {
        if (CheckIfTouchGround)
        {
            slopeForward = transform.forward;
            return;
        }

        slopeForward = Vector3.Cross(Physics2D.Raycast(transform.position, Vector2.down, playerRawData.slopeCheckDistance).normal,
            -transform.right);
    }

    public void CalculateGroundAngle()
    {
        if (CheckIfTouchGround)
        {
            groundAngle = 90f;
            return;
        }

        groundAngle = Vector3.Angle(Physics2D.Raycast(transform.position, Vector2.down, playerRawData.slopeCheckDistance).normal,
            transform.forward);
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        //  Ground
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheck.position, playerRawData.groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundFrontFootCheck.position, playerRawData.groundCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundBackFootCheck.position, playerRawData.groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(playerPlatformHeightCheck.position, Vector2.right *
            playerRawData.wallClimbCheckRadius * core.GetFacingDirection);

        //  Wall Climbing
        Gizmos.color = Color.red;
        Gizmos.DrawRay(wallClimbCheck.position, Vector2.right *
            playerRawData.wallClimbCheckRadius * core.GetFacingDirection);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(ledgeCheck.position, Vector2.right * playerRawData.wallClimbCheckRadius *
            core.GetFacingDirection);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(wallCheck.position, playerRawData.wallCheckRadius);
    }
}
