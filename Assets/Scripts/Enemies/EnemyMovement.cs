using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject aggro;
    [SerializeField] private GameObject boundary;
    [SerializeField] private float intervalMovement;
    [SerializeField] private float movementSpeed;

    private float baseSpeed;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D aggroHitbox;
    private Collider2D boundaryCollider;
    private int aggroCooldown;
    private Coroutine countdownCoroutine;
    private Coroutine moveCoroutine;
    private Vector2 movement;

    public float aggroSpeed = 0.75f;
    public bool isAggro = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aggroHitbox = aggro.GetComponent<Collider2D>();
        boundaryCollider = boundary.GetComponent<Collider2D>();
        baseSpeed = movementSpeed;
        EnableCoroutine();
    }

    private void FixedUpdate()
    {
        if (!isAggro && boundaryCollider.OverlapPoint(transform.position))
        {
            WithinPath();
        }
        else if (!isAggro && !boundaryCollider.OverlapPoint(transform.position))
        {
            Vector2 direction = (boundaryCollider.bounds.center - transform.position).normalized;
            rb.linearVelocity = direction * 0.5f;
            Flip(direction);
        }
    }

    public void Flip(Vector2 direction)
    {
        if (direction.x > 0 && transform.localScale.x > 0 ||
            direction.x < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }


    public void EnableCoroutine()
    {
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(WanderRoutine());
        }
    }
    public void DisableCoroutine()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        if (TryGetComponent(out Rigidbody2D rb))
            rb.linearVelocity = Vector2.zero;
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
            isAggro = true;
            animator.SetBool("isAggro", true);
            movementSpeed = 0.75f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!aggroHitbox.IsTouching(other) && other.CompareTag("Player"))
        {
            aggroCooldown = 3;
            countdownCoroutine = StartCoroutine(AggroTimer());
        }
    }


    private void WithinPath()
    {
        Vector2 nextPos = rb.position + rb.linearVelocity * Time.fixedDeltaTime;

        if (!boundaryCollider.OverlapPoint(nextPos) && !isAggro)
        {
            movement = -movement;
            Flip(movement);
        }
    }
    IEnumerator AggroTimer()
    {
        while (aggroCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            aggroCooldown--;
        }

        movementSpeed = baseSpeed;
        isAggro = false;
        animator.SetBool("isAggro", false);
    }

    IEnumerator WanderRoutine()
    {
        while (!isAggro)
        {
            rb.linearVelocity = Vector2.zero;

            yield return new WaitForSeconds(0.5f);

            movement = Random.insideUnitCircle.normalized;

            rb.linearVelocity = movement * movementSpeed;

            yield return new WaitForSeconds(intervalMovement);

        }
    }
}
