using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCrtl : MonoBehaviour
{
    public Vector2 speed;

    Rigidbody2D rb;

    private int playerAttackDamage;
    private int enemyAttackDamage;

    public bool canDamageEnemy = false;

    public int bulletDamage = 20;
    public int penetration = 0; //Pot penetrar 1 objecte
    private int bulletPenetratedStructures = 0;

    // Start is called before the first frame update
    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("unmobileEnemy"))
        {
            if (canDamageEnemy)
            {
                UnmobileEnemy unmobileEnemy;
                GameObject unmobileEnemyGO;
                unmobileEnemyGO = GameObject.FindGameObjectWithTag("unmobileEnemy");
                unmobileEnemy = unmobileEnemyGO.GetComponent<UnmobileEnemy>();
                Destroy(gameObject);
                unmobileEnemy.UnmobileEnemyTakeDamage(bulletDamage);
            }
        }

        else if (collision.gameObject.CompareTag("mobileEnemy"))
        {
            if (canDamageEnemy)
            {
                EnemyController enemyController;
                GameObject mobileEnemy;
                mobileEnemy = GameObject.FindGameObjectWithTag("mobileEnemy");
                enemyController = mobileEnemy.GetComponent<EnemyController>();
                Destroy(gameObject);
                enemyController.EnemyTakeDamage(bulletDamage);
            }
        }

        else if (collision.gameObject.CompareTag("boss"))
        {
            if (canDamageEnemy)
            {
                Head_Boss_Script head_Boss_Script;
                GameObject boss;
                boss = GameObject.FindGameObjectWithTag("boss");
                head_Boss_Script = boss.GetComponent<Head_Boss_Script>();
                Destroy(gameObject);
                head_Boss_Script.UnmobileEnemyTakeDamage(bulletDamage);
            }
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController;
            GameObject player;
            player = GameObject.FindGameObjectWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
            Destroy(gameObject);
            playerController.PlayerTakeDamage(bulletDamage);
        }

       else if (collision.gameObject.CompareTag("bullet"))
       {
            if (bulletPenetratedStructures >= penetration)
            {
                Destroy(gameObject);
            }
            else
            {
                bulletPenetratedStructures++;
                Destroy(collision.gameObject);
                //OnCollisionEnter2D(collision);
            }
       }

       else
       {
            Destroy(gameObject);
       }
    }
}
