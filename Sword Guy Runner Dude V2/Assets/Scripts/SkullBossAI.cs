using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBossAI : MonoBehaviour {

    public GameObject player;
    public GameObject shootingPoint;
    public GameObject fireBall;
    public GameObject maxRange;

    public float fireBallSpeed = 2f;
    public float coolDownTime = 0.5f;
    public float timer;
    public float fireBallAheadDistance = 4f;
    public float moveSpeed = 0.2f;      //0.2 to 0.25 is the sweet spot
    public float health = 100f;
	
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	
	void Update () {
        if (player.transform.position.x >= maxRange.transform.position.x)
        {
            gameObject.transform.Translate(moveSpeed, 0, 0);
        }
        FireBallAtPlayer();

        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time*3, 3) - 123.8255f, transform.position.z);
    }

    void FireBallAtPlayer()
    {
        timer += Time.deltaTime;
        if (timer >= coolDownTime)
        {
            Vector2 fireBallDirection = (player.transform.position + Vector3.right * fireBallAheadDistance) - transform.position;
            if (player.transform.position.x >= maxRange.transform.position.x)
            {
                GameObject fireBallClone;
                fireBallClone = Instantiate(fireBall, shootingPoint.transform.position, shootingPoint.transform.rotation) as GameObject;
                fireBallClone.GetComponent<Rigidbody2D>().velocity = fireBallDirection * fireBallSpeed;
                timer = 0;
            }
        }
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
