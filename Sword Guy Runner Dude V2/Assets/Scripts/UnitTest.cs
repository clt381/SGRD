using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest : MonoBehaviour {

    public GameObject startPoint;
    public GameObject endPoint;

    public float distance;
    public float locationTimer = 0f;
    public float timer = 0f;
    public float speed;
    public float startBeat = 627.8356f;

	void Start () {
        for (int i = 0; i < 35; i++)
        {
            startBeat += 3.77547f;
                //print(startBeat);

        }
        //print(startBeat);
	}
	
	
	void Update () {
        locationTimer += Time.deltaTime;
        //print(locationTimer);

        distance = endPoint.transform.position.x - startPoint.transform.position.x;
        if (transform.position.x > startPoint.transform.position.x && transform.position.x < endPoint.transform.position.x)
        {
            timer += Time.deltaTime;
        }

        if (transform.position.x >= endPoint.transform.position.x)
        {
            speed = distance / timer;
            //print("time =" + timer);
            //print("distance =" + distance);
            //print("speed =" + speed);
            //print(timer + " seconds to travel " + distance + " units at " + playerScript.autoMoveSpeed + " speed");
            //print("speed is " + speed);
        }

	}
}
