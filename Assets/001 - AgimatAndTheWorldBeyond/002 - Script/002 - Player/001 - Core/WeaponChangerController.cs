using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MyBox;

public class WeaponChangerController : MonoBehaviour
{
    //  Switching Weapons
    [ReadOnly] public float lastShowWeaponSlotsTime;

    private void OnEnable()
    {
        GameManager.instance.gameInputController.onSwitchWeaponInputChange += WeaponInputChange;
    }

    private void OnDisable()
    {
        GameManager.instance.gameInputController.onSwitchWeaponInputChange -= WeaponInputChange;
    }

    private void WeaponInputChange(object sender, EventArgs e)
    {
        if (GameManager.instance.gameInputController.GetWeaponSwitchInput > 0)
            lastShowWeaponSlotsTime = Time.time;
    }

    public void ChangeWeapon()
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
        {
            if (GameManager.instance.PlayerInventory.GetLukasWeapons.Count > 1)
            {
                if (GameManager.instance.gameInputController.canSwitchWeapon &&
                GameManager.instance.gameInputController.GetWeaponSwitchInput == 2)
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
                if (GameManager.instance.gameInputController.canSwitchWeapon &&
                GameManager.instance.gameInputController.GetWeaponSwitchInput == 2)
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
}
