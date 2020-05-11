using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedX;
    public float jumpSpeedY;
    public int gold = 0;

    private string[] currentWeapon = { "None", "Pistol", "SMG" , "Shotgun", "Rifle", "Grenade_Launcher", "Rocket_Launcher", "Minigun"};
    private bool[] weaponUnlocked = { true, true, false, true, false, false, false};
    public int currentWeaponInt = 0;

    int maxHealth = 100;
    int currentHealth;

    int currentShootTime;
    int currentRechargeTime;
    public int shootCooldown;
    public int rechargeCooldown;
    bool reloading = false;
    bool bulletReady = true;

    private int pistolBullets = 7; //ammunition (before)
    public int pistolMaxBullets = 7; //maxAmmunition (before)
    private int smgBullets = 0;
    private int smgMaxBullets = 30;
    private int shells = 5;
    public int maxShells = 5;
    public int nineMMAmmoCapacity = 30;
    public int shellAmmoCapacity = 14;
    
    public int playerAttackDamage = 10;

    public GameObject leftBullet, rightBullet; //Pistol, SMG, Rifle
    public GameObject leftPellet, rightPellet; //Shotgun

    private GameObject chestTag;

    private Chest chest;

    public HealthBar healthBar;
    public RealoadBar reloadBar;
    public ShowGold showGold;
    public WeaponsUI weaponsUI;

    Transform firePos;
    Transform firePosUp;
    Transform firePosDown;

    bool facingRight, Jumping;
    float speed;

    Animator anim;
    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        PlayerWeaponUnlock(1);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        reloadBar.SetMaxReloadCD(rechargeCooldown);
        Jumping = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        firePos = transform.Find("firePos");
        firePosUp = transform.Find("firePosUp");
        firePosDown = transform.Find("firePosDown");

        reloadBar.SetMaxAmmunition(pistolMaxBullets, smgMaxBullets, maxShells);
        reloadBar.SetAmmoCapacity(nineMMAmmoCapacity, shellAmmoCapacity);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer(speed);
        Flip();
        WeaponController(); //Weapon cooldowns
        WeaponRecharge(); //Weapon cooldowns recharge

        //left player movement

        if (Input.GetKeyDown(KeyCode.J))
        {
            speed = -speedX;
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            speed = 0;
        }

        //right player movement

        if (Input.GetKeyDown(KeyCode.L))
        {
            speed = speedX;
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            speed = 0;
        }

        //jump player movement

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (Jumping == false)
            {
                Jumping = true;
                rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
                anim.SetInteger("Status", 2);
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currentWeapon[currentWeaponInt])
            {
                case "Pistol":
                    Fire(leftBullet, rightBullet, pistolBullets, 1);
                    break;
                case "SMG":
                    Fire(leftBullet, rightBullet, pistolBullets, 2);
                    break;
                case "Shotgun":
                    FirePellet(leftPellet, rightPellet);
                    break;
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weaponUnlocked[1])
            {
                currentWeaponInt = 1;
                WeaponSelector();
            }
            
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weaponUnlocked[3])
            {
                currentWeaponInt = 3;
                WeaponSelector();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!reloading) //Aquí hi ha un bug de recarrega, es pot arreglar canviant d'arma, per lo vist al no veure municio per carregar es mante en "reloading = true"
            {
                reloading = true;
                currentRechargeTime = 0;
            }
        }

    }

    void MovePlayer(float playerSpeed)
    {
        if (playerSpeed < 0 && !Jumping || playerSpeed > 0 && !Jumping)
        {
            anim.SetInteger("Status", 1);
        }

        if (playerSpeed == 0 && !Jumping)
        {
            anim.SetInteger("Status", 0);
        }

        rb.velocity = new Vector3(speed, rb.velocity.y, 0);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "GROUND")
        {
            Jumping = false;
            anim.SetInteger("Status", 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Coin")
        {
            PlayerAddGold(5);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Chest")
        {
            chestTag = GameObject.FindGameObjectWithTag("Chest");
            chest = chestTag.GetComponent<Chest>();
            PlayerAddGold(chest.chestCoins);
        }
    }

    void Flip()
    {
        // code for change direction of the player
        if (speed < 0 && !facingRight || speed > 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;

        }
    }

    public void FireButton()
    {
        Fire(leftBullet, rightBullet, pistolBullets, 1);
    }

    public void Fire(GameObject leftGun, GameObject rightGun, int universalAmmo, int currentWeaponType)
    {
        if (bulletReady && universalAmmo > 0 && !reloading && currentWeapon[currentWeaponInt] == "Pistol")
        {
            if (!facingRight)
            {
                Instantiate(rightGun, firePos.position, Quaternion.identity);
            }

            if (facingRight)
            {
                Instantiate(leftGun, firePos.position, Quaternion.identity);
            }
            bulletReady = false;
            currentShootTime = 0;
            PlayerUseAmmo(1, currentWeaponType);
        }

    }

    public void FirePellet(GameObject leftPellet, GameObject rightPellet)
    {
        if (bulletReady && shells > 0 && !reloading && currentWeapon[currentWeaponInt] == "Shotgun")
        {
            
            if (!facingRight)
            {
                Instantiate(rightPellet, firePos.position, Quaternion.identity);
                Instantiate(rightPellet, firePosDown.position, Quaternion.identity);
                Instantiate(rightPellet, firePosUp.position, Quaternion.identity);
            }

            if (facingRight)
            {
                Instantiate(leftPellet, firePos.position, Quaternion.identity);
                Instantiate(leftPellet, firePosDown.position, Quaternion.identity);
                Instantiate(leftPellet, firePosUp.position, Quaternion.identity);
            }
            
           
            bulletReady = false;
            currentShootTime = 0;
            PlayerUseAmmo(1, 3);
        }
    }

    public void MoveLeft()
    {
        speed = -speedX;
    }

    public void MoveRight()
    {
        speed = speedX;
    }

    public void Jump()
    {
        if (Jumping == false)
        {
            Jumping = true;
            rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
            anim.SetInteger("Status", 2);
        }
    }

    public void Stop()
    {
        speed = 0;
    }

    public void PlayerTakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerUseAmmo(int ammoUsed, int currentWeapon)
    {
        switch (currentWeapon)
        {
            case 1:
                pistolBullets -= ammoUsed;
                reloadBar.SetAmmunition(pistolBullets, currentWeapon);
                break;
            case 2:
                smgBullets -= ammoUsed;
                reloadBar.SetAmmunition(smgBullets, currentWeapon);
                break;
            case 3:
                shells -= ammoUsed;
                reloadBar.SetAmmunition(shells, currentWeapon);
                break;
        }
        
       
    }

    public void WeaponRecharge()
    {
        switch (currentWeapon[currentWeaponInt])
        {
            case "Pistol": //1
                if (currentRechargeTime < rechargeCooldown && reloading && pistolBullets < pistolMaxBullets && nineMMAmmoCapacity >= 1)
                {
                    currentRechargeTime++; //Start recharge
                    reloadBar.SetReloadCD(currentRechargeTime);
                }

                else if (currentRechargeTime >= rechargeCooldown && reloading) //Finish recharge, load bullets
                {
                    PlayerAmmoUpdate();
                }

                break;

            case "SMG":

                break;

            case "Shotgun": //3
                if (currentRechargeTime < rechargeCooldown && reloading && shells < maxShells && shellAmmoCapacity >= 1)
                {
                    currentRechargeTime++;
                    reloadBar.SetReloadCD(currentRechargeTime);
                }

                else if (currentRechargeTime >= rechargeCooldown && reloading)
                {
                    PlayerAmmoUpdate();
                }
                break;
        }
        
    }

    public void PlayerAmmoUpdate() //2 nivell de WeaponRecharge
    {
        switch (currentWeapon[currentWeaponInt])
        {
            case "Pistol": //1

                nineMMAmmoCapacity = AmmoReloaderFunction(pistolBullets, pistolMaxBullets, nineMMAmmoCapacity, currentWeaponInt);
                break;

            case "SMG": //2
                nineMMAmmoCapacity = AmmoReloaderFunction(smgBullets, smgMaxBullets, nineMMAmmoCapacity, currentWeaponInt);
                break;

            case "Shotgun": //3
                shellAmmoCapacity = AmmoReloaderFunction(shells, maxShells, shellAmmoCapacity, currentWeaponInt);
                break;
        }
        reloading = false;
        PlayerUseAmmo(0, currentWeaponInt);
        reloadBar.SetAmmoCapacity(nineMMAmmoCapacity, shellAmmoCapacity);

    }

    private void PlayerAddGold(int newGold)
    {
        gold = gold + newGold;
        showGold.SetGold(gold);
        showGold.GoldNotification(newGold);
    }

    public void WeaponController()
    {
        switch (currentWeapon[currentWeaponInt])
        {
            case "Pistol": //1
               
                if (currentShootTime >= shootCooldown)
                {
                    bulletReady = true;
                }
                else
                {
                    currentShootTime++;
                }

                break;

            case "SMG": //2
                break;

            case "Shotgun": //3
                if (currentShootTime >= shootCooldown)
                {
                    bulletReady = true;
                }
                else
                {
                    currentShootTime++;
                }
                break;

            case "Rocket_Launcher":
                
                break;

            default:
               

               break;
        }

    }

    public void WeaponSelector()
    {
        switch (currentWeapon[currentWeaponInt])
        {
            case "Pistol": //1
                shootCooldown = 70; //70
                pistolMaxBullets = 7;
                rechargeCooldown = 300; //300
                break;

            case "SMG":
                shootCooldown = 20;
                smgMaxBullets = 30;
                rechargeCooldown = 400;
                break;

            case "Shotgun": //3
                shootCooldown = 120;
                maxShells = 5;
                rechargeCooldown = 700;
                break;
        }
        reloading = false;
        currentRechargeTime = rechargeCooldown;
        currentShootTime = 0; //Canvia a 0 per a que hi hagi un CD al canviar d'arma
        reloadBar.SetMaxReloadCD(rechargeCooldown);
        reloadBar.SetReloadCD(rechargeCooldown);
        weaponsUI.SetCurrentWeaponUI(currentWeaponInt); //Cambia la UI per a establir l'arma actual
        weaponsUI.SetAvailableWeaponsUI(weaponUnlocked); //Pasar l'array per a que no mostri les armes que no estan disponibles
    }

    private int AmmoReloaderFunction(int currentAmmo, int maxAmmo, int ammoCapacity, int weaponType)
    {
        if (maxAmmo > ammoCapacity)
        {
            if ((currentAmmo + ammoCapacity) > maxAmmo)
            {
                ammoCapacity = ammoCapacity - (maxAmmo - currentAmmo);
                currentAmmo = maxAmmo;

            }

            else
            {
                currentAmmo = ammoCapacity + currentAmmo;
                ammoCapacity = 0;
            }
        }

        else
        {
            ammoCapacity = ammoCapacity - (maxAmmo - currentAmmo);
            currentAmmo = maxAmmo;
        }
       
        switch (weaponType)
        {
            case 1:
                pistolBullets = currentAmmo;
                break;
            case 2:
                smgBullets = currentAmmo;
                break;
            case 3:
                shells = currentAmmo;
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
        reloadBar.SetAmmunition(currentAmmo, currentWeaponInt);
        return ammoCapacity;
    }

    private void PlayerWeaponUnlock(int unlockedWeapon)
    {
        weaponUnlocked[unlockedWeapon] = true;
        weaponsUI.SetAvailableWeaponsUI(weaponUnlocked); //Pasar l'array per a que miri les armes disponibles
    }
}