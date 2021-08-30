using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [Header("SETTINGS")]
    public EnemyMobData enemyMobData;
    public Rigidbody2D enemyRB;
    public Collider2D enemyBodyCollider;
    public Collider2D enemyFeetOffsetCollider;
    public Transform mainEnemyMob;
    public Transform enemyEnvCheckerXRot;

    [Header("DEBUGGER")]
    [ReadOnly] public Vector2 GetCurrentVelocity;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CurrentVelocitySetter() => GetCurrentVelocity = enemyRB.velocity;
}
