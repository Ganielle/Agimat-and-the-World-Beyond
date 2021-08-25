using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Agimat and the World Beyond/Player Data/Weapon Data")]
public class PlayerWeaponRawData : ScriptableObject
{
    public enum WeaponType
    {
        NONE,
        AXE,
        BOW,
        SPEAR,
        ARNIS,
        SWORD,
        WHIP,
        STAFF,
        KARAMBIT
    }
    public string weaponID = "000";
    public WeaponType weaponType;
    public PlayerStats.PlayerCharacter character;
    public float damage;
    public string boolNameInPlayerAnimator;
    public GameObject gameplayUIWeapon;
}
