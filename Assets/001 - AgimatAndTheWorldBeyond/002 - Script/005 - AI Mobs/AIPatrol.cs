using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    //global references
    [SerializeField] private GameObject mainPlayer;

    //mob states and capabilities
    [SerializeField] private float patrolDistance;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float climbSpeed;
    [SerializeField] public bool hasClimbingFeature;
    [SerializeField] private float chaseThreshold;
    [ReadOnly] [SerializeField] public bool isPatrolling;   //opposite of patrolling is chasing
    [ReadOnly] [SerializeField] public bool isChasing;
    [ReadOnly] [SerializeField] public bool isAttacking;
    private Vector3 startingPoint;
    [ReadOnly][SerializeField] private float distanceTravelled;

    //cached values
    public Rigidbody2D rb;
    public Animator anim;
    public float distanceRay = 5;

    void Awake()
    {
        mainPlayer = GameManager.instance.PlayerStats.GetSetPlayerCharacterObj.transform.parent.gameObject;

        isPatrolling = true;
        isChasing = false;
        isAttacking = false;
        startingPoint = gameObject.transform.position;
        Debug.Log("first starting point:" + startingPoint);
        chaseThreshold = 15f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    //  Processes whether state is climbing or patrolling
    private void Update()
    {
        //  Enemy is close enough for player to chase
        if (Vector3.Distance(transform.position, mainPlayer.transform.position) <= chaseThreshold)
        {
            isChasing = true;
        }
        //  Enemy is too far for player to chase
        else
        {
            //  If the player got away, the enemy will have a new starting point to patrol from
            if (isChasing)
            {
                startingPoint = transform.position;
                Debug.Log("new starting point:" + startingPoint);
                isChasing = false;
                
            }
            //  enemy will stop chasing the player and go back to patrolling based on the previously created starting point
            else
            {
                if (anim.GetBool("isClimbing"))
                    isPatrolling = false;
                else
                    isPatrolling = true;
            }
        }
    }

    private void FixedUpdate()
    {      
        //  Decide whether to patrol or to chase
        if (isPatrolling) Patrol();
        if (isChasing) ChasePlayer();      
    }

    private void Patrol()
    {
        distanceTravelled = Mathf.Abs(gameObject.transform.position.x - startingPoint.x);
        if (distanceTravelled >= patrolDistance / 2)
        {
            Flip();
            isPatrolling = true;
        }
        rb.velocity = new Vector2(patrolSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    public void Flip()
    {
        isPatrolling = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        patrolSpeed *= -1;      
    }

    public void ClimbUp()
    {
        anim.SetBool("isClimbing", true);
        rb.velocity = new Vector2(rb.velocity.x, climbSpeed * 2 * Time.fixedDeltaTime);
        rb.gravityScale = 0f;
    }

    private void ChasePlayer()
    {
        isPatrolling = false;
        Debug.Log("is chasing the player");
        //  enemy is to the left of player
        if (transform.position.x < mainPlayer.transform.position.x && patrolSpeed < 0 && mainPlayer.GetComponent<GroundPlayerController>().CheckIfTouchGround) Flip();
        //  enemy is to the right of player
        else if (transform.position.x > mainPlayer.transform.position.x && patrolSpeed > 0 && mainPlayer.GetComponent<GroundPlayerController>().CheckIfTouchGround) Flip();

        rb.velocity = new Vector2(patrolSpeed * 2 * Time.fixedDeltaTime, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isAttacking = false;
        }
    }
}
