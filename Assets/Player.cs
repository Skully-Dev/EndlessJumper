using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    public enum State
    {
        Jump,
        Fall,
    }
    private State state;

    private Rigidbody2D rb; //physics
    private Collider2D coll; //touching layer
    private Animator anim; //animations
    private SpriteRenderer sprite;

    [SerializeField]
    private AudioSource slimeSound;

    [SerializeField] //loop screen
    private float offScreenR;
    private float offScreenL;
    private float offScreenDifference;

    private int groundLayerIndex;
    private LayerMask groundLayer;

    [Tooltip("Input axis X, between -1 and 1")]
    private float horizontalDirection; //user input
    [SerializeField]
    private float moveSpeed; //user input influence

    [SerializeField]
    private float jumpForce; //jump height

    private bool isGrounded;
    [Tooltip("time off ground allowed before isGrounded=false")]
    private float coyoteTime = 0.05f;
    [Tooltip("Timer for coyoteTime, if < 0, isGrounded=false")]
    private float coyoteTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        //endless screen
        offScreenL = -offScreenR;
        offScreenDifference = offScreenR - offScreenL;

        groundLayerIndex = LayerMask.NameToLayer("Ground");
        groundLayer = 1 << groundLayerIndex;

        coyoteTimer = coyoteTime;
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        //endless screen
        if (transform.position.x > offScreenR)
        {
            transform.position = new Vector2(transform.position.x - offScreenDifference, transform.position.y);
        }
        else if (transform.position.x < offScreenL)
        {
            transform.position = new Vector2(transform.position.x + offScreenDifference, transform.position.y);
        }

        //user input
        horizontalDirection = Input.GetAxis("Horizontal");
        if (horizontalDirection > 0 && !sprite.flipX)
        {
            sprite.flipX = true;
        }
        else if (horizontalDirection < 0 && sprite.flipX)
        {
            sprite.flipX = false;
        }

        GroundedCheck();

        UpdateState();
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        Movement();

        if (Time.time > backupJumpTime)
        {
            Debug.LogWarning("Player got stuck, using backup jump");

            JumpAnim();
        }
    }

    private float backupJumpTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayerIndex)
        {
            JumpAnim();            
        }
    }

    /// <summary>
    /// apply horizontal velocity
    /// </summary>
    private void Movement()
    {
        float speed = Mathf.Lerp(rb.velocity.x, horizontalDirection * moveSpeed, 0.25f);
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    public void JumpAnim()
    {
        state = State.Jump;
        anim.SetInteger("state", 0); //start jump animation
        backupJumpTime = Time.time + 4;
    }

    public void SlimeSound()
    {
        slimeSound.Play();
    }

    public void AddForce()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); //apply force
    }

    /// <summary>
    /// Determines if isGrounded.
    /// Tracks coyoteTimer.
    /// </summary>
    private void GroundedCheck()
    {
        if (coll.IsTouchingLayers(groundLayer))
        {
            coyoteTimer = coyoteTime;
            isGrounded = true;
        }
        else if (isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
            if (coyoteTimer < 0f)
            {
                isGrounded = false;
            }
        }
    }

    public void UpdateState()
    {
        if (state == State.Jump)
        {
            if (!isGrounded && rb.velocity.y < 0)
            {
                state = State.Fall;
                anim.SetInteger("state", 1);
            }
        }
        //else if (state == State.Fall)
    }
}