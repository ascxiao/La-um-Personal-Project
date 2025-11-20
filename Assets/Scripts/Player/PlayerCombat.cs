using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private GameObject[] hitbox;
    [SerializeField] private GameObject particleFX;
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
            Vector3 spawnPos = other.transform.position;

            ParticleFx(spawnPos);
            other.GetComponent<EnemyHealth>()?.ChangeHealth(-damage);


        }
    }

    void ParticleFx(Vector3 spawnPos)
    {
        GameObject fx = Instantiate(particleFX, spawnPos, Quaternion.identity);
        ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(fx, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(fx, 0.5f);
        }
    }
}
