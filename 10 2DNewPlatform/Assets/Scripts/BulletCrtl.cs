using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCrtl : MonoBehaviour
{
    public Vector2 speed;

    Rigidbody2D rb;
    private EnemyController enemyController;
    private PlayerController playerController;

    private int playerAttackDamage;
    private int enemyAttackDamage;

    private GameObject enemy;
    private GameObject player;

    public int bulletDamage = 20;
    public int penetration = 1; //Pot penetrar 1 objecte
    private int bulletPentratedStructures = 0;

    // Start is called before the first frame update
    void Awake()
    {
        enemy = GameObject.FindGameObjectWithTag("enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        enemyController = enemy.GetComponent<EnemyController>();
        playerController = player.GetComponent<PlayerController>();

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
        if (collision.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject);
            enemyController.EnemyTakeDamage(bulletDamage);
            //Destroy(collision.gameObject);

        }

        else if (collision.gameObject.CompareTag("Player"))
        {

            Destroy(gameObject);
            playerController.PlayerTakeDamage(bulletDamage);

        }

        else
        {
            if (bulletPentratedStructures >= penetration)
            {
                Destroy(gameObject);
            }
            else
            {
                bulletPentratedStructures++;
                OnCollisionEnter2D(collision);
            }
            
        }
    }
}
