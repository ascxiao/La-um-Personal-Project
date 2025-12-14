using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public StatsSO baseStats;

    private float currentHealth;
    private float maxHealth;
    public float def;
    public float atk;
    public float spd;
    public float critR;
    public float critD;
    public float luck;

    [SerializeField] private float iframes = 1.5f;
    public bool invincible = false;

    private Coroutine iframe;
    private DamageFlash damageFlash;
    private FloatingHealthBar floatingHealthBar;

    private void Awake()
    {
        maxHealth = baseStats.maxHealth;
        def = baseStats.baseDef;
        atk = baseStats.baseDef;
        spd = baseStats.baseSpeed;
        critR = baseStats.baseCritR;
        critD = baseStats.baseCritD;
        luck = baseStats.baseLuck;

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
            floatingHealthBar.UpdateHealthBar(currentHealth, maxHealth);
            iframe = StartCoroutine(IFrameTrigger());
            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator IFrameTrigger()
    {
        yield return new WaitForSeconds(iframes);
        invincible = false;
    }
}
