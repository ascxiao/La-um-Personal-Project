using UnityEngine;
using System.Collections;
using System;


public class EnemyCombat : MonoBehaviour
{
    public StatsSO baseStats;

    private float atk;
    private float critR;
    private float critD;
    private PlayerStats targetStats;
    private Transform player;

    [SerializeField] private GameObject atkHitbox;
    [SerializeField] private GameObject atkProx;

    private Collider2D hitbox;
    private Collider2D atkProxCol;
    public Rigidbody2D rb;
    private Animator animator;
    private Coroutine timer;
    public bool isAttacking = false;
    public bool isStaggered = false;

    public EnemyMovement enemyMovement;
    public static EnemyCombat instance;
    private System.Random rng = new System.Random();


    private void Start()
    {
        atk = baseStats.baseAtk;
        critR = baseStats.baseCritR;
        critD = baseStats.baseCritD;

        hitbox = atkHitbox.GetComponent<Collider2D>();
        atkProxCol = atkProx.GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        instance = this;
    }


    private void Update()
    {
        Chase();
    }

    //IMPROVE PATHFINDING ALGORITHM, I SUGGEST TRANSFERING THE SCRIPT TO A SEPARATE ONE

    private void Chase()
    {
        if (!isStaggered)
        {
            if (enemyMovement.isAggro && !isAttacking)
            {
                enemyMovement.DisableCoroutine();
                Vector2 direction = (player.position - transform.position).normalized;
                enemyMovement.Flip(direction);

                rb.linearVelocity = direction * enemyMovement.aggroSpeed;
            }
            else
            {
                enemyMovement.EnableCoroutine();
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
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
            rb.linearVelocity = Vector2.zero;
        }

        if (hitbox.IsTouching(other) && other.CompareTag("Player"))
        {
            targetStats = other.GetComponent<PlayerStats>();
            other.GetComponent<PlayerStats>()?.ChangeHealth(-DamageCalculator());
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


    private float DamageCalculator()
    {
        float damage = atk - targetStats.def + (rng.Next(-1, 5) * 0.10f);

        if (damage < 0)
        {
            damage = 1;
        }

        if (rng.Next(1, 100) <= critR)
        {
            damage *= critD / 100;
        }

        Debug.Log(damage);
        return damage;
    }

    private IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
