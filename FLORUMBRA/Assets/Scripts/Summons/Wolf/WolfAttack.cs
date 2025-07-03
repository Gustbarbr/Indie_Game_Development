using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttack : MonoBehaviour
{
    public float attackCooldown;
    public CircleCollider2D attackCollider;
    [SerializeField] private int damage = 15;

    void Update()
    {
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= 1.5)
            attackCollider.enabled = true;
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
                attackCollider.enabled = false;
            }
                

            // Por conta da interface o lobo pode aplicar o sangramento em qualquer inimigo que tenha o ApplyStatus em seu codigo
            IApplyBleed enemyCanBleed = collision.GetComponent<IApplyBleed>();

            if(enemyCanBleed != null)
            {
                // Aplica o dano ao entrar em contato
                enemyCanBleed.TakeDamage(damage);

                // 35% de chance de aplicar sangramento on hit
                int bleedingChance = Random.Range(0, 100);

                if (bleedingChance <= 35 && !enemyCanBleed.WolfApplyBleed)
                    // Aplica 40% do dano do lobo, durante um total de 2.5 segundos e o efeito eh aplicado a cada 0.5 segundos
                    enemyCanBleed.ApplyBleed(damage * 0.4f, 2.5f, 0.5f);

                attackCooldown = 0;
                attackCollider.enabled = false;
            }
        }

    }
}
