using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject aggro;
    [SerializeField] private GameObject boundary;
    [SerializeField] private float intervalMovement;
    [SerializeField] private float movementSpeed;

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D aggroHitbox;
    private Collider2D boundaryCollider;
    private int aggroCooldown;
    private Coroutine countdownCoroutine;
    private Vector2 movement;
    private float aggroSpeed = 0.75f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aggroHitbox = aggro.GetComponent<Collider2D>();
        boundaryCollider = boundary.GetComponent<Collider2D>();
        StartCoroutine(WanderRoutine());
    }

    private void FixedUpdate()
    {
        Vector2 nextPos = rb.position + rb.linearVelocity * Time.fixedDeltaTime;

        if (!boundaryCollider.OverlapPoint(nextPos))
        {
            movement = -movement;
            rb.linearVelocity = movement * movementSpeed;
        }
    }

    ///------------------------------------------

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (aggroHitbox.IsTouching(other) && other.CompareTag("Player"))
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }
            animator.SetBool("isAggro", true);
            movementSpeed = 0.75f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!aggroHitbox.IsTouching(other) && other.CompareTag("Player"))
        {
            aggroCooldown = 10;
            countdownCoroutine = StartCoroutine(AggroTimer());
        }
    }

    IEnumerator AggroTimer()
    {
        while (aggroCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            aggroCooldown--;
        }

        movementSpeed = 0.25f;
        animator.SetBool("isAggro", false);
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            rb.linearVelocity = Vector2.zero;

            yield return new WaitForSeconds(0.5f);

            movement = Random.insideUnitCircle.normalized;
            Debug.Log(movement);

            rb.linearVelocity = movement * movementSpeed;

            yield return new WaitForSeconds(intervalMovement);

        }
    }
}
