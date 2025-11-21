using UnityEngine;
using System.Collections;

public class IsometricDepth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject objectBounds;
    private Collider2D col;
    private int baseOrder;
    private Color c;
    public bool shouldFade = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseOrder = spriteRenderer.sortingOrder;
        if (objectBounds == null) Debug.LogWarning("objectBounds is null on " + gameObject.name);
        col = objectBounds.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            SpriteRenderer otherSprite = other.GetComponent<SpriteRenderer>();
            if (col.IsTouching(other) && otherSprite != null)
            {
                if (other.CompareTag("Player") || other.CompareTag("Enemy"))
                {
                    spriteRenderer.sortingOrder = otherSprite.sortingOrder + 1;
                }
                else if (other.CompareTag("Enemy") && gameObject.CompareTag("Small Foliage"))
                {
                    spriteRenderer.sortingOrder = otherSprite.sortingOrder - 1;
                }

                if (shouldFade && other.CompareTag("Player"))
                {
                    StartCoroutine(FadeTo(0.5f, 0.25f));
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (!col.IsTouching(other) && (other.CompareTag("Player") || other.CompareTag("Enemy")))
        {
            spriteRenderer.sortingOrder = baseOrder;
            StartCoroutine(FadeTo(1f, 0.25f));
        }
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        Color c = spriteRenderer.color;
        float startAlpha = c.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            spriteRenderer.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        spriteRenderer.color = c;
    }
}
