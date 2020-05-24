using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnmobileEnemy : MonoBehaviour
{

    private GameObject player;
    private PlayerController playerController;
    private int damageCooldown = 200;
    private int currentTime = 200;

    public int maxHP;
    private int currentHP;

    public float verticalSpeed;
    private float currentVerticalSpeed;

    private Rigidbody2D rb;

    public bool turret = false; //Variables for turret enemies type
    public int turretMaxHP;
    private int turretCurrentHP;

    public int turretMaxBullets;
    private int turretBullets;

    public int turretReloadCooldown;
    private int currentTurretReload;

    public int turretShootCooldown;
    private int currentTurretCooldown;

    public GameObject leftBullet;
    public GameObject rightBullet;

    Transform leftFirePos;
    Transform rightFirePos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        turretBullets = turretMaxBullets;
        turretCurrentHP = turretMaxHP;

        leftFirePos = transform.Find("leftFirePos");
        rightFirePos = transform.Find("rightFirePos");
        currentTurretCooldown = 0;
        currentTurretReload = 0;
    }

    private void Update()
    {
        rb.velocity = new Vector2(0, currentVerticalSpeed);
        
        if (currentTime < damageCooldown)
        {
            currentTime++;
        }

        if (turret)
        {
            TurretFunction();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (currentTime >= damageCooldown)
            {
                playerController.PlayerTakeDamage(50);
                currentTime = 0;
            }

            if (currentVerticalSpeed < 0)
            {
                currentVerticalSpeed = verticalSpeed;
            }

            else if (currentVerticalSpeed > 0)
            {
                currentVerticalSpeed = -verticalSpeed;
            }
        }

        else if (other.gameObject.tag == "WALL")
        {
            currentVerticalSpeed = -verticalSpeed;
        }

        else if (other.gameObject.tag == "GROUND")
        {
            currentVerticalSpeed = verticalSpeed;
        }

    }

    private void TurretFunction() //Nomes utilitzada en la torreta, disparara si te bales i cd, si no te bales recarrega
    {
        if (currentTurretCooldown < turretShootCooldown)
        {
            currentTurretCooldown++;
        }

        else if (currentTurretCooldown >= turretShootCooldown && turretBullets > 0)
        {
            Fire(leftBullet, rightBullet);
            turretBullets--;
            currentTurretCooldown = 0;
        }

        else if (turretBullets <= 0)
        {
            ReloadFunction();
        }
    }

    private void ReloadFunction() //Recarrega arma
    { 
        if (currentTurretReload < turretReloadCooldown)
        {
            currentTurretReload++;
        }
        else if (currentTurretReload >= turretReloadCooldown)
        {
            currentTurretReload = 0;
            turretBullets = turretMaxBullets;
        }
    }

    private void Fire(GameObject leftBullet, GameObject rightBullet)
    {
        Instantiate(leftBullet, leftFirePos.position, Quaternion.identity);
        Instantiate(rightBullet, rightFirePos.position, Quaternion.identity);
    }

    public void UnmobileEnemyTakeDamage(int damage)
    {
        if (turret)
        {
            turretCurrentHP -= damage;
            //healthBarEnemy.SetHealth(currentHealth);
            if (turretCurrentHP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
