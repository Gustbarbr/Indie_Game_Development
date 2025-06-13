using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonControl : MonoBehaviour
{
    Rigidbody2D rb;

    // Velocidade de movimento
    [SerializeField] float enemyPatrolSpeed;
    [SerializeField] float enemyChaseSpeed;

    // Pontos de patrulha
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    private Transform target;

    // Verificar se player foi detectado
    public bool playerDetected = false;

    PlayerControl player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<PlayerControl>();
        transform.position = A.position;
        target = B;
    }

    private void Update()
    {
        if (!playerDetected)
        {
            // Faz com que o inimigo se move de sua posição atual até a posição alvo (A ou B) na velocidade definida
            transform.position = Vector2.MoveTowards(transform.position, target.position, enemyPatrolSpeed * Time.deltaTime);

            // Se a distância dos dois pontos for menor que 0.1, ele faz uma checagem de alvo, se o alvo atual for A troca para B, se for B troca para A
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                if (target == A) target = B;
                else target = A;
            }
        }

        else if (playerDetected) {

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyChaseSpeed * Time.deltaTime);

        }
    }
}
