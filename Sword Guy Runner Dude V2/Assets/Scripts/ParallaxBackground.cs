using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

    public float moveSpeedX;
    public float distanceToMoveY;
    public Player player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        moveSpeedX = 0.19f;           //0.2f is the player movement speed
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(moveSpeedX, 0, 0);
        
	}
}
