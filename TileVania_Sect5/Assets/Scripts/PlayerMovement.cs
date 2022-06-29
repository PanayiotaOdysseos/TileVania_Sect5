using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    float gravityScaleAtStart;

    
    Animator anim;
    CapsuleCollider2D myCapsuleCollider;

    void Start()
    {

        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;
    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    void OnJump(InputValue value)
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
            
        if (value.isPressed)
            {
                myRigidbody.velocity += new Vector2(0f, jumpSpeed);
                
        }
        
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        anim.SetBool("isRunning", playerHasHorizontalSpeed);
        

        /* if (myRigidbody.velocity.x > 0 || myRigidbody.velocity.x < 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }*/
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon; // Epsilon a better way than saying > 0

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
            //player sprite changes directions based on the x axis -1=left & 1=right, while the y axis stays the same
            //we are changing the scale of the player sprite

        }

    }

    void ClimbLadder()
    {
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            anim.SetBool("isClimbing", false);
            return;
        }
        
            Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.velocity = climbVelocity;
            myRigidbody.gravityScale = 0f;
            bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
            anim.SetBool("isClimbing", playerHasVerticalSpeed);
            Debug.Log("Climb!");
        
    }
}
