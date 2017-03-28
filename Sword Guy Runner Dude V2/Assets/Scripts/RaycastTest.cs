using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour {

    public LayerMask collisionMaskObstacle;

    public bool collidingWithGround = true;

    private GameObject lowerSpriteAnimator;

	void Start () {
        Physics2D.queriesStartInColliders = false;
        lowerSpriteAnimator = GameObject.Find("LowerSprite");
	}

	void Update () {
        //Player playerScript = GetComponent<Player>();
        CameraController cameraScript = GetComponent<CameraController>();
        Player thePlayer = GetComponent<Player>();
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up * -1, 100, collisionMaskObstacle);

        //print(hitInfo.distance);

        if (hitInfo.collider != null)           //if the raycast is hitting something...
        {
            //Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        }
        else      //if the raycast is hitting nothing
        {
            //Debug.DrawLine(transform.position, (transform.position + (transform.up * -1)) * 100, Color.green);
        }

        if (hitInfo.collider != null && hitInfo.distance <= 0.4f)      //if raycast is hitting something and hitdistance is less than 1 (not jumping)
        {
            //print(collidingWithGround);
            collidingWithGround = true;
            lowerSpriteAnimator.GetComponent<Animator>().SetBool("Jump", false);
        }
        else    //if raycast is hitting something and hitdistance is greater than contact distance (jumping)
        {
            //print(collidingWithGround);
            collidingWithGround = false;
            lowerSpriteAnimator.GetComponent<Animator>().SetBool("Jump", true);
        }
	}
}
