using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRun : MonoBehaviour
{
    // Variables for the animator, position, and direction
    public Animator animator;

    Vector3 previousPosition;
    Vector3 lastMoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        lastMoveDirection = Vector3.zero;
    }

    // If the postion if not equal to it's previous position then it sets the difference in the postions
    private void FixedUpdate()
    {
        if (transform.position != previousPosition)
        {
            lastMoveDirection = (transform.position - previousPosition).normalized;
            previousPosition = transform.position;

            // Sets the booleans for the enemy movement for the animator
            if (lastMoveDirection.y >= 0)
            {
                Debug.Log("ENEMY UP");
                animator.SetBool("Up", true);
                animator.SetBool("Down", false);
            }
            else if (lastMoveDirection.y <= 0)
            {
                Debug.Log("ENEMY DOWN");
                animator.SetBool("Up", false);
                animator.SetBool("Down", true);
            }
            if (lastMoveDirection.x <= 0)
            {
                Debug.Log("ENEMY LEFT");
                animator.SetBool("Left", true);
                animator.SetBool("Right", false);
            }
            else if (lastMoveDirection.x >= 0)
            {

                Debug.Log("ENEMY RIGHT");
                animator.SetBool("Left", false);
                animator.SetBool("Right", true);
            }

            // Sets the float for the speed of the enemy for the animator
            animator.SetFloat("Speed", lastMoveDirection.sqrMagnitude);
        }
    }
}
