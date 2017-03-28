using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour {

    public GameObject player;
    public GameObject spawnPoint;
    public GameObject charger;
    public Rigidbody2D rb;

    private float moveSpeed = 11f;
    private float minRangeMoveSpeed = 12f;
    private float bossHealth = 200f;
    public float distanceFromPlayer;
    public float maxRange = 20f;
    public float minRange = 3f;         //should be the same as the player's attack radius
    public float spawnTimer;
    private float spawnCoolDown = 0.25f;

    public float thrust;                //give this a random value so that chargers are spawned with different addforces

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        
    }

    void FixedUpdate()
    {

        RunFromPlayer();
        LaunchAtPlayer();
        //print("bossHealth is:" + bossHealth);
    }

    void RunFromPlayer()
    {
        distanceFromPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
        if (distanceFromPlayer <= minRange)
        {
            rb.velocity = new Vector2(minRangeMoveSpeed, rb.velocity.y);            //if the player is within attack radius of the boss, boss matches player's speed
        }
        else if (distanceFromPlayer <= maxRange)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    void LaunchAtPlayer()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnCoolDown)
        {
            distanceFromPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);
            thrust = Random.Range(1f, 5f);             //not working
            if (distanceFromPlayer <= maxRange)
            {
                GameObject chargerClone;
                chargerClone = Instantiate(charger, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;        //instantiates the charger at spawnpoint gameobject
                chargerClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1f) * thrust, ForceMode2D.Impulse);          //make it so it does addforce randomly
                //CONSIDER increasing the gravity scale of the charger prefab so they drop faster
                spawnTimer = 0;
            }
        }
    }

    public void Damage(int amount)
    {
        bossHealth -= amount;
        if (bossHealth <= 0)
        {
            Destroy(gameObject);        //destroy this specifc instance (object attached to)
        }
    }


}
