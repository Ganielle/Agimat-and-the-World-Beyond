using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroupFadeAnimation fadeAnimation;
    [SerializeField] private CanvasGroupFadeAnimation lukasWeaponPanelFadeAnimation;
    [SerializeField] private CanvasGroupFadeAnimation lilyWeaponPanelFadeAnimation;

    [Header("Slider")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Image chargeSkillSlider;

    [Header("Fade Animations")]
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private float showAnimSpeed;
    [SerializeField] private float hideAnimDelay;

    [Header("WeaponSlots")]
    [SerializeField] private WeaponSlotEquipController lukasEquipWeaponSlot;
    [SerializeField] private WeaponSlotEquipController lilyEquipWeaponSlot;
    [SerializeField] private List<WeaponSlotEquipController> lukasWeaponSlots;
    [SerializeField] private List<WeaponSlotEquipController> lilyWeaponSlots;
    [SerializeField] private GameObject lukasWeaponPanel;
    [SerializeField] private GameObject lilyWeaponPanel;

    [Header("Debugger")]
    //  Health
    [ReadOnly] [SerializeField] private float currentHealth;
    [ReadOnly] [SerializeField] float currentMana;

    private void OnEnable()
    {
        PanelFadeAnimation();

        ChangeWeaponEquip(GameManager.instance.PlayerInventory.GetLukasWeapons,
            lukasEquipWeaponSlot, GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex);

        ChangeWeaponEquip(GameManager.instance.PlayerInventory.GetLilyWeapons,
            lilyEquipWeaponSlot, GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex);

        GameManager.instance.PlayerStats.onPlayerBattleStateChange += BattleStateChange;
        GameManager.instance.gameplayController.onSwitchWeaponInputChange += SwitchWeaponInputChange;
        GameManager.instance.PlayerStats.onPlayerCharacterChange += CharacterChange;
        GameManager.instance.PlayerInventory.onLukasSlotIndexChange += LukasChangeEquipWeapon;
        GameManager.instance.PlayerInventory.onLilySlotIndexChange += LilyChangeEquipWeapon;
    }

    private void OnDisable()
    {
        GameManager.instance.PlayerStats.onPlayerBattleStateChange -= BattleStateChange;
        GameManager.instance.gameplayController.onSwitchWeaponInputChange -= SwitchWeaponInputChange;
        GameManager.instance.PlayerStats.onPlayerCharacterChange -= CharacterChange;
        GameManager.instance.PlayerInventory.onLukasSlotIndexChange -= LukasChangeEquipWeapon;
        GameManager.instance.PlayerInventory.onLilySlotIndexChange -= LilyChangeEquipWeapon;
    }

    private void Start()
    {
        WeaponIconSpawner();
    }

    private void Update()
    {
        HealthBarUpdate();
        ManaBarUpdate();
        ChargeSkillUpdate();
    }

    private void BattleStateChange(object sender, EventArgs e)
    {
        PanelFadeAnimation();
    }

    private void SwitchWeaponInputChange(object sender, EventArgs e)
    {
        PanelFadeAnimation();
    }

    private void CharacterChange(object sender, EventArgs e)
    {
        ChangeWeaponPanel();
    }

    private void LukasChangeEquipWeapon(object sender, EventArgs e)
    {
        ChangeWeaponEquip(GameManager.instance.PlayerInventory.GetLukasWeapons,
            lukasEquipWeaponSlot, GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex);
    }

    private void LilyChangeEquipWeapon(object sender, EventArgs e)
    {
        ChangeWeaponEquip(GameManager.instance.PlayerInventory.GetLilyWeapons,
            lilyEquipWeaponSlot, GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex);
    }

    #region FADE ANIMATION

    private void PanelFadeAnimation()
    {
        if (GameManager.instance.PlayerStats.GetSetBattleState ==
            PlayerStats.PlayerBattleState.ADVENTURING)
        {
            if (GameManager.instance.gameplayController.GetWeaponSwitchInput == 0)
            {
                fadeAnimation.CanvasGroupAnimation(easeType,
                    showAnimSpeed, hideAnimDelay, 0f);
            }
            else if (GameManager.instance.gameplayController.GetWeaponSwitchInput == 1)
            {
                fadeAnimation.CanvasGroupAnimation(easeType,
                    showAnimSpeed, 0f, 1f);
            }
        }
        else if (GameManager.instance.PlayerStats.GetSetBattleState ==
            PlayerStats.PlayerBattleState.BATTLE)
        {
            fadeAnimation.CanvasGroupAnimation(easeType,
                showAnimSpeed, 0f, 1f);
        }
    }

    private void ChangeWeaponPanel()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
        {
            lukasWeaponPanel.SetActive(true);

            lilyWeaponPanelFadeAnimation.CanvasGroupAnimation(easeType,
                showAnimSpeed, 0f, 0f, () => lilyWeaponPanel.SetActive(false));
            lukasWeaponPanelFadeAnimation.CanvasGroupAnimation(easeType,
                showAnimSpeed, 0f, 1f);
        }
        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter
            == PlayerStats.PlayerCharacter.LILY)
        {
            lilyWeaponPanel.SetActive(true);

            lukasWeaponPanelFadeAnimation.CanvasGroupAnimation(easeType,
                showAnimSpeed, 0f, 0f, () => lukasWeaponPanel.SetActive(false));
            lilyWeaponPanelFadeAnimation.CanvasGroupAnimation(easeType,
                showAnimSpeed, 0f, 1f);
        }
    }

    #endregion

    #region HEALTH MANA

    private float CalculateCurrentHealth()
    {
        currentHealth = GameManager.instance.PlayerStats.GetSetCurrentHealth /
            100f;

        return currentHealth;
    }

    private void HealthBarUpdate() => healthSlider.value = CalculateCurrentHealth();

    private float CalculateCurrentMana()
    {
        currentMana = GameManager.instance.PlayerStats.GetSetCurrentMana /
            100f;

        return currentMana;
    }

    private void ManaBarUpdate() => manaSlider.value = CalculateCurrentMana();

    private void ChargeSkillUpdate() => chargeSkillSlider.fillAmount = 
        GameManager.instance.PlayerStats.GetSetChargeMana / 100f;

    #endregion

    #region WEAPONS SLOT

    private void ChangeWeaponEquip(List<WeaponData> datas, 
        WeaponSlotEquipController weaponSlot, int index)
    {
        if (weaponSlot.weaponGameplayUI != null)
        {
            Destroy(weaponSlot.weaponGameplayUI);
            weaponSlot.weaponGameplayUI = null;
        }

        GameObject slot = Instantiate(datas[index].GetSetGameplayWeaponUIObj, weaponSlot.transform);

        weaponSlot.weaponIndex = index;
        weaponSlot.weaponGameplayUI = slot;
    }

    public void WeaponIconSpawner()
    {
        WeaponSlot(GameManager.instance.PlayerInventory.GetLukasWeapons, lukasWeaponSlots);
        WeaponSlot(GameManager.instance.PlayerInventory.GetLilyWeapons, lilyWeaponSlots);
        /*
         *  TODO: FOR LILY WEAPON ICON SPAWNER 
         */

        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            lukasWeaponPanel.SetActive(true);

        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            lilyWeaponPanel.SetActive(true);

        ChangeWeaponPanel();
    }

    public void WeaponSlot(List<WeaponData> datas, List<WeaponSlotEquipController> weaponList)
    {
        for (int a = 0; a < datas.Count; a++)
        {
            GameObject weapon = Instantiate(datas[a].GetSetGameplayWeaponUIObj, weaponList[a].transform);

            weaponList[a].weaponIndex = a;
            weaponList[a].weaponGameplayUI = weapon;
        }
    }

    #endregion
}
