using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private string[] chestRarity = {"Common", "Uncommon", "Rare", "Legendary"};
    public int chestRarityPointer = 0;
    public int chestCoins = 25;
    public int weaponUnlock = 0;
    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
        switch (chestRarity[chestRarityPointer])
        {
            case "Common":

                break;
            case "Uncommon":

                break;
            case "Rare":

                break;
            case "Legendary":
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Opening Chest");
            anim.SetInteger("Baul", 1);
            UnlockWeapon();
        }
    }

    private void UnlockWeapon()
    {
        GameObject tag;
        PlayerController script;
        tag = GameObject.FindGameObjectWithTag("Player");
        script = tag.GetComponent<PlayerController>();
        script.PlayerWeaponUnlock(weaponUnlock);
    }
}
