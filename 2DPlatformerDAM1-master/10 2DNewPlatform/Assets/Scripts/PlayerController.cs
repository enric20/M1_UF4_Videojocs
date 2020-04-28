using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedX;
    public float jumpSpeedY;

    int maxHealth = 100;
    int currentHealth;
    
    public GameObject leftBullet, rightBullet;
    public HealthBar HealthBar;

    Transform firePos;

    bool facingRight, Jumping;
    float speed;

    Animator anim;
    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.SetMaxHealth(maxHealth);
        Jumping = false;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        firePos = transform.Find("firePos");
    }

    // Update is called once per frame
    void Update()
    {
        // player movement

        MovePlayer(speed);

        Flip();


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

        if (Input.GetKeyUp(KeyCode.Z))
        {
            PlayerTakeDamage(10);
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
            Fire();
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
            Debug.Log("hola");
            Destroy(collision.gameObject);
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

    public void Fire()
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
        HealthBar.SetHealth(currentHealth);
    }

}



