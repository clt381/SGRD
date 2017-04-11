using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAI : MonoBehaviour {

    public float moveSpeed = -5f;       //charge left
    public Rigidbody2D rb;
    public GameObject monsterSpriteAnimator;
    public bool dying;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        //monsterSpriteAnimator = GameObject.Find("MonsterSprite");
    }


    void FixedUpdate() {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);        //charges left

    }

    public void KillEnemy()
    {
        if (!dying)
        {
            //StartCoroutine("SpawnDeath");
            dying = true;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Animator>().SetBool("MonsterDeath", true);
        }
    }

    public void DestroySprite()
    {
        Destroy(gameObject);
    }

    IEnumerator SpawnDeath()
    {
        dying = true;
        monsterSpriteAnimator.GetComponent<Animator>().SetBool("MonsterDeath", true);          //currently not working; trigger works, but doesn't trigger from overlapcircle
        GetComponent<Collider2D>().enabled = false;     //so charger doesn't damage player during death animation
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
        //destroy gameobject
        //dying = false;
    }
}
