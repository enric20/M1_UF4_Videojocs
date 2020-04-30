using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speedX;

    bool facingRight;

    int maxHealth = 100;
    int currentHealth;

    public int enemyAttackDamage = 20;

    public GameObject leftBullet, rightBullet;
    public HealthBar healthBar;
    //private GameObject enemyHealthBar;


    Transform firePos;

    int currentTime;
    public int shootCooldown = 200;

    Rigidbody2D rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetInteger("Status", 1);
        firePos = transform.Find("firePos");
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(speedX, rb.velocity.y, 0);
        currentTime++;

        if (currentTime >= shootCooldown)
        {
            Fire();
            currentTime = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "WALL")
        {
            speedX = -speedX;
            Flip();
        }

    }

    void Flip()
    {
        // code for change direction of the enemy
        if (speedX < 0 && !facingRight || speedX > 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;
        }
        
    }

    void Fire()
    {
        if (!facingRight)
        {
            Instantiate(rightBullet, firePos.position, Quaternion.identity);
        }

        if (facingRight)
        {
            Instantiate(leftBullet, firePos.position, Quaternion.identity);
        }

    }

    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}