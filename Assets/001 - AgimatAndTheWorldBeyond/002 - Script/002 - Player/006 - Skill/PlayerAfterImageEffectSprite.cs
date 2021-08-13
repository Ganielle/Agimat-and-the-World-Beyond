using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageEffectSprite : MonoBehaviour
{

    [SerializeField] private float alphaSet = 0.8f;
    [SerializeField] private float activeTime = 0.1f;
    [SerializeField] private float alphaMultiplier = 0.85f;
    [SerializeField] private SpriteRenderer sr;


    [Header("Debugger")]
    [ReadOnly] [SerializeField] private Transform player;
    [ReadOnly] [SerializeField] private SpriteRenderer playerSR;
    [ReadOnly] [SerializeField] private float timeActivated;
    [ReadOnly] [SerializeField] private float alpha;
    [ReadOnly] [SerializeField] bool firstInstance;
    private Color color;

    private void Awake()
    {
        firstInstance = true;
    }

    private void OnEnable()
    {
        SetSettings();
    }

    private void PlayerObjChange(object sender, EventArgs e)
    {
        SetSettings();
    }

    private void SetSettings()
    {
        if (!firstInstance)
        {
            GameManager.instance.PlayerStats.onPlayerCharacterObjChange += PlayerObjChange;

            player = GameManager.instance.PlayerStats.GetSetPlayerCharacterObj.transform;
            playerSR = GameManager.instance.PlayerStats.GetSetPlayerSR;

            alpha = alphaSet;
            sr.sprite = playerSR.sprite;
            transform.position = player.position;
            transform.rotation = player.rotation;
            timeActivated = Time.time;
        }

        if (firstInstance)
            firstInstance = false;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        sr.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            GameManager.instance.afterImagePooler.AddToPool(gameObject);
        }
    }
}
