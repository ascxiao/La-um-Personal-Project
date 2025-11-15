using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 1;
    public Transform player;

    [SerializeField] private GameObject atkHitbox;
    [SerializeField] private GameObject atkProx;

    private Collider2D hitbox;
    private Collider2D atkProxCol;
    public Rigidbody2D rb;
    private Animator animator;
    private Coroutine timer;
    public bool isAttacking = false;

    public EnemyMovement enemyMovement;
    public static EnemyCombat instance;

    private void Start()
    {
        hitbox = atkHitbox.GetComponent<Collider2D>();
        atkProxCol = atkProx.GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        instance = this;
    }

    //CHASING STATE

    private void Update()
    {
        Chase();
    }

    private void Chase()
    {
        if (enemyMovement.isAggro && !isAttacking)
        {
            enemyMovement.DisableCoroutine();
            Vector2 direction = (player.position - transform.position).normalized;

            rb.linearVelocity = direction * enemyMovement.aggroSpeed;
        }
        else
        {
            enemyMovement.EnableCoroutine();
        }
    }


    //ATTACKING STATE
    public void EnemyEnableHitbox()
    {
        atkHitbox.SetActive(true);
    }

    public void EnemyDisableHitbox()
    {
        atkHitbox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (atkProxCol.IsTouching(other) && other.CompareTag("Player"))
        {
            if (timer != null)
            {
                StopCoroutine(timer);
            }
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            Debug.Log(isAttacking);
        }

        if (hitbox.IsTouching(other) && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (atkProxCol.IsTouching(other) && other.CompareTag("Player"))
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!atkProxCol.IsTouching(other) && other.CompareTag("Player"))
        {
            timer = StartCoroutine(WaitForReset());
        }
    }

    private IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(1f);

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
