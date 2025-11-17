using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    [SerializeField] private float iframes = 1f;
    public bool invincible = false;
    private Coroutine iframe;

    public void ChangeHealth(int amount)
    {

        if (!invincible)
        {
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
