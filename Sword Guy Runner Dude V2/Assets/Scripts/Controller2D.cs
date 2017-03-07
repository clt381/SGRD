using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Controller2D : RaycastController {

    float maxClimbAngle = 80;       //adjust to determine how steep the obstacles the player can climb are
    float maxDescendAngle = 75;     //adjust to determine how steep the obstacles the player can descend are
    public CollisionInfo collisions;

    //to store enemy prefab and enemy spawn position information
    public List<GameObject> enemies;
    public List<Vector3> enemyPositions;
    public float enemySpawnDistance = 20f;

    public override void Start()
    {
        base.Start();
    }

    public void Move(Vector3 velocity) {      //collisions modify velocity so that the player can't go through objects
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;
        SpawnEnemy();           //works, but problem where the player cannot move after the final enemy in the list has been spawned; solved: made an if statement that conditions enemypositions index to be greater than zero to spawn enemies

        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        if (velocity.x !=0)
        {
            HorizontalCollisions(ref velocity);
        }
        
        if (velocity.y !=0)
        {
            VerticalCollisions(ref velocity);
        }
        
        
        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)         //PROBLEM: HORIZONTAL COLLISIONS WITHOUT COLLISIONS BELOW RESULTS IN PLAYER DROPPING OUT OF MAP
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)        //to raycast according to horizontalraycount variable
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;        //where to start raycasts from
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            RaycastHit2D hitEnemy = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMaskEnemy);

            //Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);       //to test that horizontal raycasts are working

            if (hit)        //if raycasts hit something
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxClimbAngle)
                    if (collisions.descendingSlope)         //to prevent simultaneous collision with both upward and downward slopes thus slowing player
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                {
                    float distanceToSlopeStart = 0;     //so that horizontal raycasts don't trigger on slopes until distance between player and slope is 0
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;           //to prevent raycasts from overriding each other when raycasting on objects of different heights simultaneously

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);  //to make horizontal collisions against objects on slopes more smooth
                    }

                    collisions.left = directionX == -1;     //if hit something and going left, collisions.left = true
                    collisions.right = directionX == 1;
                }     
            }

            if (hitEnemy)
            {
                //print("hitEnemy");
                                            //transform.position = respawnPoint.transform.position;        //respawns player at respawn point whenever it raycast collides with enemy
                                            //SceneManager.LoadScene(2);      //reload scene upon hitting enemy
                Player playerScript = GetComponent<Player>();
                playerScript.Damage(1);
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;         

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            RaycastHit2D hitEnemy = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMaskEnemy);       //not sure if i want player to die upon vertical collisions yet
            //RaycastHit2D hitPitfall = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMaskPitfall);

            //Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);      //to test that vertical raycasts are working

            if (hit)        //if raycasts hit something
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;           //to prevent raycasts from overriding each other when raycasting on objects of different heights simultaneously

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);    //to make vertical collisions against objects on slopes more smooth
                }

                collisions.below = directionY == -1;     
                collisions.above = directionY == 1;
            }

            if (hitEnemy)       //remember to place within the for loop or else hitEnemy won't exist in the context
            {
                //print("hitEnemy");
                //transform.position = respawnPoint.transform.position;        //respawns player at respawn point whenever it raycast collides with enemy
                //SceneManager.LoadScene(2);      //reload scene upon hitting enemy
                Player playerScript = GetComponent<Player>();
                playerScript.Damage(1);
            }

            //if (hitPitfall)
            //{
            //    print("hitPitfall");
            //    //transform.position = respawnPoint.transform.position;       //respawns player at respawn point whenever it raycast collides with pitfall
            //    SceneManager.LoadScene(2);      //reload scene upon hitting pitfall
            //}
        }
        if (collisions.climbingSlope)           //smooth transition from one slope to another
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle) //enables player to climb slopes at the same speed as flat ground if slope angle is less than or equal to 80
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;        //allows player to jump even when climbing slopes because jumping requires jump input && collisions.below
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
       
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        if (enemyPositions.Count > 0)
        {
            float enemyDistanceFromPlayer = Mathf.Abs(transform.position.x - enemyPositions[0].x);
            if (enemyDistanceFromPlayer <= enemySpawnDistance)
            {
                Instantiate(enemies[0], enemyPositions[0], Quaternion.identity);        //instantiate from index
                enemies.RemoveAt(0);                            //'shift' index by one so that the next instantiation is the next enemy always at 0
                enemyPositions.RemoveAt(0);
            }
        }
        
    }


   

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
	
}
