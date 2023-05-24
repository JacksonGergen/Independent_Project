using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator animate;

    private float moveSpeed;        // checks player speed
    private float jumpForce;        // checks player jump force
    private bool isJumping;         // checks if player is jumping
    private bool hasDoubleJumped;   // checks if player has jumped twice
    private float moveHorizontal;   // checks input for horizontal movement of player
    private float moveVertical;     // checks input for vertical movement of player
    private bool facingRight = true;// checks whether player is looking right or not

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animate = gameObject.GetComponent<Animator>();

        moveSpeed = 0.75f;
        jumpForce = 30f;
        isJumping = false;
        hasDoubleJumped = false;
    }

    // Update is called once per frame
    // Code that runs in background for input
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        animate.SetFloat("Speed", Mathf.Abs(moveHorizontal));

        // Check if player has pressed the jump button
        if (Input.GetButtonDown("Vertical"))
        {
            if (isJumping && !hasDoubleJumped)
            {
                // Perform the double jump
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0f); // Reset the y velocity before jumping again
                rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                hasDoubleJumped = true;
            }
            else if (!isJumping)
            {
                // Perform the initial jump
                rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }

        if (moveHorizontal > 0 && !facingRight)
        {
            flip();
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            flip();
        }
    }

    // Somewhat the same as Update
    // However, this updates with the Physics Engine
    void FixedUpdate()
    {
        if (moveHorizontal > 0.1f || moveHorizontal < -0.1f)
        {
            rb2D.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = false;
            hasDoubleJumped = false; // Reset the double jump flag when the player lands on a platform
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            isJumping = true;
        }
    }

    private void flip()
    {
        facingRight = !facingRight;

        Vector2 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

}
