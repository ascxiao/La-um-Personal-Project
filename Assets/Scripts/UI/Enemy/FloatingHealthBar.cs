using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] public Slider healthSlider;
    [SerializeField] private Slider damageVis;
    private float damage;
    private float currentSlider;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        currentSlider = healthSlider.value;

        damage = currentValue / maxValue;
        healthSlider.value = damage;

        DamageVis(damage);
    }

    public void DamageVis(float damage)
    {
        StartCoroutine(Damage(damage));
    }

    private IEnumerator Damage(float damage)
    {
        float elapsed = 0f;
        while (elapsed < 1.5f)
        {
            elapsed += Time.deltaTime;
            damageVis.value = Mathf.Lerp(currentSlider, damage, elapsed / 1.5f);
            yield return null;
        }
    }
}
