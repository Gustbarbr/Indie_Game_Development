using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonControl : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float enemyPatrolMovespeed;
    [SerializeField] float enemyChaseMovespeed;

    [SerializeField] Transform A;
    [SerializeField] Transform B;

    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = A.position;
        target = B; // Começa indo para o ponto B
    }

    void Update()
    {
        // Move o inimigo em direção ao ponto-alvo
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemyPatrolMovespeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            // Seria um if traget == A, target = B, ou seja se estiver próximo do A, troca o target para B
            target = (target == A) ? B : A;

        }
    }
}
