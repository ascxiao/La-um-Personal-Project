using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private bool runningState;
    public Rigidbody2D rb;
    public Animator animator;
    public static PlayerMovement instance;
    public bool isAttacking = false;
    public PlayerCombat playerCombat;

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
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
        playerControls.Movement.Run.performed += OnRunPressed;
        playerControls.Attack.Sword.performed += Attack;
    }

    private void Move()
    {
        Flip();

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

    void Flip()
    {
        if (movement.x > 0 && transform.localScale.x < 0 ||
            movement.x < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
}
