using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private GameObject[] hitbox;
    [SerializeField] private GameObject particleFX;
    [SerializeField] private float[] atkPower;
    private int atkIndex = 0;
    private EnemyCombat enemyStats;
    private CombatManager cm;
    private PlayerStats ps;
    private Collider2D hitTrigger;
    public int damage = 1;

    private void Start()
    {
        cm = GetComponent<CombatManager>();
        ps = GetComponent<PlayerStats>();
    }
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
        enemyStats = other.GetComponent<EnemyCombat>();
        if (hitTrigger != null && hitTrigger.IsTouching(other) && other.CompareTag("Enemy"))
        {
            Vector3 spawnPos = other.transform.position;

            ParticleFx(spawnPos);
            float currentPower = atkPower[atkIndex];
            other.GetComponent<EnemyHealth>()?.ChangeHealth(-cm.DamageCalculator(ps.atk, enemyStats.def, ps.critR, ps.critD, ps.luck, currentPower));

            atkIndex++;
            if (atkIndex >= atkPower.Length)
                atkIndex = 0;
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
