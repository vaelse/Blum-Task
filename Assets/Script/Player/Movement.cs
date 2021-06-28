using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    float gravscale;
    [SerializeField] float stopInputDash;
    [SerializeField] float stopInputWallJump;
    float wallCheckRadius = .2f;
    [Header("Bool")]
    public bool canDoubleJump;
    public bool canGrab, isGrabbing, isGrounded, isDashing, canDash = true;
    [Header("Dash")]
    public float dashCoolDown;
    public float dashDuration;
    public float dashForce;
    [Header("Jump")]
    public float wallJumpTime;
    public float doubleJumpForce;
    public float jumpForce;
    [Header("WallGrabJump")]
    public float wallJumpPushForce;
    public float wallJumpForce;
    [Header("Movement")]
    public float movementSpeed;
    public float moveInput;
    [Header("KnockBack")]
    public float knockback;
    public float knockbackLenght;
    public float knockbackCount;
    public bool knockfromright;
    InputSystem input;
    Rigidbody2D rb;
    Collider2D col;
    Animator anim;
    public GameObject gameManager;
    public Transform frontCheck;
    public Transform groundCheck;
    [SerializeField] ParticleSystem dustParticle;
    [SerializeField] ParticleSystem wallJumpParticle;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask Wall;

    private void Awake()
    {
        input = new InputSystem();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        
    }
    private void Update()
    {
        if(PauseMenu.isMenuActive)
        {
            return;
        }
        moveInput = input.Player.Movement.ReadValue<float>();
        if (stopInputWallJump <= 0 && stopInputDash <= 0)
        {
            Moving();
            Jump();
            Dash();
        }
        else
        {
            stopInputWallJump -= Time.deltaTime;
            stopInputDash -= Time.deltaTime;
        }
        if (isDashing)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashForce, 0);
        }
    }

    public void Moving()
    {
        if (knockbackCount <= 0)
        {
            rb.velocity = new Vector2(moveInput * movementSpeed, rb.velocity.y);
        }
        else
        {
            if (knockfromright)
            {
                rb.velocity = new Vector2(-knockback, knockback);
            }
            else
            {
                rb.velocity = new Vector2(knockback, knockback);
            }
            knockbackCount -= Time.deltaTime;
        }

        if (moveInput < 0)
        {
            anim.SetBool("isRunning", true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput > 0)
        {
            anim.SetBool("isRunning", true);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        WallGrab();
    }

    public void WallGrab()
    {
        canGrab = Physics2D.OverlapCircle(frontCheck.position, wallCheckRadius, Wall);
        isGrabbing = false;

        if (canGrab && !isGrounded)
        {
            if ((transform.localScale.x == 1 && moveInput > 0) || (transform.localScale.x == -1 && moveInput < 0))
            {
                isGrabbing = true;
            }
        }

        if (isGrabbing)
        {
            canDoubleJump = false;
            rb.gravityScale = gravscale;
            anim.SetBool("isGrabbingWall", true);
            if (input.Player.Jump.triggered)
            {
                stopInputWallJump = wallJumpTime;
                isGrabbing = false;
                rb.gravityScale = 1;
                rb.velocity = new Vector2(-moveInput * wallJumpPushForce, wallJumpForce);
                wallJumpParticle.Play();
                anim.SetBool("isGrabbingWall", false);
                if (moveInput < 0)
                {
                    transform.localScale = Vector3.one;
                }
                else if (moveInput > 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                Invoke("AfterWallDoubleJump", stopInputWallJump);
            }
        }
        else
        {
            anim.SetBool("isGrabbingWall", false);
            if (!isDashing)
                rb.gravityScale = 1;
        }
    }

    public void Dash()
    {
        if (input.Player.Dash.triggered && canDash && !isGrabbing)
        {
            stopInputDash = dashDuration;
            StartCoroutine(Dashing(dashDuration, dashCoolDown));
        }
    }

    IEnumerator Dashing(float x, float y)
    {
        var originalConstraints = rb.constraints;
        isDashing = true;
        canDash = false;
        Physics2D.IgnoreLayerCollision(7, 8, true);
        Physics2D.IgnoreLayerCollision(7, 11, true);
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(x);
        canDoubleJump = true;
        isDashing = false;
        rb.gravityScale = 1;
        col.isTrigger = false;
        rb.constraints = originalConstraints;
        Physics2D.IgnoreLayerCollision(7, 8, false);
        Physics2D.IgnoreLayerCollision(7, 11, false);
        yield return new WaitForSeconds(y);
        canDash = true;
    }
    public void Jump()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(.1f, .1f), Ground);
        anim.SetBool("isJumping", false);
        if (input.Player.Jump.triggered)
        {
            if (isGrounded)
            {
                canDoubleJump = true;
                anim.SetBool("isJumping", true);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                createDust();
            }
            else if (canDoubleJump)
            {
                anim.Play("DoubleJump_Animation");
                rb.velocity = new Vector2(0, doubleJumpForce);
                canDoubleJump = false;
            }
        }

        if (!isGrounded)
        {
            anim.SetBool("isJumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (col.IsTouchingLayers(Ground))
        {
            canDoubleJump = true;
            anim.SetBool("isJumping", false);
            createDust();
        }
    }

    public void createDust()
    {
        dustParticle.Play();
    }

    public void AfterWallDoubleJump()
    {
        canDoubleJump = true;
    }
    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }
}
