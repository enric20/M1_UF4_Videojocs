using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speedX;
    public float jumpSpeedY;
    public int gold = 0;
    //Array de declaració d'armes       //0        1       2        3          4        5           6                 7
    private string[] currentWeapon = { "None", "Pistol", "SMG" , "Shotgun", "Rifle", "Sniper", "Rocket_Launcher", "Minigun"};
    //Array de declaració d'armes disponibles
    private bool[] weaponUnlocked = { true, false, false, false, false, false, false, false};
    public int currentWeaponInt = 0; //Punter que indica l'arma usada actualment
    //HP
    int maxHealth = 100;
    int currentHealth;
    //Cooldowns
    int currentShootTime;
    int currentRechargeTime;
    public int shootCooldown;
    public int rechargeCooldown;
    bool reloading = false;
    bool bulletReady = true;

    //Ammo
    private int pistolBullets = 7; //ammunition (before)
    public int pistolMaxBullets = 7; //maxAmmunition (after)

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
    
    //public int playerAttackDamage = 10;

    //Diferents bales
    public GameObject leftBullet, rightBullet; //Pistol, SMG
    public GameObject leftPellet, rightPellet; //Shotgun
    public GameObject leftRifleBullet, rightRifleBullet; //Rifle
    public GameObject leftSniperBullet, rightSniperBullet; //Sniper

    private GameObject chestTag;

    //Acces a scripts
    private Chest chest;
    public HealthBar healthBar;
    public RealoadBar reloadBar;
    public ShowGold showGold;
    public WeaponsUI weaponsUI;

    //Posicions de dispar
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
        MovePlayer(speed); //Moviment del jugador
        Flip(); //Mirar la posicio actual del personatge
        WeaponController(); //Weapon cooldowns
        WeaponRecharge(); //Weapon cooldowns recharge

        /*if (!weaponUnlocked[1])
        {
            PlayerWeaponUnlock(1);
        }*/
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
            switch (currentWeapon[currentWeaponInt]) //Segons el tipus d'arma que tens executara un dispar diferent
            {
                case "Pistol":
                    Fire(leftBullet, rightBullet, pistolBullets, 1); //GameObject left shoot, right shoot, ammoType, currentWeaponInt
                    break;
                case "SMG":
                    Fire(leftBullet, rightBullet, smgBullets, 2);
                    break;
                case "Shotgun":
                    FirePellet(leftPellet, rightPellet); //Funcio per escopeta dispara 3 perdigons
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

    void MovePlayer(float playerSpeed) //Moviment del jugador
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

    void OnCollisionEnter2D(Collision2D other) //Saltar
    {
        if (other.gameObject.tag == "GROUND")
        {
            Jumping = false;
            anim.SetInteger("Status", 0);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision) //Recollir objectes
    {

        if (collision.gameObject.tag == "Coin")
        {
            PlayerAddGold(5);
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "Chest")
        {
            chestTag = GameObject.FindGameObjectWithTag("Chest");
            chest = chestTag.GetComponent<Chest>();
            //PlayerAddGold(50);
            PlayerAddGold(chest.chestCoins);
            
        }
        //Munició:

        else if (collision.gameObject.tag == "Lava")
        {
            PlayerTakeDamage(200);
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }

        else if (collision.gameObject.tag == "DeathZone")
        {
            Death();
        }

        else if (collision.gameObject.tag == "9mmAmmoBox")
        {
            nineMMAmmoCapacity = nineMMAmmoCapacity + 10;
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "shellsAmmoBox")
        {
            shellAmmoCapacity = shellAmmoCapacity + 5;
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "rifleAmmoBox")
        {
            rifleAmmoCapacity = rifleAmmoCapacity + 10;
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "sniperAmmoBox")
        {
            sniperAmmoCapacity = sniperAmmoCapacity + 3;
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "medikit") //Recollir vida
        {
            bool canGetHealth = true;
            if (currentHealth >= maxHealth)
            {
                canGetHealth = false;
            }

            if (canGetHealth)
            {
                if ((currentHealth+50) >= 100) //No pasara de 100 de vida
                {
                    currentHealth = 100;
                }
                else
                {
                    currentHealth = currentHealth + 50;
                }
                healthBar.SetMaxHealth(maxHealth);
                healthBar.SetHealth(currentHealth);
                Destroy(collision.gameObject);
            }
            
        }

        else if (collision.gameObject.tag == "CLEARSKY")
        {
            print("CLEARSKY");
            GameObject tag;
            BackgroundChange script;
            tag = GameObject.FindGameObjectWithTag("MainCamera");
            script = tag.GetComponent<BackgroundChange>();
            script.ChangeSky(1);
            //Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "SKYCITY")
        {
            print("SKYCITY");
            GameObject tag;
            BackgroundChange script;
            tag = GameObject.FindGameObjectWithTag("MainCamera");
            script = tag.GetComponent<BackgroundChange>();
            script.ChangeSky(2);
            //Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "NIGHTSKY")
        {
            print("NOBACKGROUND");
            GameObject tag;
            BackgroundChange script;
            tag = GameObject.FindGameObjectWithTag("MainCamera");
            script = tag.GetComponent<BackgroundChange>();
            script.ChangeSky(3); //0 Deletes background
            //Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == "NOBACKGROUND")
        {
            print("NOBACKGROUND");
            GameObject tag;
            BackgroundChange script;
            tag = GameObject.FindGameObjectWithTag("MainCamera");
            script = tag.GetComponent<BackgroundChange>();
            script.ChangeSky(0); //0 Deletes background
            //Destroy(collision.gameObject);
        }

        


        reloadBar.SetAmmoCapacity(nineMMAmmoCapacity, shellAmmoCapacity, rifleAmmoCapacity, sniperAmmoCapacity);
    }

    void Flip() //Girar el jugador segons la velocitat
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

    public void FireButton() //Disparar amb botons
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
            if (!facingRight) //Initialitza 3 bales
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

    public void PlayerTakeDamage(int damage) //S'activa a través dels scripts dels enemics
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameOver");
        Destroy(gameObject);

    }

    public void PlayerUseAmmo(int ammoUsed, int currentWeapon) //Funcio per actualitzar munició
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

    public void WeaponRecharge() //S'utilitza per a mirar les diferents variables i verifica si es pot o no recarregar, aquesta funcio s'executa en el update, ja que utilitza cooldowns
    {
        switch (currentWeapon[currentWeaponInt])
        {
            case "Pistol": //1 Recarrega Pistola
                if (currentRechargeTime < rechargeCooldown && reloading && pistolBullets < pistolMaxBullets && nineMMAmmoCapacity >= 1)
                {
                    if(currentRechargeTime == 1) //Activa el so de la recarrega en el moment que el cooldown es 1, per a que no s'executi varies vegades
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

            case "SMG": //2 Recarrega SMG
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

            case "Shotgun": //3 Recarrega Escopeta
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
            case "Rifle": //4 Recarrega Rifle
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
            case "Sniper": //5 Recarrega franctirador
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

    public void PlayerAmmoUpdate() //Segons nivell de WeaponRecharge, actualitza la munició actual, fa servir la funcio AmmoReloaderFunction per a que la municio actual i la capacitat de municio s'actualitzin correctament
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
        PlayerUseAmmo(0, currentWeaponInt); //Actualitza la municio en la UI
        reloadBar.SetAmmoCapacity(nineMMAmmoCapacity, shellAmmoCapacity, rifleAmmoCapacity, sniperAmmoCapacity); //Actualitza la capacitat de munició

    }

    private void PlayerAddGold(int newGold) //Funcio per afegir OR a la variable i mostrarla a la UI
    {
        gold = gold + newGold;
        showGold.SetGold(gold);
        showGold.GoldNotification(newGold);
    }

    public void WeaponController() //Mira el cooldown del dispar del arma, es podria eliminar el switch, pero d'aquesta forma permet afegir sons entre l'enfredament del arma i altres coses.
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

    public void WeaponSelector() //Canviar cooldowns de les armes, temps de recarrega, bales maximes... S'executa cada vegada que cambiem d'arma (utilitzem tecles 0 a 5)
    {
        switch (currentWeapon[currentWeaponInt]) //Canvia el cooldown general, ho he fet aixi per estalviar crear variables de més.
        {
            case "Pistol": //1
                shootCooldown = 70; //Temps entre dispar
                pistolMaxBullets = 7; //Bales maximes
                rechargeCooldown = 270; //Temps de recarrega
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
        reloading = false; //Canvia l'estat de la recarrega a false, si el jugador cambia d'arma mentres recarrega es cancela la recarrega
        currentRechargeTime = rechargeCooldown;
        currentShootTime = 0; //Canvia a 0 per a que hi hagi un CD al canviar d'arma
        reloadBar.SetMaxReloadCD(rechargeCooldown); //Actualitza UI barra recarrega
        reloadBar.SetReloadCD(rechargeCooldown); //Actualitza UI barra recarrega
        weaponsUI.SetCurrentWeaponUI(currentWeaponInt); //Cambia la UI per a establir l'arma actual
        weaponsUI.SetAvailableWeaponsUI(weaponUnlocked); //Pasar l'array per a que no mostri les armes que no estan disponibles

        rechargePistolAudio.Stop(); //Evita que l'audio es quedi trabat mentres es canvia d'arma
        reloadSMGAudio.Stop();
        reloadShotgunAudio.Stop();
        reloadRifleAudio.Stop();
        reloadSniperAudio.Stop();
    }

    private int AmmoReloaderFunction(int currentAmmo, int maxAmmo, int ammoCapacity, int weaponType) //La funcio definitiva per a actualitzar la municio actual i la capacitat de munició
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
       
        switch (weaponType) //Actualitza la munició actual del arma
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
        return ammoCapacity; //Retorna la capacitat de munició
    }

    public void PlayerWeaponUnlock(int unlockedWeapon) //Desbloqueja armes i les mostra en la interfície
    {
        weaponUnlocked[unlockedWeapon] = true;
        weaponsUI.SetAvailableWeaponsUI(weaponUnlocked); //Pasar l'array per a que miri les armes disponibles
    }

}