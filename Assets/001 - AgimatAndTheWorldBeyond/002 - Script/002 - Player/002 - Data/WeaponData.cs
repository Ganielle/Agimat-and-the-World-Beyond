using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponData
{
    string weapID;
    public string GetSetWeaponID
    {
        get { return weapID; }
        set { weapID = "weapon_ID_" + value; }
    }

    PlayerWeaponRawData.WeaponType weaponType;
    public PlayerWeaponRawData.WeaponType CurrentWeaponType
    {
        get => weaponType;
        set => weaponType = value;
    }

    private event EventHandler equipStateChange;
    public event EventHandler onEquipStateChange
    {
        add
        {
            if (equipStateChange == null || !equipStateChange.GetInvocationList().Contains(value))
                equipStateChange += value;
        }
        remove { equipStateChange -= value; }
    }
    bool isEquip;
    public bool GetSetEquipState
    {
        get { return isEquip; }
        set { isEquip = value; }
    }

    float damage;
    public float GetSetDamage
    {
        get { return damage; }
        set { damage = value; }
    }

    string weaponBoolNameInPlayerAnimator;
    public string GetSetWeaponBoolNameInPlayerAnim
    {
        get { return weaponBoolNameInPlayerAnimator; }
        set { weaponBoolNameInPlayerAnimator = value; }
    }

    GameObject uiGameplayObj;
    public GameObject GetSetGameplayWeaponUIObj
    {
        get { return uiGameplayObj; }
        set { uiGameplayObj = value; }
    }
}
