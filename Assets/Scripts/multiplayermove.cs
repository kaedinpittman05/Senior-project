using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;


public class multiplayermove : MonoBehaviour
{

  
    
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;
    private Vector2 movementinput = Vector2.zero ;


  
   
    public void OnMove(InputAction.CallbackContext context)
    {
        movementinput = context.ReadValue<Vector2>();
    }
    
    
    
    
    void Update()
    {

        Vector2 move = new Vector2(movementinput.x, movementinput.y);


        if (PauseMenu.GameIsPaused == false)
        {
            animator.SetFloat("Horizontal", movementinput.x);
            animator.SetFloat("Vertical", movementinput.y);
            animator.SetFloat("Speed", move.sqrMagnitude);
        }
        


    }

    private void FixedUpdate()
    {
        Vector2 move = new Vector2(movementinput.x, movementinput.y);
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

    }







}
