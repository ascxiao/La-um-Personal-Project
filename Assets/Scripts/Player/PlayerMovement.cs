using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 1f;
    [SerializeField] private float dashDistance = 0.5f;

    private PlayerControls playerControls;
    public Vector2 movement = Vector2.down;
    private Vector2 lastDirection = Vector2.down;
    private bool runningState;
    public PlayerHealth playerHealth;
    public Rigidbody2D rb;
    public Animator animator;
    public static PlayerMovement instance;
    public bool isAttacking = false;
    public bool isDashing = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerHealth = GetComponent<PlayerHealth>();
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
            isDashing = true;
            playerHealth.invincible = true;
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, lastDirection, dashDistance);

            if (hits.Length > 0)
            {
                RaycastHit2D? firstMapHit = hits.FirstOrDefault(hit => hit.collider.CompareTag("Map"));
                if (hits.Any(hit => hit.collider != null && hit.collider.CompareTag("Map")))
                {
                    transform.position = firstMapHit.Value.point - lastDirection * 0.2f;
                }
                else
                {
                    transform.position += (Vector3)lastDirection * dashDistance * 0.7f;
                }
            }
            else
            {
                transform.position += (Vector3)lastDirection * dashDistance * 0.7f;
            }
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

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 origin = transform.position;
        Vector3 direction = (Vector3)lastDirection;

        float maxDist = dashDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + direction * maxDist);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, lastDirection, dashDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Map"))
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(hit.point, 0.08f);
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(hit.point, 0.08f);
                }
            }
        }
    }
}
