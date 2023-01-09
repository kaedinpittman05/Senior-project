using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRun : MonoBehaviour
{
    Vector3 previousPosition;
    Vector3 lastMoveDirection;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        lastMoveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (transform.position != previousPosition)
        {
            lastMoveDirection = (transform.position - previousPosition).normalized;
            previousPosition = transform.position;
            //Debug.Log("Enemy Moevment"+lastMoveDirection + previousPosition);
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
            animator.SetFloat("Speed", lastMoveDirection.sqrMagnitude);
        }
    }
}
