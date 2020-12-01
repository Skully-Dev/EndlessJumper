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

    private Platform platform;

    private Rigidbody2D rb; //physics
    private Collider2D coll; //touching layer
    private Animator anim; //animations
    private SpriteRenderer sprite; //flip

    [SerializeField, Tooltip("Sound played on bounce")]
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

    private float backupJumpTime = 3;


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
        //flip sprite
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

        //not physics, but doesn't require as frequent checks
        if (Time.time > backupJumpTime)
        {
            Debug.LogWarning("Something went wrong, using backup jump");
            //isGrounded = false;
            //state = State.Fall;
            //anim.SetInteger("state", (int)state);
            //JumpAnim();

            rb.velocity = new Vector2(rb.velocity.x, jumpForce); //apply force but dont remove platform, benefit of the doubt.
            backupJumpTime = Time.time + 3;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayerIndex)
        {
            JumpAnim();
            GetPlatform();
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
        anim.SetInteger("state", (int)state); //start jump animation
        backupJumpTime = Time.time + 3;
    }

    /// <summary>
    /// Since Collision2D with OnCollisionEnter sometimes is returning the incorrect platform (the close one just above).
    /// I required a method to get the correct platform to trigger platform fade on the correct object.
    /// </summary>
    private void GetPlatform()
    {
        RaycastHit2D hit;

        float radius = 0.5f;

        Vector2 origin = transform.position;
        origin.y -= radius; //offsets to bottom of collider

        Vector2 originL = origin;
        originL.x -= radius; //from left side of collider

        Vector2 originR = origin;
        originR.x += radius; //from right side of collider

        float distance = 0.05f; //casts a little below

        hit = Physics2D.Raycast(originL, Vector2.down, distance, groundLayer);
        if (hit.collider == null)
        {
            hit = Physics2D.Raycast(originR, Vector2.down, distance, groundLayer);
        }

        if (hit.collider != null)
        {
            platform = hit.collider.gameObject.GetComponent<Platform>();
        }
    }

    public void SlimeSound()
    {
        slimeSound.Play();
    }

    public void AddForce()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); //apply force
        if (platform != null)
        {
           platform.JumpedOn();
        }        
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
                anim.SetInteger("state", (int)state); 
            }
        }
        //else if (state == State.Fall)
    }
}