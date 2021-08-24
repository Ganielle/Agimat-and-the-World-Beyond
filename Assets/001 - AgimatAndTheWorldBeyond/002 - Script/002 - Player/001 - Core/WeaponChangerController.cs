using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;

public class WeaponChangerController : MonoBehaviour
{
    [SerializeField] private Core core;

    [Header("DEBUGGER")]
    //  Switching Weapons
    [ReadOnly] public float lastShowWeaponSlotsTime;

    private void OnEnable()
    {
        GameManager.instance.gameplayController.onSwitchWeaponInputChange += WeaponInputChange;
    }

    private void OnDisable()
    {
        GameManager.instance.gameplayController.onSwitchWeaponInputChange -= WeaponInputChange;
    }

    private void WeaponInputChange(object sender, EventArgs e)
    {
        if (GameManager.instance.gameplayController.GetWeaponSwitchInput > 0)
            lastShowWeaponSlotsTime = Time.time;
    }

    //  This is for changing weapon in inventory
    public void ChangeWeapon()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
        {
            if (GameManager.instance.PlayerInventory.GetLukasWeapons.Count > 1)
            {
                if (GameManager.instance.gameplayController.canSwitchWeapon &&
                GameManager.instance.gameplayController.GetWeaponSwitchInput == 2)
                {
                    if (GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex <
                        GameManager.instance.PlayerInventory.GetLukasWeapons.Count - 1)
                        GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex++;

                    else if (GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex ==
                        GameManager.instance.PlayerInventory.GetLukasWeapons.Count - 1)
                        GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex = 0;
                }

                WeaponIndexChanger(GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex,
                    GameManager.instance.PlayerInventory.GetLukasWeapons);
            }
        }
        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
        {
            if (GameManager.instance.PlayerInventory.GetLilyWeapons.Count > 1)
            {
                if (GameManager.instance.gameplayController.canSwitchWeapon &&
                GameManager.instance.gameplayController.GetWeaponSwitchInput == 2)
                {
                    if (GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex <
                        GameManager.instance.PlayerInventory.GetLilyWeapons.Count - 1)
                        GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex++;

                    else if (GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex ==
                        GameManager.instance.PlayerInventory.GetLilyWeapons.Count - 1)
                        GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex = 0;
                }

                WeaponIndexChanger(GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex,
                    GameManager.instance.PlayerInventory.GetLilyWeapons);
            }
        }
    }

    private void WeaponIndexChanger(int index, List<WeaponData> weaponDatas)
    {
        weaponDatas[index].GetSetEquipState = true;

        GameManager.instance.PlayerStats.GetSetWeaponEquipBoolInPlayerAnim =
            weaponDatas[index].GetSetWeaponBoolNameInPlayerAnim;

        if (index == 0)
            weaponDatas[weaponDatas.Count - 1].GetSetEquipState = false;
        else if (index > 0)
            weaponDatas[index - 1].GetSetEquipState = false;
    }

    //  This is for showing animation
    public void SwitchWeapon()
    {
        if (core.statemachineController.weaponSwitchState.CheckIfCanWeaponSwitch() &&
            GameManager.instance.gameplayController.canSwitchWeapon &&
            GameManager.instance.gameplayController.GetWeaponSwitchInput == 2)
        {
            core.statemachineController.core.weaponChangerController.ChangeWeapon();

            if (core.groundPlayerController.CheckIfTouchGround &&
                    (GameManager.instance.PlayerStats.GetSetAnimatorStateInfo ==
                    PlayerStats.AnimatorStateInfo.IDLE ||
                    GameManager.instance.PlayerStats.GetSetAnimatorStateInfo ==
                    PlayerStats.AnimatorStateInfo.SWITCHWEAPON))
            {
                core.statemachineController.weaponSwitchState.animBoolName =
                    GameManager.instance.PlayerStats.GetSetWeaponEquipBoolInPlayerAnim;
                core.statemachineController.statemachineChanger.ChangeState(core.statemachineController.weaponSwitchState);
            }

            GameManager.instance.gameplayController.UseCanSwitchWeaponInput();
        }
    }

    public void DoneSwitchingWeapon()
    {
        if (Time.time >= lastShowWeaponSlotsTime + 5f
            && GameManager.instance.gameplayController.GetWeaponSwitchInput != 0)
            GameManager.instance.gameplayController.ResetSwitchWeaponInput();
    }
}
