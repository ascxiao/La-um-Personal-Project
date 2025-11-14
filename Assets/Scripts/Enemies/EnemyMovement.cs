using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private GameObject aggro;
    private Collider2D aggroHitbox;
    private int aggroCooldown = 10;
    private Coroutine countdownCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        aggroHitbox = aggro.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (aggroHitbox.IsTouching(other) && other.CompareTag("Player"))
        {
            Debug.Log(other);
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }
            animator.SetBool("isAggro", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!aggroHitbox.IsTouching(other) && other.CompareTag("Player"))
        {
            aggroCooldown = 10;
            countdownCoroutine = StartCoroutine(StartCountdown());
        }
    }

    IEnumerator StartCountdown()
    {
        while (aggroCooldown > 0)
        {
            Debug.Log(aggroCooldown);
            yield return new WaitForSeconds(1f);
            aggroCooldown--;
        }

        animator.SetBool("isAggro", false);
    }
}
