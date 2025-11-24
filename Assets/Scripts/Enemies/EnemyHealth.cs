using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public StatsSO baseStats;
    public float currentHealth;

    private SpriteRenderer sr;
    private Animator animator;

    public bool invincible = false;
    public bool isHealing = false;
    public static EnemyHealth instance;
    private EnemyCombat enemyCombat;
    private DamageFlash damageFlash;
    private FloatingHealthBar floatingHealthBar;
    public GameObject healthBar;
    private Coroutine healthCoroutine;

    private void Start()
    {
        currentHealth = baseStats.maxHealth;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemyCombat = GetComponent<EnemyCombat>();
        damageFlash = GetComponent<DamageFlash>();
        floatingHealthBar = GetComponent<FloatingHealthBar>();
        instance = this;
    }
    public void ChangeHealth(float amount)
    {
        if (!invincible || isHealing)
        {
            currentHealth += amount;
            damageFlash.CallDamageFlash();
            healthBar.SetActive(true);

            if (healthCoroutine != null)
            {
                StopCoroutine(healthCoroutine);
            }
            healthCoroutine = StartCoroutine(HealthBar());
            floatingHealthBar.UpdateHealthBar(currentHealth, baseStats.maxHealth);
        }

        if (currentHealth > baseStats.maxHealth)
        {
            currentHealth = baseStats.maxHealth;
        }
        else if (currentHealth <= 0)
        {
            animator.Play("Death");
        }

        if (amount < 0 && currentHealth > 0)
        {
            animator.Play("Stagger");
            invincible = true;
            enemyCombat.isStaggered = true;
        }
    }

    IEnumerator HealthBar()
    {
        healthBar.SetActive(true);
        yield return new WaitForSeconds(5f);
        healthBar.SetActive(false);
    }
}
