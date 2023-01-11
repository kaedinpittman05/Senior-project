using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables for the rigidbody, animator, movement speed, and the movement
    public Rigidbody2D rb;
    public Animator animator;

    // Start of particles
       //public ParticleSystem particle;

    public float moveSpeed = 5f;

    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        // Inputs for up(w), down(s), left(a), and right(d) keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Sets the booleans for the player movement for the animator
        if (movement.y > 0)
        {
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);

        }
        else if (movement.y < 0)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", true);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
     

        }
        else if (movement.x < 0)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);


        }
        else if (movement.x > 0)
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
    
        }
        else
        {
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
         
        }

        // Sets the float for the speed of the player for the animator
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        // Moves character based on key input and speed 
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
