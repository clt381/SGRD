using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAI : MonoBehaviour {

    public float moveSpeed = -5f;       //charge left
    public Rigidbody2D rb;
    public GameObject monsterSpriteAnimator;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        monsterSpriteAnimator = GameObject.Find("MonsterSprite");
    }


    void FixedUpdate() {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);        //charges left

    }

    public void KillEnemy()
    {
        StartCoroutine("SpawnDeath");

    }
    IEnumerator SpawnDeath()
    {
        //put animation trigger here; waitforseconds corresponds to animation time
        monsterSpriteAnimator.GetComponent<Animator>().SetTrigger("MonsterDeath");          //currently not working; trigger works, but doesn't trigger from overlapcircle
        GetComponent<Collider2D>().enabled = false;     //so charger doesn't damage player during death animation
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        //destroy gameobject
    }
}
