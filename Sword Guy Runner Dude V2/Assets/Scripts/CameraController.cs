using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Player thePlayer;
    public RaycastTest raycastTest;
    public Vector3 lastPlayerPosition;     //store position of the player

    public float distanceToMoveX;
    public float distanceToMoveY;

    public float smoothTime = 0.3f;
    private Vector3 smoothVelocity = Vector3.zero;

    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        raycastTest = FindObjectOfType<RaycastTest>();
        lastPlayerPosition = thePlayer.transform.position;
    }

    void Update()
    {
        Vector3 playerPosition = thePlayer.transform.TransformPoint(new Vector3(15, 3, -10));
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref smoothVelocity, smoothTime);

        //distanceToMoveX = thePlayer.transform.position.x - lastPlayerPosition.x;
        //distanceToMoveY = distanceToMoveY = thePlayer.transform.position.y - lastPlayerPosition.y;

        //transform.position = new Vector3(transform.position.x + distanceToMoveX, transform.position.y, transform.position.z);
        ////transform.position = new Vector3(transform.position.x, transform.position.y + distanceToMoveY, transform.position.z);

        //if (raycastTest.collidingWithGround)
        //{
        //    //transform.position = new Vector3(transform.position.x, transform.position.y + distanceToMoveY, transform.position.z);
        //    transform.position = new Vector3(transform.position.x, (thePlayer.transform.position.y + 3), transform.position.z);       //resets to player position y when colliding with ground

        //}
        //if (raycastTest.collidingWithGround == false)
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //}
        

        //lastPlayerPosition = thePlayer.transform.position;
        
    }

}
