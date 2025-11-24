using UnityEngine;

public class AggroBehavior : MonoBehaviour
{

    public EnemyMovement enemyMovement;

    private Transform player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyMovement = GetComponent<EnemyMovement>();
    }
    public void Chase(Rigidbody2D rb, bool isStaggered, bool isAttacking)
    {
        if (!isStaggered)
        {
            if (enemyMovement.isAggro && !isAttacking)
            {
                enemyMovement.DisableCoroutine();
                Vector2 direction = (player.position - transform.position).normalized;
                enemyMovement.Flip(direction);

                rb.linearVelocity = direction * enemyMovement.aggroSpeed;
            }
            else
            {
                enemyMovement.EnableCoroutine();
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
