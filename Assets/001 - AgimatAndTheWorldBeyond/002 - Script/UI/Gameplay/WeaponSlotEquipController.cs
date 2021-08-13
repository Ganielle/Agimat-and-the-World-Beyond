using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotEquipController : MonoBehaviour
{
    [SerializeField] private BottomUIController bottomUIController;

    [ReadOnly] public int weaponIndex;
    [ReadOnly] public GameObject weaponGameplayUI;

    private void OnEnable()
    {
        EventSetter(true);
    }

    private void OnDisable()
    {
        EventSetter(false);
    }

    private void EventSetter(bool isEnable)
    {
        if (isEnable)
        {
            if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            {
                CheckIfWeaponEquip(GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex);
                GameManager.instance.PlayerInventory.onLukasSlotIndexChange += EquipStateChange;
                GameManager.instance.PlayerInventory.onLukasWeaponInventoryChange += InventoryChange;
            }
            else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            {
                CheckIfWeaponEquip(GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex);
                GameManager.instance.PlayerInventory.onLilySlotIndexChange += EquipStateChange;
                GameManager.instance.PlayerInventory.onLilyWeaponInventoryChange += InventoryChange;
            }
        }
        else
        {
            if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            {
                GameManager.instance.PlayerInventory.onLukasSlotIndexChange -= EquipStateChange;
                GameManager.instance.PlayerInventory.onLukasWeaponInventoryChange -= InventoryChange;
            }
            else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            {
                GameManager.instance.PlayerInventory.onLilySlotIndexChange -= EquipStateChange;
                GameManager.instance.PlayerInventory.onLilyWeaponInventoryChange -= InventoryChange;
            }
        }
    }

    private void InventoryChange(object sender, EventArgs e)
    {
        Destroy(weaponGameplayUI);
        weaponGameplayUI = null;
    }

    private void EquipStateChange(object sender, EventArgs e)
    {
        if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LUKAS)
            CheckIfWeaponEquip(GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex);
        else if (GameManager.instance.PlayerStats.GetSetPlayerCharacter == PlayerStats.PlayerCharacter.LILY)
            CheckIfWeaponEquip(GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex);
    }

    private void CheckIfWeaponEquip(int characterSlotIndex)
    {
        if (weaponGameplayUI != null)
        {
            if (characterSlotIndex == weaponIndex)
            {
                weaponGameplayUI.transform.GetChild(0).gameObject.SetActive(false);
                weaponGameplayUI.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                weaponGameplayUI.transform.GetChild(0).gameObject.SetActive(true);
                weaponGameplayUI.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
