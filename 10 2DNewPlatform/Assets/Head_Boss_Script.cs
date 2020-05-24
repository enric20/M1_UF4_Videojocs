using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Head_Boss_Script : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    public HealthBarEnemy healthBarEnemy;

    private int damageCooldown = 200;
    private int currentTime = 200;

    public int maxHP;
    private int currentHP;

    public float verticalSpeed;
    private float currentVerticalSpeed;

    private Rigidbody2D rb;

    private bool facingRight;

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

    Transform firePos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        turretBullets = turretMaxBullets;
        turretCurrentHP = turretMaxHP;
        healthBarEnemy.SetMaxHealth(turretMaxHP);
        healthBarEnemy.SetHealth(turretCurrentHP);

        firePos = transform.Find("firePosBoss");

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
            Flip();
        }
    }

    private void Fire(GameObject leftBullet, GameObject rightBullet)
    {
        if (!facingRight)
        {
            Instantiate(leftBullet, firePos.position, Quaternion.identity);
        }

        else
        {
            Instantiate(rightBullet, firePos.position, Quaternion.identity);
        }
    }

    public void UnmobileEnemyTakeDamage(int damage)
    {
        if (turret)
        {
            turretCurrentHP -= damage;
            healthBarEnemy.SetMaxHealth(turretMaxHP);
            healthBarEnemy.SetHealth(turretCurrentHP);
            if (turretCurrentHP <= 0)
            {
                Death();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;
    }

    public void Death()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("GameEnd");
        Destroy(gameObject);
    }
}
