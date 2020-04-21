using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayOrdination : MonoBehaviour
{

    public void InventorySortByName(int [] iItemId, string[] iItems, string[] iItemType, string[] iItemRarity)
    {
        int integer = 0;
        string str = "";

        for (int i = 0; i < iItemId.Length-1; i++)
        {
            for (int j = i+1; i < iItemId.Length; j++)
            {
                if (iItems[i].CompareTo(iItems[j]) > 0)
                {
                    integer = iItemId[i];
                    iItemId[i] = iItemId[j];
                    iItemId[j] = integer;

                    str = iItems[i];
                    iItems[i] = iItems[j];
                    iItems[j] = str;

                    str = iItemType[i];
                    iItemType[i] = iItemType[j];
                    iItemType[j] = str;

                    str = iItemRarity[i];
                    iItemRarity[i] = iItemRarity[j];
                    iItemRarity[j] = str;
                }
            }
        }
        
    }

    public static int CheckVoidArray(int[] iItemId) //Searches a void space in the inventory
    {
        for (int i = 0; i < iItemId.Length; i++)
        {
            if (iItemId[i] == 0) //If it's empty, return inventory section number
            {
                return i;
            }
        }
        return -1; 
    }

    

}
