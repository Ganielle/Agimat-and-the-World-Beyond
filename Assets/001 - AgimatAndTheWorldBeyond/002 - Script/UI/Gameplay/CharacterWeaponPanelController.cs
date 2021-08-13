using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponPanelController : MonoBehaviour
{
    [SerializeField] private Animator animatorController;
    [SerializeField] private PlayerStats.PlayerCharacter character;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] bool canShowWeapon;

    private void OnEnable()
    {
        canShowWeapon = true;

        GameManager.instance.gameInputController.onSwitchWeaponInputChange += IndexChange;
    }

    private void OnDisable()
    {
        animatorController.SetBool("showWeapon", false);
        animatorController.SetBool("hideWeapon", true);

        GameManager.instance.gameInputController.onSwitchWeaponInputChange -= IndexChange;
    }

    private void IndexChange(object sender, EventArgs e)
    {
        ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        if (GameManager.instance.gameInputController.GetWeaponSwitchInput == 0)
        {
            animatorController.SetBool("showWeapon", false);
            animatorController.SetBool("hideWeapon", true);

            canShowWeapon = true;
        }
        
        else if (GameManager.instance.gameInputController.GetWeaponSwitchInput == 1 && canShowWeapon)
        {
            animatorController.SetBool("hideWeapon", false);
            animatorController.SetBool("showWeapon", true);

            canShowWeapon = false;
        }
    }

    public void ChangeCanChangeWeapon(bool value) =>
        GameManager.instance.gameInputController.canSwitchWeapon = value;
}
