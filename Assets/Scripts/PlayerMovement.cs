using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

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
    }

    private void Move(){
        rb.linearVelocity = movement * moveSpeed;
        if (movement == Vector2.zero){
            rb.linearVelocity = Vector2.zero;
        }else{
            animator.SetFloat("XInput", movement.x);
            animator.SetFloat("YInput", movement.y);
        }
    }
}
