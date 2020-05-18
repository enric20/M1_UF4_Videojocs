using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStadistics : MonoBehaviour
{
    /**This script is used for manipulating stadistics, these stadistics will be saved on a file and will be loaded
     * opening the game */
    PlayerController playerController;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SaveStadistics (int hp, int strength, int magicAtk, int mana, int physDef, int magDef, int speed, int stealth, int stamina)
    {

    }

    public static void LoadStadistics (int hp, int strength, int magicAtk, int mana, int physDef, int magDef, int speed, int stealth, int stamina)
    {

    }
    /**
     * Reloads all the actual stadistics to the new adquired
     * */
    public void RenewStadistics()
    {
        playerController.healthPoints = playerController.startingHealthPoints;
        playerController.strength = playerController.startingStrength;
        playerController.intellect = playerController.startingIntellect;
        playerController.manaPoints = playerController.startingManaPoints;
        playerController.physicalDefense = playerController.startingPhysicalDefense;
        playerController.magicalDefense = playerController.startingMagicalDefense;
        playerController.stamina = playerController.startingStamina;
        
    }

}
