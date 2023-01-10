using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayer;
    public TagAttribute enemyTag;

    public int attackDamage = 40;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

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

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        { return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
