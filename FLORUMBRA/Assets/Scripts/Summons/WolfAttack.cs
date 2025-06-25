using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttack : MonoBehaviour
{
    public float attackCooldown;
    [SerializeField] private float damage = 0.2f;

    void Update()
    {
        attackCooldown += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && attackCooldown >= 2.5)
        {
            // Por conta da interface o lobo pode aplicar o sangramento em qualquer inimigo que tenha o ApplyStatus em seu codigo
            IApplyStatus enemy = collision.GetComponent<IApplyStatus>();

            if(enemy!= null)
            {
                // Aplica o dano ao entrar em contato
                enemy.TakeDamage(damage);

                // 35% de chance de aplicar sangramento on hit
                int bleedingChance = Random.Range(0, 100);

                if (bleedingChance <= 35 && !enemy.WolfApplyBleed)
                    // Aplica 40% do dano do lobo, durante um total de 2.5 segundos e o efeito eh aplicado a cada 0.5 segundos
                    enemy.ApplyBleed(damage * 0.4f, 2.5f, 0.5f);

                attackCooldown = 0;
            }
        }

    }
}
