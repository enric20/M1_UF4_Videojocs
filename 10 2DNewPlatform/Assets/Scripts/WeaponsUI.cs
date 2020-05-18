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

    public Image pistolUI; //1
    public GameObject pistol;

    public Image smgUI; //2
    public GameObject smg;

    public Image shotgunUI; //3
    public GameObject shotgun;

    public Image rifleUI; //4
    public GameObject rifle;

    public Image sniperUI; //5
    public GameObject sniper;

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
            case 4:
                rifleUI.color = usingWeapon;
                break;
            case 5:
                sniperUI.color = usingWeapon;
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
                    case 4:
                        rifle.SetActive(false);
                        break;
                    case 5:
                        sniper.SetActive(false);
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
        rifle.SetActive(true);
        sniper.SetActive(true);

        pistolUI.color = notUsingWeapon;
        smgUI.color = notUsingWeapon;
        shotgunUI.color = notUsingWeapon;
        rifleUI.color = notUsingWeapon;
        sniperUI.color = notUsingWeapon;

    }

}
