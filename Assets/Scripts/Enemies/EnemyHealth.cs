using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    public int currentHealth;

    private SpriteRenderer sr;
    private Animator animator;

    public bool invincible = false;
    public bool isDamaged = false;
    public bool isHealing = false;

    public static EnemyHealth instance;
    public EnemyCombat enemyCombat;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        enemyCombat = GetComponent<EnemyCombat>();
        instance = this;
    }
    private void Update()
    {
        Damage();
    }
    public void ChangeHealth(int amount)
    {
        if (!invincible || isHealing)
        {
            currentHealth += amount;
        }

        if (amount < 0)
        {
            animator.Play("Stagger");
            invincible = true;
            isDamaged = true;
            enemyCombat.isStaggered = true;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Damage()
    {
        if (isDamaged)
        {
            Color c = new Color(1f, 0f, 0f);
            sr.color = c;
        }
        else
        {
            Color c = new Color(1f, 1f, 1f);
            sr.color = c;
        }
    }
}
