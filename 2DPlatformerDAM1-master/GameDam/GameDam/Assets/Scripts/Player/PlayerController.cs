using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variable cambio stat de arma
    public bool weaponStatement = false;


    public Animator animator;
    public WeaponController weaponController;
    private PlayerInventory playerInventory;
    private PlayerStadistics playerStadistics;
    public int playerID = 1;

    //Basic Stadistics
    public int startingHealthPoints = 100; public int startingStrength = 100 /*% Damage added to weapon (100 = base dmg)*/; 
    public int healthPoints; public int strength;
    //Magical Stadistics
    public int startingIntellect = 100; public int startingManaPoints = 100;
    public int intellect; public int manaPoints;
    //Defense Stadistics
    public int startingPhysicalDefense = 0; public int startingMagicalDefense = 0;
    public int physicalDefense = 0; public int magicalDefense = 0;
    //Agility Stadistics
    public int speed = 10; public int stealth = 20 /*% Reduction to enemy vision*/;
    public int startingStamina = 100;
    public int stamina = 100;
    public float attackRate = 2;
    private float nextAttackRate = 0;

    //PlayerMechanicsVariables
    public bool isAttacking = false; public bool stunned = false;
    private bool isJumping = false; public bool spotted = false;

    public GameObject meleeWeapon;
    public GameObject magicAttack;
    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        weaponController = GetComponent<WeaponController>();
        playerStadistics.RenewStadistics();
        playerInventory.RenewInventory();

        int attackDamage = WeaponController.totalDamage(weaponController.weaponDamage, strength);

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackRate)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //animator.SetTrigger("Attack");
                while (Time.time >= nextAttackRate) //This won't be necesary if I activate the function from animatior
                {
                    isAttacking = true;
                }
                isAttacking = false;
                nextAttackRate = Time.time + attackRate;

            }
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        bool meleeCollition = false;
        //comentado para evitar mensajes de error en corrección de colocar la espada
        /*if (other.gameObject.CompareTag("Enemy") && isAttacking)
        {
            meleeCollition = true;
            
        }

        else if (other.gameObject.CompareTag("Object"))
        {
            meleeCollition = true;
            Debug.Log("Object touched");
        }*/

        if (other.gameObject.CompareTag("Terrain"))
        {
            isJumping = false;
        }

        if (other.gameObject.CompareTag("Weapon"))
        {
            //weaponController.weaponAtached = true;
            weaponStatement = true;
            Debug.Log(weaponStatement);
            Debug.Log("Weapon touched");
        }
        meleeCollition = false;
        isAttacking = false;
    }

    private void OnTriggerEnter()
    {
        if (player.gameObject.CompareTag("Object"))
        {
            
        } 
    }
}
