using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("ran into wall");
            if (transform.parent.GetComponent<AIPatrol>().hasClimbingFeature)
            {
                transform.parent.GetComponent<AIPatrol>().isPatrolling = false;
                transform.parent.GetComponent<AIPatrol>().ClimbUp();
            }               
            else
                transform.parent.GetComponent<AIPatrol>().Flip();
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (transform.parent.GetComponent<AIPatrol>().hasClimbingFeature)
        {
            if (collision.gameObject.tag == "Ground")
            {
                Debug.Log("done climbing");
                transform.parent.GetComponent<AIPatrol>().anim.SetBool("isClimbing", false);
                transform.parent.GetComponent<AIPatrol>().isPatrolling = true;
                transform.parent.GetComponent<AIPatrol>().rb.gravityScale = 1f;
            }    
                
        }       
    }*/
}
