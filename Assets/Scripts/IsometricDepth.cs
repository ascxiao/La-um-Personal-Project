using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class IsometricDepth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public Rigidbody2D player;
    public SpriteRenderer playerSprite;
    private int spriteOrder;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOrder = spriteRenderer.sortingOrder;
    }

    void Update()
    {
        if (player.position.y > rb.position.y){
        spriteRenderer.sortingOrder = playerSprite.sortingOrder + 1;
        } else {
            spriteRenderer.sortingOrder = spriteOrder;
        }
    }
}
