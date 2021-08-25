using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCore core;
    [SerializeField] private PlayerRawData playerRawData;

    [Space]
    public Rigidbody2D ropeRB;
    public HingeJoint2D playerHingeJoint;
    public HingeJoint2D ropeHingeJoint;

    [Header("Rope")]
    public LayerMask whatIsRope;
    public Transform ropeCheck;
    public Transform ropeAboveCheck;
    public Transform ropeBelowCheck;

    public void RemoveRopePlayerHingeJointConnector()
    {
        ropeHingeJoint.connectedBody = null;
        ropeHingeJoint.enabled = false;
        playerHingeJoint.enabled = false;
    }

    public void RopePlayerHingeJointConnector()
    {
        Collider2D colliderOne = Physics2D.OverlapCircle(ropeCheck.position, playerRawData.ropeCheckRadius,
            whatIsRope);

        if (colliderOne != null)
        {
            ropeHingeJoint.connectedBody = colliderOne.attachedRigidbody;
            ropeHingeJoint.enabled = true;
            playerHingeJoint.enabled = true;
        }
    }

    #region ENVIRONMENT

    public bool CheckIfTouchingRope
    {
        get => Physics2D.OverlapCircle(ropeCheck.position, playerRawData.ropeCheckRadius,
            whatIsRope);
    }

    public bool CheckIfTouchingRopeAbove
    {
        get => Physics2D.OverlapCircle(ropeAboveCheck.position, playerRawData.ropeCheckRadius,
            whatIsRope);
    }

    public Transform RopePosition()
    {
        Collider2D collider = Physics2D.OverlapCircle(ropeCheck.position, playerRawData.ropeCheckRadius,
               whatIsRope);

        if (collider == null)
            return null;

        return collider.transform;
    }

    public bool RopeAboveChecker
    {
        get => Physics2D.OverlapCircle(ropeAboveCheck.position, playerRawData.ropeCheckRadius,
               whatIsRope);
    }

    public bool RopeBelowChecker
    {
        get => Physics2D.OverlapCircle(ropeBelowCheck.position, playerRawData.ropeCheckRadius,
               whatIsRope);
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        //  Rope
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(ropeCheck.position, core.playerRawData.ropeCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(ropeAboveCheck.position, core.playerRawData.ropeCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(ropeBelowCheck.position, core.playerRawData.ropeCheckRadius);
    }
}
