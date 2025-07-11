using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MonoBehaviour
{
    BatControl bat;
    PlayerControl player;

    public float attackCooldown;
    public CircleCollider2D attackCollider;
    [SerializeField] private int damage = 10;
    public int numberOfAttacks = 5;

    void Start()
    {
        bat = GetComponentInParent<BatControl>();
        player = FindObjectOfType<PlayerControl>();
    }

    void Update()
    {
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= 1.5)
            attackCollider.enabled = true;

        if (numberOfAttacks <= 0)
        {
            bat.batParent.gameObject.SetActive(false);
            player.isSummoned = false;
        }
            
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {

            IDamageable enemy = collision.GetComponent<IDamageable>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                attackCooldown = 0;
                numberOfAttacks -= 1;
                attackCollider.enabled = false;
            }

            IApplyBleed enemyCanBleed = collision.GetComponent<IApplyBleed>();

            if (enemyCanBleed != null)
            {
                enemyCanBleed.TakeDamage(damage);

                int bleedingChance = Random.Range(0, 100);

                if (bleedingChance <= 75 && !enemyCanBleed.SummonApplyBleed)
                    enemyCanBleed.ApplyBleed(damage * 1.5f, 2, 0.5f);

                attackCooldown = 0;
                attackCollider.enabled = false;
            }
        }

    }
}
