using UnityEngine;

public class CollisionDebugger : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered with " + other.gameObject.name);
    }
}
