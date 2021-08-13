using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassInteractiveController : MonoBehaviour
{
    [SerializeField] private Renderer grassRenderer;
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private float interpolationTime;
    [SerializeField] private float maxVelocity;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] float velocity;
    [ReadOnly] [SerializeField] float oldVelocity;
    [ReadOnly] [SerializeField] float currentVelocity;
    [ReadOnly] [SerializeField] float timeElapsed;

    int leanID, tweenID;

    private void Awake()
    {
        oldVelocity = grassRenderer.material.GetFloat("_TouchDirection");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LeanTween.cancel(leanID);
            LeanTween.cancel(tweenID);

            velocity = collision.attachedRigidbody.velocity.x / 10;
            currentVelocity = grassRenderer.material.GetFloat("_TouchDirection");

            if (velocity > 0)
                velocity = 0.15f;
            else if (velocity < 0)
                velocity = maxVelocity;

            leanID = LeanTween.value(gameObject, DirectionUpdate, currentVelocity,
                velocity, interpolationTime).setEase(easeType).setOnComplete(() => {

                    currentVelocity = grassRenderer.material.GetFloat("_TouchDirection");
                    tweenID = LeanTween.value(gameObject, DirectionUpdate, currentVelocity,
                oldVelocity, interpolationTime).setEase(easeType).id;
                }).id;
        }
    }

    private void DirectionUpdate(float val)
    {
        grassRenderer.material.SetFloat("_TouchDirection", val);
    }
}
