using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataLoadingController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool debugMode;
    [SerializeField] private List<PlayerWeaponRawData> weaponDataDebug;

    #region WEAPON SPAWNER DEBUG

    public IEnumerator DebugInsertWeapons()
    {
        if (GameManager.instance.debugMode)
        {
            if (GameManager.instance.isLukas)
                GameManager.instance.PlayerStats.GetSetPlayerCharacter = PlayerStats.PlayerCharacter.LUKAS;
            else
                GameManager.instance.PlayerStats.GetSetPlayerCharacter = PlayerStats.PlayerCharacter.LILY;
        }

        if (!debugMode)
            yield break;

        /* 
         *  NOTE 
         *  weaponID format: weapon_ID_<number>
         */


        foreach (PlayerWeaponRawData data in weaponDataDebug)
        {
            if (data.character == PlayerStats.PlayerCharacter.LUKAS)
            {
                GameManager.instance.PlayerInventory.AddLukasWeapons(new WeaponData
                {
                    GetSetWeaponID = data.weaponID,
                    CurrentWeaponType = data.weaponType,
                    GetSetDamage = data.damage,
                    GetSetEquipState = false,
                    GetSetWeaponBoolNameInPlayerAnim = data.boolNameInPlayerAnimator,
                    GetSetGameplayWeaponUIObj = data.gameplayUIWeapon
                });
            }
            else if (data.character == PlayerStats.PlayerCharacter.LILY)
            {
                GameManager.instance.PlayerInventory.AddLilyWeapons(new WeaponData
                {
                    GetSetWeaponID = data.weaponID,
                    CurrentWeaponType = data.weaponType,
                    GetSetDamage = data.damage,
                    GetSetEquipState = false,
                    GetSetWeaponBoolNameInPlayerAnim = data.boolNameInPlayerAnimator,
                    GetSetGameplayWeaponUIObj = data.gameplayUIWeapon
                });
            }

            yield return null;
        }


        yield return StartCoroutine(SetWeaponEquipState(GameManager.instance.PlayerInventory.GetLukasWeapons
            , true));

        yield return StartCoroutine(SetWeaponEquipState(GameManager.instance.PlayerInventory.GetLilyWeapons
            , false));
    }

    IEnumerator SetWeaponEquipState(List<WeaponData> weaponDatas, bool isLukas)
    {

        if (GameManager.instance.debugMode)
        {
            weaponDatas[0].GetSetEquipState = true;
        }

        for (int a = 0; a < weaponDatas.Count; a++)
        {
            if (weaponDatas[a].GetSetEquipState)
            {
                /* 
                 * TO DO: SET WEAPON SLOT INDEX FOR BOTH CHARACTER 
                 */
                if (isLukas)
                    GameManager.instance.PlayerInventory.GetSetWeaponLukasSlotIndex = a;
                else
                    GameManager.instance.PlayerInventory.GetSetWeaponLilySlotIndex = a;

                break;
            }

            yield return null;
        }
    }

    #endregion
}
