using UnityEngine;
using System.Collections;

public class IsometricDepth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polyCollider;
    private int baseOrder;
    private Color c;
    public bool shouldFade = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseOrder = spriteRenderer.sortingOrder;

        polyCollider = GetComponent<PolygonCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer playerSprite = other.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                spriteRenderer.sortingOrder = playerSprite.sortingOrder + 1;

                if (shouldFade){
                    StartCoroutine(FadeTo(0.5f, 0.25f));
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer playerSprite = other.GetComponent<SpriteRenderer>();
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
