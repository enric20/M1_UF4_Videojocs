using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    private PlayerController playerController;
    private ArrayOrdination arrayOrdination;

    public int startingPotions = 1;
    public static int inventoryCapacity = 10;

    public int gold = 0;
    public int xp = 0;
    public int lvl = 1;

    public int[] inventoryItemID = new int[inventoryCapacity];
    public string[] inventoryItems = new string[inventoryCapacity];
    public string[] inventoryItemType = new string[inventoryCapacity];
    public string[] inventoryItemRarity = new string[inventoryCapacity];
    public GameObject[] position = new GameObject[inventoryCapacity];

    public Text notificationBox;
    public GameObject backpack;

    public void ShowInventory()
    {
        backpack.SetActive(true);
        arrayOrdination.InventorySortByName(inventoryItemID, inventoryItems, inventoryItemType, inventoryItemRarity);

    }

    public static void SaveInventory(string[] items, int xp, int lvl, int gold)
    {

    }

    public static void LoadInventory(string[] items, int xp, int lvl, int gold)
    {

    }

    public void RenewInventory()
    {
        
    }

    public void ItemPickUp(int itemID, string item, string itemType, string itemRarity) //Used when picking objects
    {
        int section = ArrayOrdination.CheckVoidArray(inventoryItemID);
        if (section >= 0)
        {
            notificationBox.text = "Item added to Inventory";
            inventoryItemID[section] = itemID;
            inventoryItems[section] = item;
            inventoryItemType[section] = itemType;
            inventoryItemRarity[section] = itemRarity;
        }
        else
        {
            notificationBox.text = "Inventory is full";
        }
    }

}
