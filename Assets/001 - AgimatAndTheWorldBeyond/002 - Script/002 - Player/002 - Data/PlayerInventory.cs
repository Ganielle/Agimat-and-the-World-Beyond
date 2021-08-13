using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory
{
    private event EventHandler lukasWeaponInventoryChange;
    public event EventHandler onLukasWeaponInventoryChange
    {
        add
        {
            if (lukasWeaponInventoryChange == null || !lukasWeaponInventoryChange.GetInvocationList().
                Contains(value))
                lukasWeaponInventoryChange += value;
        }
        remove { lukasWeaponInventoryChange -= value; }
    }
    private event EventHandler lilyWeaponInventoryChange;
    public event EventHandler onLilyWeaponInventoryChange
    {
        add
        {
            if (lukasWeaponInventoryChange == null || !lukasWeaponInventoryChange.GetInvocationList().
                Contains(value))
                lukasWeaponInventoryChange += value;
        }
        remove { lukasWeaponInventoryChange -= value; }
    }
    List<WeaponData> lukasWeaponList = new List<WeaponData>(), lilyWeaponList = new List<WeaponData>();
    public List<WeaponData> GetLukasWeapons
    {
        get { return lukasWeaponList; }
    }
    public void AddLukasWeapons(WeaponData data)
    {
        if (lukasWeaponList.Contains(data))
        {
            Debug.Log("already have weapon lukas");
            return;
        }

        lukasWeaponList.Add(data);
        lukasWeaponInventoryChange?.Invoke(this, EventArgs.Empty);
    }
    public List<WeaponData> GetLilyWeapons
    {
        get { return lilyWeaponList; }
    }
    public void AddLilyWeapons(WeaponData data)
    {
        if (lilyWeaponList.Contains(data))
        {
            Debug.Log("already have weapon liy");
            return;
        }

        lilyWeaponList.Add(data);
        lilyWeaponInventoryChange?.Invoke(this, EventArgs.Empty);
    }

    private event EventHandler lukasSlotIndexChange;
    public event EventHandler onLukasSlotIndexChange
    {
        add
        {
            if (lukasSlotIndexChange == null || !lukasSlotIndexChange.GetInvocationList().Contains(value))
                lukasSlotIndexChange += value;
        }
        remove { lukasSlotIndexChange -= value; }
    }
    int weaponLukasSlotIndex;
    public int GetSetWeaponLukasSlotIndex
    {
        get { return weaponLukasSlotIndex; }
        set 
        { 
            weaponLukasSlotIndex = value;
            lukasSlotIndexChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private event EventHandler lilySlotIndexChange;
    public event EventHandler onLilySlotIndexChange
    {
        add
        {
            if (lilySlotIndexChange == null || !lilySlotIndexChange.GetInvocationList().Contains(value))
                lilySlotIndexChange += value;
        }
        remove { lilySlotIndexChange -= value; }
    }
    int weaponLilySlotIndex;
    public int GetSetWeaponLilySlotIndex
    {
        get { return weaponLilySlotIndex; }
        set
        {
            weaponLilySlotIndex = value;
            lilySlotIndexChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
