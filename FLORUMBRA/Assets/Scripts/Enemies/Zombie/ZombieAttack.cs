using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public PlayerControl player;
    public CircleCollider2D circleCollider;
    private float attackCooldown;
    private float damage = 0.15001f;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= 1.5)
        {
            circleCollider.enabled = true;
            attackCooldown = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.hpBar.value = player.hpBar.value - damage;
            circleCollider.enabled = false;
            attackCooldown = 0;
        }

        if (collision.CompareTag("Summon"))
        {
            IDamageable summon = collision.GetComponent<IDamageable>();
            summon.TakeDamage(damage);
            circleCollider.enabled = false;
            attackCooldown = 0;
        }
    }
}
