using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealoadBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public Text pistolCurrentBulletsText; //1
    private int pistolMaxBullets;

    public Text smgCurrentBulletsText; //2
    private int smgMaxBullets;

    public Text shellsText; //3
    private int maxShells;

    public Text rifleCurrentBulletsText; //4
    private int rifleMaxBullets;
    //f
    public Text sniperCurrentBulletsText; //5
    private int sniperMaxBullets;

    public Text nineMMAmmoCapacityText; //1-2
    public Text shellsAmmoCapacityText; //3
    public Text rifleAmmoCapacityText; //4
    public Text sniperAmmoCapacityText; //5


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
            case 4: //Rifle
                rifleCurrentBulletsText.text = "" + currentAmmunition + "/" + rifleMaxBullets;
                break;
            case 5: //Sniper
                sniperCurrentBulletsText.text = "" + currentAmmunition + "/" + sniperMaxBullets;
                break;
        }
       
    }

    public void SetMaxAmmunition(int pistolMaxBulletsLocal, int smgMaxBulletsLocal, int maxShotgunShellsLocal, int rifleMaxBulletsLocal, int sniperMaxBulletsLocal)
    {
        pistolMaxBullets = pistolMaxBulletsLocal;
        smgMaxBullets = smgMaxBulletsLocal;
        maxShells = maxShotgunShellsLocal;
        rifleMaxBullets = rifleMaxBulletsLocal;
        sniperMaxBullets = sniperMaxBulletsLocal;
    }

    public void SetAmmoCapacity(int nineMMAmmoCapacity, int shellsAmmoCapacity, int rifleAmmoCapacity, int sniperAmmoCapacity)
    {
        nineMMAmmoCapacityText.text = "9mm: " + nineMMAmmoCapacity;
        shellsAmmoCapacityText.text = "shells: " + shellsAmmoCapacity;
        rifleAmmoCapacityText.text = "7.62mm: " + rifleAmmoCapacity;
        sniperAmmoCapacityText.text = "High Caliber: " + sniperAmmoCapacity;
    }
}
