using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAI : MonoBehaviour {

    public float moveSpeed = -5f;       //charge left
    public Rigidbody2D rb;
	
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	
	void FixedUpdate () {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);        //charges left
		
	}

}
