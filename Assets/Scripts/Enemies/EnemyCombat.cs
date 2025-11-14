using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 1;

    [SerializeField] private GameObject AtkHitbox;

    private Collider2D hitbox;

    private void Start()
    {
        hitbox = AtkHitbox.GetComponent<PolygonCollider2D>();
    }
    public void EnableHitbox(int hitBoxIndex)
    {
        AtkHitbox.SetActive(true);
    }

    public void DisableHitbox(int hitBoxIndex)
    {
        AtkHitbox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitbox.IsTouching(other) && other.CompareTag("Player"))
        {
            other.GetComponent<EnemyHealth>()?.ChangeHealth(-damage);
        }
    }
}
