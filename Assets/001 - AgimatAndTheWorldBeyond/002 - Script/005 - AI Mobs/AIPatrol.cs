using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    //global references
    [SerializeField] private GameObject detectedPlayer;

    [Header("Mob Data")]
    [SerializeField] private float patrolDistance;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float climbSpeed;
    
    [SerializeField] private float chaseThreshold;
    [ReadOnly] [SerializeField] public bool isPatrolling; 
    [ReadOnly] [SerializeField] private bool isAgro;
    [ReadOnly] [SerializeField] private bool isChasing;
    [ReadOnly] [SerializeField] private bool isTouchingWall;
    [ReadOnly] [SerializeField] private bool isSteppingOnLedge;
    [ReadOnly] [SerializeField] private bool isFacingRight;
    private Vector2 startingPoint;
    [ReadOnly][SerializeField] private float distanceTravelled;

    [Header("Features")]
    [SerializeField] public bool willChasePlayer;
    [SerializeField] public bool hasClimbingFeature;
    [SerializeField] public bool hasBacksight;

    //  Checkers
    [Header("Checkers")]
    [SerializeField] public Transform lineOfSight;
    [SerializeField] private Transform wallChecker;
    [SerializeField] private Transform ledgeChecker;

    //cached values
    [Header("Cached Values")]
    public Rigidbody2D rb;
    public Animator anim;
    public float distanceRay = 5;

    void Awake()
    {
        //mainPlayer = GameManager.instance.PlayerStats.GetSetPlayerCharacterObj.transform.parent.gameObject;

        isPatrolling = true;
        isAgro = false;
        isChasing = false;
        isSteppingOnLedge = false;
        isFacingRight = true;
        startingPoint = gameObject.transform.position;
        Debug.Log("first starting point:" + startingPoint);
        chaseThreshold = 15f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    //  Processes whether state is climbing or patrolling
    private void Update()
    {
        if (willChasePlayer)
        {
            //  Enemy is close enough for player to chase
            //if (Vector2.Distance(transform.position, mainPlayer.transform.position) <= chaseThreshold)
            if (CanSeePlayer(chaseThreshold))
            {
                isPatrolling = false;
                isAgro = true;
            }
            //  Enemy cannot see player in general
            else
            {
                //  If the enemy cannot see the player because they got away, the enemy wait 2 seconds then will have a new starting point to patrol from
                if (isAgro)
                {
                    if (!isChasing)
                    {
                        isChasing = true;
                        Invoke("StopChasingPlayer", 5);
                    }
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

            if (isAgro)
            {
                ChasePlayer();
            }
        }

        //  Processes what to do if the enemy is touching a wall
        if(HasTouchedWall(0.125f))
        {
            if (hasClimbingFeature)
            {
                //If not yet climbing, stop patrolling and play the climbing animation
                if (!anim.GetBool("isClimbing"))
                {
                    isPatrolling = false;
                    anim.SetBool("isClimbing", true);                   
                }                    

                //  We separate this loop so that the enemy will climb on EVERY FRAME that isClimbing is True
                if (anim.GetBool("isClimbing"))
                {
                    ClimbUp();                  
                }
            }
            else
            {
                Flip();
            }
        }

        else if (HasDetectedLedge(0.125f) && anim.GetBool("isClimbing"))
        {
            anim.SetBool("isClimbing", false);
            isSteppingOnLedge = false;
            isPatrolling = true;
            rb.gravityScale = 1f;
        }
    }

    private void FixedUpdate()
    {      
        //  Decide whether to patrol or to chase
        if (isPatrolling) Patrol();
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (hasClimbingFeature)
        {
            if (collision.gameObject.tag == "Ground" && anim.GetBool("isClimbing"))
            {
                Debug.Log("back to walking");
                anim.SetBool("isClimbing", false);
                isPatrolling = true;
                rb.gravityScale = 1f;
            }

        }
    }*/

    private bool CanSeePlayer(float aggroThreshold)
    {
        bool val = false;
        float sightRange = aggroThreshold;

        if (!isFacingRight)
        {
            sightRange = -aggroThreshold;
        }

        Vector2 endPos = lineOfSight.position + Vector3.right * sightRange;
        RaycastHit2D playerHit = Physics2D.Linecast(lineOfSight.position, endPos, LayerMask.GetMask("Character"));
        

        if (playerHit.collider != null)
        {
            if (playerHit.collider.gameObject.CompareTag("Player"))
            {
                detectedPlayer = playerHit.collider.gameObject;
                val = true;
            }
            else
                val = false;
        }
        else
        {
            Debug.DrawLine(lineOfSight.position, endPos, Color.yellow);
        }
        return val;
    }

    private bool HasTouchedWall(float wallSearchThreshold)
    {
        float wallRange = wallSearchThreshold;

        if (!isFacingRight)
        {
            wallRange = -wallSearchThreshold;
        }
        Vector2 wallRangeEnd = wallChecker.position + Vector3.right * wallRange;
        RaycastHit2D wallFinder = Physics2D.Linecast(wallChecker.position, wallRangeEnd, LayerMask.GetMask("Ground"));
        if (wallFinder.collider != null)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }
        Debug.DrawLine(wallChecker.position, wallRangeEnd, Color.yellow);
        return isTouchingWall;
    }

    private bool HasDetectedLedge(float ledgeSearchThreshold)
    {

        float ledgeRange = ledgeSearchThreshold;

        if (!isFacingRight)
        {
            ledgeRange = -ledgeSearchThreshold;
        }
        Vector2 ledgeRangeEnd = ledgeChecker.position + Vector3.right * ledgeRange;
        RaycastHit2D ledgeFinder = Physics2D.Linecast(ledgeChecker.position, ledgeRangeEnd, LayerMask.GetMask("Ground"));
        if (ledgeFinder.collider != null)
        {
            isSteppingOnLedge = false;
            Debug.Log("looking for ledge");
        }
        else
        {
            isSteppingOnLedge = true;
        }
        Debug.DrawLine(ledgeChecker.position, ledgeRangeEnd, Color.green);
        return isSteppingOnLedge;
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
        isFacingRight = !isFacingRight;
    }

    public void ClimbUp()
    {
        anim.SetBool("isClimbing", true);
        rb.velocity = new Vector2(rb.velocity.x, climbSpeed * 2 * Time.fixedDeltaTime);
        rb.gravityScale = 0f;
    }

    private void ChasePlayer()
    {       
        Debug.Log("is chasing the player");
        //To do: add secondary raycast to check if player is behind enemy WHILE in chase state

        //  enemy is to the left of player
        if (transform.position.x < detectedPlayer.transform.position.x && patrolSpeed < 0 && detectedPlayer.GetComponent<GroundPlayerController>().CheckIfTouchGround) Flip();
        //  enemy is to the right of player
        else if (transform.position.x > detectedPlayer.transform.position.x && patrolSpeed > 0 && detectedPlayer.GetComponent<GroundPlayerController>().CheckIfTouchGround) Flip();

        rb.velocity = new Vector2(patrolSpeed * 2 * Time.fixedDeltaTime, 0f);
    }

    private void StopChasingPlayer()
    {
        isAgro = false;
        isChasing = false;
        startingPoint = transform.position;
        isPatrolling = true;
    }
}
