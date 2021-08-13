using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerParticleController : MonoBehaviour
{
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private List<SpriteRenderer> effectList;
    [SerializeField] private float speed;

    [SerializeField] private float activeTime = 2.1f;

    [Header("Debugger")] 
    [ReadOnly] [SerializeField] private Transform player;
    [ReadOnly] [SerializeField] private float timeActivated;
    [ReadOnly] public bool isCanceled;
    [ReadOnly] [SerializeField] bool firstInstance;

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
        gameObject.GetComponent<Animator>().enabled = true;

        if (!firstInstance)
        {
            GameManager.instance.PlayerStats.onPlayerCharacterObjChange += PlayerObjChange;

            player = GameManager.instance.PlayerStats.GetSetPlayerCharacterObj.transform;

            transform.position = player.position;
            transform.rotation = player.rotation;

            foreach (SpriteRenderer renderer in effectList)
                renderer.color = new Color(1f, 1f, 1f, 1f);

            timeActivated = Time.time;
        }

        if (firstInstance)
            firstInstance = false;
    }

    private void Update()
    {
        if (Time.time >= (timeActivated + activeTime) && !isCanceled)
            GameManager.instance.switchEffectPooler.AddToPool(gameObject);
    }

    public IEnumerator StopParticles()
    {
        gameObject.GetComponent<Animator>().enabled = false;
        
        foreach(SpriteRenderer renderer in effectList)
        {
            LeanTween.value(renderer.gameObject, alpha => renderer.color = alpha,
                new Color(renderer.color.r, renderer.color.g,
                renderer.color.b, renderer.color.a),
                new Color(renderer.color.r, renderer.color.g,
                renderer.color.b, 0f), speed).setEase(easeType);

            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        isCanceled = false;
        GameManager.instance.switchEffectPooler.AddToPool(gameObject);
    }
}
