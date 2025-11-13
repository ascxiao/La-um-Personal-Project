using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        other.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        Debug.Log("This is working" + other.gameObject);
    }
}
