using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int currentHealth;
    [SerializeField] private int maxHealth;

    public void ChangeHealth(int amount){
        currentHealth += amount;

        if(currentHealth <= 0){
            gameObject.SetActive(false);
        }
    }
}
