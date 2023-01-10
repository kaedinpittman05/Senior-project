using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public GameObject enemy;
    public int maxHealth = 100;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("SlimeHurt");
        if (currentHealth <= 0)
        {
            Die();
        
        }
    }

    void Die()
    {
        Destroy(enemy);
        FindObjectOfType<AudioManager>().Play("Slime Death");
    }
}
