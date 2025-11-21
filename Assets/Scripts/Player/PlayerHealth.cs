using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    [SerializeField] private float iframes = 1.5f;
    public bool invincible = false;
    private Coroutine iframe;
    private DamageFlash damageFlash;
    private FloatingHealthBar floatingHealthBar;

    private void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
        floatingHealthBar = GetComponent<FloatingHealthBar>();
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
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
