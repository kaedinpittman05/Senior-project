using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Variables for the animator, enemy layer, as well as the attack point, range, and damage
    public Animator animator;

    public LayerMask enemyLayer;
    public Transform attackPoint;
    public float attackRange;
    public int attackDamage = 40;

    // Update is called once per frame
    void Update()
    {
        // If the left mouse button is clicked then the attack method is called
        if (Input.GetKeyDown("space"))
        {
            Attack();
        }
    }

    // Gets all the enemies that are in the enemy layer and then it makes each enemy in the attack range take damage
    void Attack()
    {
        FindObjectOfType<AudioManager>().Play("Attack");
        animator.SetTrigger("Attack");

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in enemiesHit)
        {
            enemy.GetComponent<EnemyDie>().TakeDamage(attackDamage);
        }
        
    }

    // Draws a circle for the attack range
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        { return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
