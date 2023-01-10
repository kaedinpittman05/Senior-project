using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    // Variables for the enemy object itself, its health, and then its current health
    public GameObject enemy;
    public int maxHealth = 100;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        // Sets health
        currentHealth = maxHealth;
    }

    // Enemy damge and checks if health is less than or equal to 0 and then call the Die() method
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManager>().Play("SlimeHurt");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Gets rid of the enemy from the game
    void Die()
    {
        Destroy(enemy);
        FindObjectOfType<AudioManager>().Play("Slime Death");
    }
}
