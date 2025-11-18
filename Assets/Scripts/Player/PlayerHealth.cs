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

    private void Awake()
    {
        damageFlash = GetComponent<DamageFlash>();
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
            iframe = StartCoroutine(IFrameTrigger());
        }
    }

    private IEnumerator IFrameTrigger()
    {
        yield return new WaitForSeconds(iframes);
        invincible = false;
    }
}
