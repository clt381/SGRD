using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Player thePlayer;
    public Transform playerPosition;
    public RaycastTest raycastTest;
    public Vector3 lastPlayerPosition;     //store position of the player
    

    public float zoomSpeed = 1.0f;
    public float shiftSpeed = 1.0f;
    public Vector3 shiftOffSet = new Vector3(0,0,0);
    public float defaultCameraSize = 6.396629f;
    public float skullTempleZoom = 10f;
    public float platformJumpingZoom = 10f;
    bool zooming;
    bool shifting;

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

    public void Zoom(float firstPos, float lastPos)
    {
        //Camera.main.orthographicSize = defaultCameraSize;
        //if (playerPosition.position.x > platformJumpingTrigger1Start.transform.position.x && playerPosition.position.x < platformJumpingTrigger1End.transform.position.x)
        //{
        //    Camera.main.orthographicSize = platformJumpingZoom;
        //}
        if (!zooming)
        {
            StartCoroutine(Zooming(firstPos, lastPos));
        }
    }

    IEnumerator Zooming(float firstPos, float lastPos)
    {
        zooming = true;
        float percentage = 0.0f;
        while (percentage < 1.0f)
        {
            //Debug.Log(percentage + " " + Camera.main.orthographicSize);
            Camera.main.orthographicSize = Mathf.Lerp(firstPos, lastPos, percentage);
            percentage += zoomSpeed * Time.deltaTime;//bigger numbers means faster zoom
            percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);
            yield return null; 
        }
        zooming = false;
    }

    public void Shift(Vector3 firstPos, Vector3 lastPos)
    {
        if (!shifting)
        {
            StartCoroutine(Shifting(firstPos, lastPos));
        }
    }

    IEnumerator Shifting(Vector3 firstPos, Vector3 lastPos)
    {
        shifting = true;
        float percentage = 0.0f;
        while (percentage < 1.0f)
        {
            shiftOffSet = Vector3.Lerp(firstPos, lastPos, percentage);
            percentage += shiftSpeed * Time.deltaTime;
            percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);
            yield return null;
        }
        shifting = false;
    }

    void Update()
    {
        Vector3 playerPosition = thePlayer.transform.TransformPoint(new Vector3(11, 1f, -10));
        transform.position = Vector3.SmoothDamp(transform.position, playerPosition + shiftOffSet, ref smoothVelocity, smoothTime);
        //Zoom();

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
