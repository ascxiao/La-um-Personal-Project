using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 2f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Vector2 lastDirection;
    private bool runningState;
    public Rigidbody2D rb;
    public Animator animator;
    public static PlayerMovement instance;
    public bool isAttacking = false;
    public bool isDashing = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        instance = this;
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Movement.Run.performed += OnRunPressed;
        playerControls.Attack.Sword.performed += Attack;
        playerControls.Movement.Dash.performed += Dash;
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;

        if (movement != Vector2.zero)
        {
            lastDirection = movement.normalized;
        }
    }

    private void Move()
    {
        Flip();

        lastDirection = movement.normalized;

        if (runningState == false)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isMoving", true);
            rb.linearVelocity = movement * moveSpeed;
        }
        else
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isMoving", true);
            rb.linearVelocity = movement * (moveSpeed + 0.5f);
        }

        if (movement == Vector2.zero)
        {
            animator.SetBool("isMoving", false);
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            animator.SetFloat("XInput", movement.x);
            animator.SetFloat("YInput", movement.y);
        }
    }

    private void OnRunPressed(InputAction.CallbackContext context)
    {
        runningState = !runningState;
    }

    void Attack(InputAction.CallbackContext context)
    {
        if (!isAttacking && context.performed)
        {
            isAttacking = true;
            moveSpeed -= 0.5f;
        }

    }

    void Dash(InputAction.CallbackContext context)
    {
        if (!isDashing && context.performed)
        {
            //FIX DASHHHHHH
            isDashing = true;
            rb.linearVelocity = lastDirection * dashSpeed;
        }
    }

    void Flip()
    {
        if (movement.x > 0 && transform.localScale.x < 0 ||
            movement.x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
}
