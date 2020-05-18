using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    //ID of the item, used as secondary key for databases (ID is primary key)
    /// </summary>
    //public int[] itemID = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
    public int itemID;
    //Name of the item (name is unique)
    public string itemName;
    //public string[] itemName = {"Sword", "Potion", "Chestplate", "", "", "", "", "", "", ""};
    //Weapon, Consumable, Armor
    public string itemType;
    //public string[] itemType = {"Weapon", "Consumable", "Armor", "", "", "", "", "", "", "", ""};
    //Common [White], Uncommon [Green], Rare [Blue], Epic [Purple], Legendary [Yellow], Ultimate [Red]
    public string itemRarity;
    //public string[] itemRarity = { "Common", "Common", "Common", "", "", "", "", "", "", ""};


    
}
