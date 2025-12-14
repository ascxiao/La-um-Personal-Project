using UnityEngine;
using System.Collections;
using System;

public class EnemyCombat : MonoBehaviour
{
    public StatsSO baseStats;

    private float atk;
    public float def;
    private float critR;
    private float critD;
    private float luck;
    [SerializeField] private float[] atkPower;
    private int atkIndex = 0;
    private PlayerStats playerStats;
    private CombatManager cm;
    private AggroBehavior ab;

    [SerializeField] private GameObject atkHitbox;
    [SerializeField] private GameObject atkProx;

    private Collider2D hitbox;
    private Collider2D atkProxCol;
    public Rigidbody2D rb;
    private Animator animator;
    private Coroutine timer;
    public bool isAttacking = false;
    public bool isStaggered = false;
    public static EnemyCombat instance;


    private void Start()
    {
        atk = baseStats.baseAtk;
        def = baseStats.baseDef;
        critR = baseStats.baseCritR;
        critD = baseStats.baseCritD;
        luck = baseStats.baseLuck;

        hitbox = atkHitbox.GetComponent<Collider2D>();
        atkProxCol = atkProx.GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cm = GetComponent<CombatManager>();
        ab = GetComponent<AggroBehavior>();

        instance = this;
    }


    private void Update()
    {
        ab.Chase(rb, isStaggered, isAttacking);
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
            playerStats = other.GetComponentInParent<PlayerStats>();
            float currentPower = atkPower[atkIndex];
            other.GetComponent<PlayerStats>()?.ChangeHealth(-cm.DamageCalculator(atk, playerStats.def, critR, critD, luck, currentPower));

            atkIndex++;
            if (atkIndex >= atkPower.Length)
                atkIndex = 0;
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
        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }
}
