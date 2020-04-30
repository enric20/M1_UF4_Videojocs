using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealoadBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public Text pistolCurrentBulletsText;
    private int pistolMaxBullets;

    public Text smgCurrentBulletsText;
    private int smgMaxBullets;

    public Text shellsText;
    private int maxShells;

    public Text nineMMAmmoCapacityText;
    public Text shellsAmmoCapacityText;


    public void SetMaxReloadCD(int reloadCD)
    {
        slider.maxValue = reloadCD;
        slider.value = reloadCD;
        fill.color = gradient.Evaluate(1f);

    }

    public void SetReloadCD(int reloadCD)
    {
        slider.value = reloadCD;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetAmmunition(int currentAmmunition, int currentWeaponType)
    {
        switch (currentWeaponType)
        {
            case 1: //Pistol
                pistolCurrentBulletsText.text = "" + currentAmmunition + "/" + pistolMaxBullets;
                break;
            case 2: //SMG
                smgCurrentBulletsText.text = "" + currentAmmunition + "/" + smgMaxBullets;
                break;
            case 3: //Shotgun
                shellsText.text = "" + currentAmmunition + "/" + maxShells;
                break;
        }
       
    }

    public void SetMaxAmmunition(int pistolMaxBulletsLocal, int smgMaxBulletsLocal, int maxShotgunShellsLocal)
    {
        pistolMaxBullets = pistolMaxBulletsLocal;
        smgMaxBullets = smgMaxBulletsLocal;
        maxShells = maxShotgunShellsLocal;
    }

    public void SetAmmoCapacity(int nineMMAmmoCapacity, int shellsAmmoCapacity)
    {
        nineMMAmmoCapacityText.text = "9mm " + nineMMAmmoCapacity;
        shellsAmmoCapacityText.text = "shells " + shellsAmmoCapacity;
    }
}
