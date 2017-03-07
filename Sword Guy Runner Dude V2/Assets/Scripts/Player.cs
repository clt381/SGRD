using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (Controller2D))]
[RequireComponent (typeof (RaycastController))]
public class Player : MonoBehaviour {

    

    public float jumpHeight = 5f;           //units covered by jump; can be manipulated to give the impression of gravity
    private float timeToJumpApex = 0.25f;     //time in seconds to jump apex; 0.25 is an optimalish number for the current platform distance
    float accelerationTimeAirborne = 0.2f;      //to make player decelerate instead of stop completely when getkeydown is no longer true
    float accelerationTimeGrounded = 0.1f;
    float moveSpeed = 6f;
    
    float gravity;          //can set directly to -20 but a little bit unintuitive
    float jumpVelocity;     //can set directly to 8 but a little bit unintuitive
    Vector3 velocity;
    float velocityXSmoothing;

    private float autoMoveSpeed = 2f;    //automatic move right speed

    //to store attack information
    public GameObject attackTest;
    public float attackCoolDownTime = 0.2f;     //cool down time on player attack
    public float attackTimer;
    public float attackRadius = 5f;

    //to store health information
    public GameObject health1;          //convert this into an array or a list
    public GameObject health2;
    public GameObject health3;
    public int playerHealth = 70;

    //to store pitfall information
    public Transform pitfallPoint;
    private float pitfallDepth = -20f;

    Controller2D controller;
    RaycastController raycastController;

    void Start () {
        controller = GetComponent<Controller2D>();
        raycastController = GetComponent<RaycastController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);     //more intuitive equation that allows 'jumpheight' and 'timetojumpapex' variables to be manipulated
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //print("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);   //can refer to console to find out how jumpheight and timetojumpapex affect gravity and jumpvelocity 
    }


    void FixedUpdate() {                //fixedupdate stops the boss' jittery movement BUT makes attackEnemy less responsive FIXED with additional update

        //Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")      //if I want to regain control of player, paste this back into above Vector2 input
        //autoMoveSpeed, 0f                                                 //if I want the player to move right automatically, paste this back into above Vector2 input
        //Vector2 input2 = new Vector2(autoMoveSpeed2, 0f);

        //float targetVelocityX = input2.x * moveSpeed;
        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));    //smoother turning on x axis
        //velocity.y += gravity * Time.deltaTime;
        //controller.Move(velocity * Time.deltaTime);


    }

    void Update()           //put all inputs into update
    {
        //print(playerHealth);            

        AttackEnemy();

        Jump();

        DieOnPitfalls();

        Vector2 input = new Vector2(autoMoveSpeed, 0f);

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne));    //smoother turning on x axis
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;             //so that the raycast does not continually accumulate length when player is stationary/colliding with the ground
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;  //can jump only if space is pressed and player is colliding with the ground
        }
    }


    void AttackEnemy ()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCoolDownTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.F))
            {
                //print("isAttacking");
                attackTest.SetActive(true);         //sets to true for as long as attackcooldowntime lasts    
                attackTest.transform.localScale = new Vector3(attackRadius, attackRadius, 0);   //sets scale of attacktest object to attackradius
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, raycastController.collisionMaskEnemy);        //returns array of all detected colliders
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].gameObject.tag == "Boss")            //if enemy in list has tag "Boss"
                    {
                        enemies[i].gameObject.GetComponent<BossAI>().Damage(10);        //damage function
                    }
                    else
                    {
                        Destroy(enemies[i].gameObject);
                    }
                   
                }
                attackTimer = 0.00001f;                //not sure if I want the player to even have a cooldown time (button mashing may be more fun)

            }
            else
            {
                attackTest.SetActive(false);
            }
        }

    }

    void DieOnPitfalls()
    {
        if (transform.position.x <= pitfallPoint.position.x)
        {
            if (transform.position.y <= pitfallDepth)
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    public void Damage(int amount)              //ask about this; need to find a way to decrease health by only one
    {
        playerHealth -= amount;
        if (playerHealth <= 64)             //pretty ghetto alternative; change asap 
        {
            Destroy(health1);
        }
        if (playerHealth <= 37)
        {
            Destroy(health2);
        }
        if (playerHealth <= 22)
        {
            Destroy(health3);
            
        }
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }

}
