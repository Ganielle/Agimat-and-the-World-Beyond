using Cinemachine;
using MyBox;
using System;
using System.Collections;
using UnityEngine;

public class DynamicCameraPlayerGameplay : MonoBehaviour
{
    [SerializeField] private CinemachineCameraOffset cinemachineOffsetter;
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private PlayerCore playerCore;


    [Header("Idle")]
    [SerializeField] private float idleXSpeed;
    [SerializeField] private float idleYZSpeed;
    [SerializeField] private float idleXOffset = 2.5f;
    [SerializeField] private float idleYOffset = 0.5f;
    [SerializeField] private float idleZOffset = -3f;

    [Header("LookingUp")]
    [SerializeField] private float lookingUpXSpeed;
    [SerializeField] private float lookingUpYZSpeed;
    [SerializeField] private float lookingUpXOffset;
    [SerializeField] private float lookingUpYOffset;
    [SerializeField] private float lookingUpZOffset;

    [Header("LookingDown")]
    [SerializeField] private float lookingDownXSpeed;
    [SerializeField] private float lookingDownYZSpeed;
    [SerializeField] private float lookingDownXOffset;
    [SerializeField] private float lookingDownYOffset;
    [SerializeField] private float lookingDownZOffset;

    [Header("land")]
    [SerializeField] private float landXSpeed;
    [SerializeField] private float landYZSpeed;
    [SerializeField] private float landXOffset;
    [SerializeField] private float landYOffset;
    [SerializeField] private float landZOffset;

    [Header("Run Settings")]
    [SerializeField] private float runXSpeed;
    [SerializeField] private float runYZSpeed;
    [SerializeField] private float runXOffset;
    [SerializeField] private float runYOffset;
    [SerializeField] private float runZOffset;

    [Header("Sprint")]
    [SerializeField] private float sprintXSpeed;
    [SerializeField] private float sprintYZSpeed;
    [SerializeField] private float sprintXOffset;
    [SerializeField] private float sprintYOffset;
    [SerializeField] private float sprintZOffset;

    [Header("Dash Settings")]
    [SerializeField] private float dashChargeXSpeed;
    [SerializeField] private float dashChargeYZSpeed;
    [SerializeField] private float dashChargeXOffset;
    [SerializeField] private float dashChargeYOffset;
    [SerializeField] private float dashChargeZOffset;

    [SerializeField] private float dashBurstXSpeed;
    [SerializeField] private float dashBurstYZSpeed;
    [SerializeField] private float dashBurstXOffset;
    [SerializeField] private float dashBurstYOffset;
    [SerializeField] private float dashBurstZOffset;

    [Header("Jump Settings")]
    [SerializeField] private float jumpXSpeed;
    [SerializeField] private float jumpYZSpeed;
    [SerializeField] private float jumpXOffset;
    [SerializeField] private float jumpYOffset;
    [SerializeField] private float jumpZOffset;

