using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public PlayerControl player;
    public CircleCollider2D circleCollider;
    private float attackCooldown;
    private int damage = 15;
    public int poisonMeter = 0;

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
        IApplyPoison poisonEnemy = collision.GetComponent<IApplyPoison>();

        if (poisonEnemy != null)
        {
            poisonEnemy.TakeDamage(damage);

            poisonMeter += 40;

            if (poisonMeter >= 100)
            {
                poisonMeter = 0;
                poisonEnemy.ApplyPoison(0.2f, 5, 1);
            }

            attackCooldown = 0;
            circleCollider.enabled = false;
        }
    }
}
