 using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //  this is for calling the public reference variable in
    //  every script 
    public static GameManager instance;

    //  =======================================

    //  PLAYER SCRIPS
    public PlayerStats PlayerStats = new PlayerStats();
    public PlayerInventory PlayerInventory = new PlayerInventory();

    //  CAMERA
    public CameraShaker cameraShaker = new CameraShaker();

    //  =======================================

    public bool debugMode;
    [ConditionalField("debugMode")] public bool isLukas;
    [ConditionalField("debugMode")] public bool debugScenes;
    [ConditionalField("debugMode")] [SerializeField] private string firstScene;

    [Header("ScriptReferences")]
    public GameplayController gameplayController;
    public SceneDataLoadingController sceneDataLoading;
    public EffectPooler afterImagePooler;
    public EffectPooler switchEffectPooler;

    [Header("Camera")]
    public Camera mainCamera;
    public Camera uiCamera;
    public Camera mouseCamera;

    private void Awake()
    {
        instance = this;

        SetReferenceScripts();

        if (debugMode)
        {
            StartCoroutine(LoadData());

            //  Debug stamina
            PlayerStats.GetSetLukasStamina = 100f;
            PlayerStats.GetSetLilyStamina = 100f;

            //  Debug Health
            PlayerStats.GetSetLukasHealth = 100f;
            PlayerStats.GetSetLilyHealth = 100f;

            //  Debug Mana
            PlayerStats.GetSetLukasMana = 100f;
            PlayerStats.GetSetLilyMana = 100f;
        }
    }

    private void SetReferenceScripts()
    {
        PlayerStats = new PlayerStats();
        cameraShaker = new CameraShaker();
    }

    #region LOADING DATA SCENES

    IEnumerator LoadData()
    {
        yield return StartCoroutine(sceneDataLoading.DebugInsertWeapons());

        if (debugScenes)
        {
            SceneManager.LoadSceneAsync(firstScene, LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("CharactersScene", LoadSceneMode.Additive);
        }
    }


    #endregion

    public void CoroutineRunner(IEnumerator coroutine) => StartCoroutine(coroutine);
}
