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
        if (collision.CompareTag("Enemy") && attackCooldown >= 1.5)
        {
            SkeletonControl skeleton = collision.GetComponent<SkeletonControl>();

            // Aplica o dano ao entrar em contato
            skeleton.TakeDamage(damage);

            // 35% de chance de aplicar sangramento on hit
            int bleedingChance = Random.Range(0, 100);

            if (bleedingChance <= 35 && !skeleton.wolfBleed)
                StartCoroutine(ApplyBleeding(skeleton));

            attackCooldown = 0;
        }

    }

    IEnumerator ApplyBleeding(SkeletonControl skeleton)
    {
        // Habilidade de sangramento do lobo
        float bleedDuration = 2.5f; // Duracao
        float bleedInterval = 0.5f; // Intervalo entre instancias de dano
        float bleedElapsedTime = 0; // Tempo decorrido do sangramento

        skeleton.wolfBleed = true;

        // Enquanto a duracao total nao for atingida, o esqueleto toma dano equivalente a 40% do dano do lopo, a cada 0.5 segundos
        while (bleedElapsedTime < bleedDuration && skeleton != null)
        {
            skeleton.TakeDamage(damage * 0.8f);
            Debug.Log("While");
            yield return new WaitForSeconds(bleedInterval);
            bleedElapsedTime += bleedInterval;
        }
        Debug.Log("While'nt");
        skeleton.wolfBleed = false;
    }
}
