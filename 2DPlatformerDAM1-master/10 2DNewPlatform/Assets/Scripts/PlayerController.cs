using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedX;
    public float jumpSpeedY;
    public int gold = 0;

    private string[] currentWeapon = { "None", "Pistol", "SMG" , "Shotgun", "Rifle", "Sniper", "Rocket_Launcher", "Minigun"};
    private bool[] weaponUnlocked = { true, true, true, true, true, true, false, false};
    public int currentWeaponInt = 0;

    int maxHealth = 100;
    int currentHealth;

    int currentShootTime;
    int currentRechargeTime;
    public int shootCooldown;
    public int rechargeCooldown;
    bool reloading = false;
    bool bulletReady = true;

    //Ammo
    private int pistolBullets = 7; //ammunition (before)
    public int pistolMaxBullets = 7; //maxAmmunition (before)

    private int smgBullets = 20;
    private int smgMaxBullets = 20;

    private int shells = 5;
    public int maxShells = 5;

    private int rifleBullets = 30;
    public int rifleMaxBullets = 30;

    private int sniperBullets = 10;
    private int sniperMaxBullets = 10;

    //Ammo Capacity
    public int nineMMAmmoCapacity = 30;
    public int shellAmmoCapacity = 14;
    public int rifleAmmoCapacity = 40;
    public int sniperAmmoCapacity = 10;
    
    public int playerAttackDamage = 10;

    public GameObject leftBullet, rightBullet; //Pistol, SMG
    public GameObject leftPellet, rightPellet; //Shotgun
    public GameObject leftRifleBullet, rightRifleBullet; //Rifle
    public GameObject leftSniperBullet, rightSniperBullet; //Sniper

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

    //Pistol Audios
    public AudioSource firePistolAudio;
    public AudioSource rechargePistolAudio;

    //SMG Audios
    public AudioSource fireSMGAudio;
    public AudioSource reloadSMGAudio;
    //Shotgun Audios
    public AudioSource fireShotgunAudio;
    public AudioSource shotgunShellFall;
    public AudioSource reloadShotgunAudio;
    //Rifle Audios
    public AudioSource fireRifleAudio;
    public AudioSource reloadRifleAudio;
    //Sniper Audio
    public AudioSource fireSniperAudio;
    public AudioSource reloadSniperAudio;

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

        reloadBar.SetMaxAmmunition(pistolMaxBullets, smgMaxBullets, maxShells, rifleMaxBullets, sniperMaxBullets);
        reloadBar.SetAmmoCapacity(nineMMAmmoCapacity, shellAmmoCapacity, rifleAmmoCapacity, sniperAmmoCapacity);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer(speed);
        Flip();
        WeaponController(); //Weapon cooldowns
        WeaponRecharge(); //Weapon cooldowns recharge

        //left player movement

        if (Input.GetKeyDown(KeyCode.A))
        {
            speed = -speedX;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            speed = 0;
        }

        //right player movement

        if (Input.GetKeyDown(KeyCode.D))
        {
            speed = speedX;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            speed = 0;
        }

        //jump player movement

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Jumping == false)
            {
                Jumping = true;
                rb.AddForce(new Vector2(rb.velocity.x, jumpSpeedY));
                anim.SetInteger("Status", 2);
            }

        }

        if (Input.GetKey(KeyCode.Space))
        {
            //reloading = false; //Posa "false" per evitar bug pero buguejar so
            //reloadBar.SetReloadCD(rechargeCooldown);
            switch (currentWeapon[currentWeaponInt])
            {
                case "Pistol":
                    Fire(leftBullet, rightBullet, pistolBullets, 1); //GameObject left shoot, right shoot, ammoType, currentWeaponInt
                    break;
                case "SMG":
                    Fire(leftBullet, rightBullet, smgBullets, 2);
                    break;
                case "Shotgun":
                    FirePellet(leftPellet, rightPellet);
                    break;
                case "Rifle":
                    Fire(leftRifleBullet, rightRifleBullet, rifleBullets, 4);
                    break;
                case "Sniper":
                    Fire(leftSniperBullet, rightSniperBullet, sniperBullets, 5);
                    break;
            }

        }

        //if (Input.GetKeyUp(KeyCode.Space))

        if (Input.GetKeyDown(KeyCode.Alpha1)) //Choose Pistol
        {
            if (weaponUnlocked[1])
            {
                currentWeaponInt = 1;
                WeaponSelector();
            }
            
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2)) //Choose SMG
        {
            if (weaponUnlocked[2])
            {
                currentWeaponInt = 2;
                WeaponSelector();
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3)) //Choose Shotgun
        {
            if (weaponUnlocked[3])
            {
                currentWeaponInt = 3;
                WeaponSelector();
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))  //Choose Rifle
        {
            if (weaponUnlocked[4])
            {
                currentWeaponInt = 4;
                WeaponSelector();
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5)) //Choose Sniper
        {
            if (weaponUnlocked[5])
            {
                currentWeaponInt = 5;
                WeaponSelector();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        { //Si la municio esta a full no hauria de activarse
            switch (currentWeapon[currentWeaponInt])
            {
                case "Pistol":
                    if (pistolBullets >= pistolMaxBullets && nineMMAmmoCapacity >= 1)
                    {
                        reloading = false;
                    }
                    else if (!reloading)
                    {
                        reloading = true;
                        currentRechargeTime = 0;
                    }
                    break;
                case "SMG":
                    if (smgBullets >= smgMaxBullets)
                    {
                        reloading = false;
                    }
                    else if (!reloading && nineMMAmmoCapacity >= 1)
                    {
                        reloading = true;
                        currentRechargeTime = 0;
                    }
                    break;
                case "Shotgun":
                    if (shells >= maxShells)
                    {
                        reloading = false;
                    }
                    else if (!reloading && shellAmmoCapacity >= 1)
                    {
                        reloading = true;
                        currentRechargeTime = 0;
                    }
                    break;
                case "Rifle":
                    if (rifleBullets >= rifleMaxBullets)
                    {
                        reloading = false;
                    }
                    else if (!reloading && rifleAmmoCapacity >= 1)
                    {
                        reloading = true;
                        currentRechargeTime = 0;
                    }
                    break;
                case "Sniper":
                    if (sniperBullets >= sniperMaxBullets)
                    {
                        reloading = false;
                    }
                    else if (!reloading && sniperAmmoCapacity >= 1)
                    {
                        reloading = true;
                        currentRechargeTime = 0;
                    }
                    break;
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

    public void Fire(GameObject leftGun, GameObject rightGun, int universalAmmo, int currentWeaponType) //Fire unilateral bullets
    {
        if (bulletReady && universalAmmo > 0 && !reloading && currentWeapon[currentWeaponInt] == "Pistol" /*|| currentWeapon[currentWeaponInt] == "SMG"*/)
        {
            //firePistolAudio.Stop();
            firePistolAudio.Play();
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
            PlayerUseAmmo(1, currentWeaponType); //Ammo used, currentWeaponInt Pistol
        }

        else if (bulletReady && universalAmmo > 0 && !reloading && currentWeapon[currentWeaponInt] == "SMG")
        {
            fireSMGAudio.Play();
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
            PlayerUseAmmo(1, currentWeaponType); //Ammo used, currentWeaponInt SMG
        }

        else if (bulletReady && universalAmmo > 0 && !reloading && currentWeapon[currentWeaponInt] == "Rifle")
        {
            fireRifleAudio.Play();
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
            PlayerUseAmmo(1, currentWeaponType); //Ammo used, currentWeaponInt Rifle
        }

        else if (bulletReady && universalAmmo > 0 && !reloading && currentWeapon[currentWeaponInt] == "Sniper")
        {
            fireSniperAudio.Play();
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
            PlayerUseAmmo(1, currentWeaponType); //Ammo used, currentWeaponInt Sniper
        }

    }

    public void FirePellet(GameObject leftPellet, GameObject rightPellet) //Fire pellets
    {
        if (bulletReady && shells > 0 && !reloading && currentWeapon[currentWeaponInt] == "Shotgun")
        {
            fireShotgunAudio.Play();
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
            PlayerUseAmmo(1, 3); //Ammo used, currentWeaponInt
            shotgunShellFall.Play();
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
            case 4:
                rifleBullets -= ammoUsed;
                reloadBar.SetAmmunition(rifleBullets, currentWeapon);
                break;
            case 5:
                sniperBullets -= ammoUsed;
                reloadBar.SetAmmunition(sniperBullets, currentWeapon);
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
                    if(currentRechargeTime == 1)
                    {
                        rechargePistolAudio.Play();
                    }
                    currentRechargeTime++; //Start recharge
                    reloadBar.SetReloadCD(currentRechargeTime);
                }

                else if (currentRechargeTime >= rechargeCooldown && reloading) //Finish recharge, load bullets
                {
                    
                    PlayerAmmoUpdate();
                }

                break;

            case "SMG":
                if (currentRechargeTime < rechargeCooldown && reloading && smgBullets < smgMaxBullets && nineMMAmmoCapacity >= 1)
                {
                    if (currentRechargeTime == 1)
                    {
                        reloadSMGAudio.Play();
                    }
                    currentRechargeTime++;
                    reloadBar.SetReloadCD(currentRechargeTime);
                }
                else if (currentRechargeTime >= rechargeCooldown && reloading)
                {
                    
                    PlayerAmmoUpdate();
                }
                break;

            case "Shotgun": //3
                if (currentRechargeTime < rechargeCooldown && reloading && shells < maxShells && shellAmmoCapacity >= 1)
                {
                    if (currentRechargeTime == 1)
                    {
                        reloadShotgunAudio.Play();
                    }
                    currentRechargeTime++;
                    reloadBar.SetReloadCD(currentRechargeTime);
                }

                else if (currentRechargeTime >= rechargeCooldown && reloading)
                {
                   
                    PlayerAmmoUpdate();
                }
                break;
            case "Rifle": //3
                if (currentRechargeTime < rechargeCooldown && reloading && rifleBullets < rifleMaxBullets && rifleAmmoCapacity >= 1)
                {
                    if (currentRechargeTime == 1)
                    {
                        reloadRifleAudio.Play();
                    }
                    currentRechargeTime++;
                    reloadBar.SetReloadCD(currentRechargeTime);
                }

                else if (currentRechargeTime >= rechargeCooldown && reloading)
                {
                    
                    PlayerAmmoUpdate();
                }
                break;
            case "Sniper": //3
                if (currentRechargeTime < rechargeCooldown && reloading && sniperBullets < sniperMaxBullets && sniperAmmoCapacity >= 1)
                {
                    if (currentRechargeTime == 1)
                    {
                        reloadSniperAudio.Play();
                    }
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
                nineMMAmmoCapacity = AmmoReloaderFunction(pistolBullets, pistolMaxBullets, nineMMAmmoCapacity, currentWeaponInt); //currentBullets, maxBullets, ammoCapacity, currentWeaponInt
                break;

            case "SMG": //2
                nineMMAmmoCapacity = AmmoReloaderFunction(smgBullets, smgMaxBullets, nineMMAmmoCapacity, currentWeaponInt);
                break;

            case "Shotgun": //3
                shellAmmoCapacity = AmmoReloaderFunction(shells, maxShells, shellAmmoCapacity, currentWeaponInt);
                break;
            case "Rifle":
                rifleAmmoCapacity = AmmoReloaderFunction(rifleBullets, rifleMaxBullets, rifleAmmoCapacity, currentWeaponInt);
                break;
            case "Sniper":
                sniperAmmoCapacity = AmmoReloaderFunction(sniperBullets, sniperMaxBullets, sniperAmmoCapacity, currentWeaponInt);
                break;
        }
        reloading = false;
        PlayerUseAmmo(0, currentWeaponInt);
        reloadBar.SetAmmoCapacity(nineMMAmmoCapacity, shellAmmoCapacity, rifleAmmoCapacity, sniperAmmoCapacity);

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
                if (currentShootTime >= shootCooldown)
                {
                    bulletReady = true;
                }
                else
                {
                    currentShootTime++;
                }
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
            case "Rifle": //3
                if (currentShootTime >= shootCooldown)
                {
                    bulletReady = true;
                }
                else
                {
                    currentShootTime++;
                }
                break;
            case "Sniper": //3
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

    public void WeaponSelector() //Canviar coldowns de les armes, temps de recarrega, bales maximes...
    {
        switch (currentWeapon[currentWeaponInt])
        {
            case "Pistol": //1
                shootCooldown = 70; //70
                pistolMaxBullets = 7;
                rechargeCooldown = 270; //300
                break;

            case "SMG":
                shootCooldown = 20;
                smgMaxBullets = 20;
                rechargeCooldown = 400;
                break;

            case "Shotgun": //3
                shootCooldown = 160;
                maxShells = 5;
                rechargeCooldown = 450;
                break;
            case "Rifle": //4
                shootCooldown = 30;
                rifleMaxBullets = 30;
                rechargeCooldown = 250;
                break;
            case "Sniper": //5
                shootCooldown = 180;
                sniperMaxBullets = 10;
                rechargeCooldown = 350;
                break;
        }
        reloading = false;
        currentRechargeTime = rechargeCooldown;
        currentShootTime = 0; //Canvia a 0 per a que hi hagi un CD al canviar d'arma
        reloadBar.SetMaxReloadCD(rechargeCooldown);
        reloadBar.SetReloadCD(rechargeCooldown);
        weaponsUI.SetCurrentWeaponUI(currentWeaponInt); //Cambia la UI per a establir l'arma actual
        weaponsUI.SetAvailableWeaponsUI(weaponUnlocked); //Pasar l'array per a que no mostri les armes que no estan disponibles

        rechargePistolAudio.Stop();
        reloadSMGAudio.Stop();
        reloadShotgunAudio.Stop();
        reloadRifleAudio.Stop();
        reloadSniperAudio.Stop();
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
                rifleBullets = currentAmmo;
                break;
            case 5:
                sniperBullets = currentAmmo;
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