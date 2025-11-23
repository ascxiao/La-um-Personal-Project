using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public StatsSO baseStats;

    private float currentHealth;
    private float maxHealth;
    public float def;

    [SerializeField] private float iframes = 1.5f;
    public bool invincible = false;

    private Coroutine iframe;
    private DamageFlash damageFlash;
    private FloatingHealthBar floatingHealthBar;

    private void Awake()
    {
        maxHealth = baseStats.maxHealth;
        def = baseStats.baseDef;
        damageFlash = GetComponent<DamageFlash>();
        floatingHealthBar = GetComponent<FloatingHealthBar>();
        currentHealth = maxHealth;
    }

    public void ChangeHealth(float amount)
    {
        if (!invincible)
        {
            if (amount < 0)
            {
                damageFlash.CallDamageFlash();
            }
            invincible = true;
            currentHealth += amount;

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
            floatingHealthBar.UpdateHealthBar(currentHealth, maxHealth);
            iframe = StartCoroutine(IFrameTrigger());
        }
    }

    private IEnumerator IFrameTrigger()
    {
        yield return new WaitForSeconds(iframes);
        invincible = false;
    }
}