    [Header("InAir Settings")]
    [SerializeField] private float fallingXSpeed;
    [SerializeField] private float fallingYZSpeed;
    [SerializeField] private float fallingXOffset;
    [SerializeField] private float fallingYOffset;
    [SerializeField] private float fallingZOffset;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] bool canResume;
    [ReadOnly] [SerializeField] private float speedYZOffset;
    [ReadOnly] [SerializeField] private float speedXOffset;
    [ReadOnly] [SerializeField] float xTime;
    [ReadOnly] [SerializeField] float yTime;
    [ReadOnly] [SerializeField] float zTime;
    [ReadOnly] [SerializeField] float endXOffset;
    [ReadOnly] [SerializeField] float endYOffset;
    [ReadOnly] [SerializeField] float endZOffset;
    [ReadOnly] [SerializeField] CinemachineTransposer compVirtualCamera;

    private void OnEnable()
    {
        compVirtualCamera = VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        ChangeOffsetsCamera();

        GameManager.instance.PlayerStats.onAnimatorStateInfoChange += MovementDirectionChange;
        GameManager.instance.gameplayController.onMovementNormalizeVectorXChange += MovementDirectionChange;
    }

    private void OnDisable()
    {
        GameManager.instance.PlayerStats.onAnimatorStateInfoChange -= MovementDirectionChange;
        GameManager.instance.gameplayController.onMovementNormalizeVectorXChange -= MovementDirectionChange;
    }

    private void Update()
    {
        ChangeOffsetsCamera();

        //  xAxis
        if (compVirtualCamera.m_FollowOffset.x != endXOffset)
        {
            xTime += Time.deltaTime / speedXOffset;
            DynamicCamera(compVirtualCamera.m_FollowOffset.x, endXOffset, xTime,
                true, false, false);
        }

        //  yAxis
        if (cinemachineOffsetter.m_Offset.y != endYOffset)
        {
            yTime += Time.deltaTime / speedYZOffset;
            DynamicCamera(cinemachineOffsetter.m_Offset.y, endYOffset, yTime,
                false, true, false);
        }

        // zAxis
        if (cinemachineOffsetter.m_Offset.z != endZOffset)
        {
            zTime += Time.deltaTime / speedYZOffset;
            DynamicCamera(cinemachineOffsetter.m_Offset.z, endZOffset, zTime,
                false, false, true);
        }
    }

    private void MovementDirectionChange(object sender, EventArgs e)
    {
        xTime = 0.01f;
        yTime = 0.01f;
        zTime = 0.01f;
    }

    private void ChangeOffsetsCamera()
    {
        switch (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo)
        {
            case PlayerStats.AnimatorStateInfo.IDLE:
                endYOffset = idleYOffset;
                endZOffset = idleZOffset;

                if (GameManager.instance.PlayerStats.GetSetLastAnimatorStateInfo !=
                    PlayerStats.AnimatorStateInfo.CHANGEIDLEDIRECTION)
                    endXOffset = idleXOffset * playerCore.GetFacingDirection;

                speedXOffset = idleXSpeed;
                speedYZOffset = idleYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.LOOKINGUP:
                endXOffset = lookingUpXOffset * playerCore.GetFacingDirection;
                endYOffset = lookingUpYOffset;
                endZOffset = lookingUpZOffset;

                speedXOffset = lookingUpXSpeed;
                speedYZOffset = lookingUpYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.LOOKINGDOWN:
                endXOffset = lookingDownXOffset * playerCore.GetFacingDirection;
                endYOffset = lookingDownYOffset;
                endZOffset = lookingDownZOffset;

                speedXOffset = lookingDownXSpeed;
                speedYZOffset = lookingDownYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.JUMPING:
                endXOffset = jumpXOffset * playerCore.GetFacingDirection;
                endYOffset = jumpYOffset;
                endZOffset = jumpZOffset;

                speedXOffset = jumpXSpeed;
                speedYZOffset = jumpYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.RUNNING:
                endXOffset = runXOffset * playerCore.GetFacingDirection;
                endYOffset = runYOffset;
                endZOffset = runZOffset;

                speedXOffset = runXSpeed;
                speedYZOffset = runYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.DASHCHARGE:
                endXOffset = dashChargeXOffset * playerCore.GetFacingDirection;
                endYOffset = dashChargeYOffset;
                endZOffset = dashChargeZOffset;

                speedXOffset = dashChargeXSpeed;
                speedYZOffset = dashChargeYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.DASHBURST:
                endXOffset = dashBurstXOffset * playerCore.GetFacingDirection;
                endYOffset = dashBurstYOffset;
                endZOffset = dashBurstZOffset;

                speedXOffset = dashBurstXSpeed;
                speedYZOffset = dashBurstYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.SPRINT:
                endXOffset = sprintXOffset * playerCore.GetFacingDirection;
                endYOffset = sprintYOffset;
                endZOffset = sprintZOffset;

                speedXOffset = sprintXSpeed;
                speedYZOffset = sprintYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.FALLING:
                endXOffset = fallingXOffset * playerCore.GetFacingDirection;
                endYOffset = fallingYOffset;
                endZOffset = fallingZOffset;

                speedXOffset = fallingXSpeed;
                speedYZOffset = fallingYZSpeed;
                break;
            case PlayerStats.AnimatorStateInfo.HIGHLAND:
                endXOffset = landXOffset * playerCore.GetFacingDirection;
                endYOffset = landYOffset;
                endZOffset = landZOffset;

                speedXOffset = landXSpeed;
                speedYZOffset = landYZSpeed;
                break;
        }
    }

    private void DynamicCamera(float startPos, float endPos, float time,
        bool isXAxis, bool isYAxis, bool isZAxis)
    {
        if (time <= 1.0f)
        {
            if (isXAxis)
                compVirtualCamera.m_FollowOffset.x = Mathf.Lerp(startPos, endPos, time);

            if (isYAxis)
                cinemachineOffsetter.m_Offset.y = Mathf.Lerp(startPos, endPos, time);

            if (isZAxis)
                cinemachineOffsetter.m_Offset.z = Mathf.Lerp(startPos, endPos, time);
        }
    }
}
