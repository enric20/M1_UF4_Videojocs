using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowGold : MonoBehaviour
{
    public Text goldUI;
    public Text goldNotification;
    private int showGoldUICD;
    private int goldBetweenCD;
    bool goldUITrigger = false;

    void Update()
    {
        if (showGoldUICD >= 400)
        {
            goldUITrigger = false;
            goldNotification.text = "";
            goldBetweenCD = 0;
            
        }

        else if (goldUITrigger)
        {
            showGoldUICD++;
        }
    }

    public void SetGold(int gold)
    {
       goldUI.text = "Gold: " + gold;
    }

    public void GoldNotification(int addGold)
    {
        goldBetweenCD += addGold;
        goldNotification.text = "+" + goldBetweenCD;
        goldUITrigger = true;
        showGoldUICD = 0;
    }
}
