using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsUI : MonoBehaviour
{

    private int currentWeapon = 0;
    public Color usingWeapon;
    public Color notUsingWeapon;
    public Color unavailableWeapon;

    public Image pistolUI;
    public GameObject pistol;
    public Image smgUI;
    public GameObject smg;
    public Image shotgunUI;
    public GameObject shotgun;

    void Start()
    {
        DebugUI();
    }

    public void SetCurrentWeaponUI(int currentWeaponLocal)
    {
        DebugUI();
        currentWeapon = currentWeaponLocal;
        switch (currentWeaponLocal)
        {
            case 1:
                pistolUI.color = usingWeapon;
                break;
            case 2:
                smgUI.color = usingWeapon;
                break;
            case 3:
                shotgunUI.color = usingWeapon;
                break;
        }
    }

    public void SetAvailableWeaponsUI(bool[] availableWeaponsArray)
    {
        for (int i = 0; i < availableWeaponsArray.Length; i++)
        {
            if (!availableWeaponsArray[i])
            {
                switch (i)
                {
                    case 1:
                        //pistolUI.color = unavailableWeapon;
                        pistol.SetActive(false);
                        break;
                    case 2:
                        smg.SetActive(false);
                        //smgUI.color = unavailableWeapon;
                        break;
                    case 3:
                        shotgun.SetActive(false);
                        //shotgunUI.color = unavailableWeapon;
                        break;
                }
            }
        }
    }

    private void DebugUI()
    {
        pistol.SetActive(true);
        smg.SetActive(true);
        shotgun.SetActive(true);
        smgUI.color = notUsingWeapon;
        shotgunUI.color = notUsingWeapon;
        pistolUI.color = notUsingWeapon;
    }

}
