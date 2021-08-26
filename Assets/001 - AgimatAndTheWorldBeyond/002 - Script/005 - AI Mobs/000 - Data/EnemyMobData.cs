using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMovementData", menuName = "Agimat and the World Beyond/Enemy Data/Movement Data")]
public class EnemyMobData : ScriptableObject
{
    [Header("PATROL STATE")]
    public float movementSpeed = 10f;
    public float sprintSpeed = 15f;
    public float maxVelocityXOnGround = 7.5f;

    [Header("GROUND CHECKER")]
    public float groundCheckRadius = 3f;
    public float raycastGroundDistance = 3f;
    public float floatShadowHeightOffset = 5f;
    public Vector2 feetOffset;
    public float floorCheckOffsetHeight = 0.01f;
    public float floorCheckOffsetWidth = 0.5f;
    public float maxFloorCheckDist = 1.0f;

    [Header("WALL CHECKER")]
    public Vector2 wallCheckRadius = new Vector2(0.1f, 0.1f);
    public float wallClimbCheckRadius = 1f;
    public float wallSlideVelocity = 3f;
    public float wallClimbVelocity = 3f;
}
