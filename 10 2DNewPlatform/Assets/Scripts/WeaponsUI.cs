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

    public void SetCurrentWeaponUI(int currentWeaponLocal) //Estableix l'arma utilitzada actualment, la mostrara en la interfície amb més brillantor
    {
        DebugUI(); //S'utilitza per a establir totes les armes com a no usades
        currentWeapon = currentWeaponLocal;
        switch (currentWeaponLocal) //Estableix l'arma usada
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

    public void SetAvailableWeaponsUI(bool[] availableWeaponsArray) //Estableix les armes NO DISPONIBLES, no les mostrara
    {
        //DebugUI();
        for (int i = 0; i < availableWeaponsArray.Length; i++)
        {
            if (availableWeaponsArray[i])
            {
                switch (i)
                {
                    case 1:
                        pistol.SetActive(true);
                        break;
                    case 2:
                        smg.SetActive(true);
                        break;
                    case 3:
                        shotgun.SetActive(true);
                        break;
                    case 4:
                        rifle.SetActive(true);
                        break;
                    case 5:
                        sniper.SetActive(true);
                        break;
                }
            }
        }
    }

    private void DebugUI() //Utilitza aquesta funcio al principi del joc per a arreglar la UI
    {
        pistol.SetActive(false);
        smg.SetActive(false);
        shotgun.SetActive(false);
        rifle.SetActive(false);
        sniper.SetActive(false);

        pistolUI.color = notUsingWeapon;
        smgUI.color = notUsingWeapon;
        shotgunUI.color = notUsingWeapon;
        rifleUI.color = notUsingWeapon;
        sniperUI.color = notUsingWeapon;

    }

}
