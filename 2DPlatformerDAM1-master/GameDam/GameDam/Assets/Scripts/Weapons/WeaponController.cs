using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    public int weaponDamage = 50;
    public float weaponAttackRate = 2;
    public string weaponType = "Sword";
    public bool weaponAtached = false;
    //var playerController
    private PlayerController playerControllerForWeapon;
    //var rotation 
    private bool rotated = false;
    public GameObject swordRotation;

    public GameObject playerRightHand;

    // Start is called before the first frame update
    void Start()
    {

        playerControllerForWeapon = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (weaponAtached)
        {
           transformWeapon();
        }

    }



    /**
* Calculates the total damage trougth the given weapon damage and player strength
* @param int weaponDamage
* @param int playerStrength
* */
    public static int totalDamage(int weaponDmg, int playerStrength)
    {
        int totalDamage = (int) weaponDmg * (playerStrength/100);
        return totalDamage;
    }

    public void transformWeapon()
    {
        if (!rotated)
        {


            
            GetComponent<Collider>().enabled = false;
       
            GameObject.Find("FinalSword").transform.Rotate(-30, 180, -90);
            rotated = true;
        }
        else { 
            transform.position = new Vector3(playerRightHand.transform.position.x + 0.42f, playerRightHand.transform.position.y + 0.02f, playerRightHand.transform.position.z + 0.8f);
            GameObject.Find("FinalSword").transform.Rotate(swordRotation.transform.rotation.x, swordRotation.transform.rotation.y, swordRotation.transform.rotation.z);

        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            weaponAtached = true;
            
         
        }
        



    }

}
