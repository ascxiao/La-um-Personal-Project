using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    public int currentHealth;

    private SpriteRenderer sr;
    private Animator animator;

    public bool invincible = false;
    public bool isHealing = false;
    public static EnemyHealth instance;
    private EnemyCombat enemyCombat;
    private DamageFlash damageFlash;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemyCombat = GetComponent<EnemyCombat>();
        damageFlash = GetComponent<DamageFlash>();
        instance = this;
    }
    public void ChangeHealth(int amount)
    {
        if (!invincible || isHealing)
        {
            currentHealth += amount;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
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
            damageFlash.CallDamageFlash();
        }
    }
}
