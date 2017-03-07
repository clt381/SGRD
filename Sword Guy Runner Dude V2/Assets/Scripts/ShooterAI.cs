using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterAI : MonoBehaviour {

    public GameObject player;
    public GameObject projectile;
    public GameObject shootingPoint;

    public float distanceFromPlayer;
    public float maxRange = 10f;
    public float projectileTimer;
    public float bulletAheadDistance = 4f;      //so that different shooters can have different ahead distances

    private float projectileCoolDown = 2f;           //should hopefully fire once per second
    private float projectileSpeed = 1.5f;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	
	void FixedUpdate () {
        distanceFromPlayer = transform.position.x - player.transform.position.x;

        ShootPlayer();
	}

    void ShootPlayer()
    {
        projectileTimer += Time.deltaTime;          //projectile timer starts ticking up at game start; will fire at player as soon as player enters maxRange

        if (projectileTimer >= projectileCoolDown)
        {
            Vector2 projectileDirection = (player.transform.position + Vector3.right * bulletAheadDistance) - transform.position;
            //projectileDirection.Normalize();
            distanceFromPlayer = Mathf.Abs(transform.position.x - player.transform.position.x);     //Must add mathf.abs to get absolute value otherwise distancefromplayer will always be less than maxrange in negative

            if (distanceFromPlayer <= maxRange)
            {
                    //print("playerWithinRange");         //MOFOKING WORKS BABY
                    GameObject projectileClone;
                    projectileClone = Instantiate(projectile, shootingPoint.transform.position, shootingPoint.transform.rotation) as GameObject;        //instantiates the projectile at shooting point gameobject
                    projectileClone.GetComponent<Rigidbody2D>().velocity = projectileDirection * projectileSpeed;               

                    projectileTimer = 0;        //resets timer so shooter can't spam 
                
            }

        }
    }
}
