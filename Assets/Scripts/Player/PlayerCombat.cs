using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private GameObject[] hitbox;
    private Collider2D hitTrigger;
    public int damage = 1;

    public void EnableHitbox(int hitBoxIndex)
    {
        hitTrigger = hitbox[hitBoxIndex].GetComponent<PolygonCollider2D>();
        hitbox[hitBoxIndex].SetActive(true);
    }

    public void DisableHitbox(int hitBoxIndex)
    {
        hitbox[hitBoxIndex].SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitTrigger != null && hitTrigger.IsTouching(other) && other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.ChangeHealth(-damage);
        }
    }
}
