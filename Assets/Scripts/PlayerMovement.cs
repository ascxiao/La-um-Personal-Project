using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float runningSpeed = 1.5f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private bool runningState;

    public float frameRate;
    float idleTime;

    private void Awake(){
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    private void Update(){
        PlayerInput();
    }

    private void FixedUpdate(){
        Move();
    }

    private void PlayerInput(){
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
        playerControls.Movement.Run.performed += OnRunPressed;
    }

    private void Move(){
        if(runningState == false){
            animator.SetBool("isRunning", false);
            animator.SetBool("isMoving", true);
            rb.linearVelocity = movement * moveSpeed;
        } else {
            animator.SetBool("isRunning", true);
            animator.SetBool("isMoving", true);
            rb.linearVelocity = movement * runningSpeed;
        }

        if (movement == Vector2.zero){
            animator.SetBool("isMoving", false);
            rb.linearVelocity = Vector2.zero;
        }else{
            animator.SetFloat("XInput", movement.x);
            animator.SetFloat("YInput", movement.y);
        }
    }

        private void OnRunPressed(InputAction.CallbackContext context)
    {
        runningState = !runningState;
    }
}
