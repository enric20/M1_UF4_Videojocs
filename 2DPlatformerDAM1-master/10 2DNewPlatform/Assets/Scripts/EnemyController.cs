using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speedX;

    bool facingRight;

    public GameObject leftBullet, rightBullet;

    Transform firePos;

    float targetTime;

    Rigidbody2D rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        targetTime = 60;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetInteger("Status", 1);
        firePos = transform.Find("firePos");
        Fire();

    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy(speedX);

        /*targetTime -= Time.deltaTime;
        if (targetTime >= 0.0f)
        {
            Fire();
        }
       */
    }

    void MoveEnemy(float enemySpeed)
    {
        rb.velocity = new Vector3(enemySpeed, rb.velocity.y, 0);
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
            Fire();
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

}