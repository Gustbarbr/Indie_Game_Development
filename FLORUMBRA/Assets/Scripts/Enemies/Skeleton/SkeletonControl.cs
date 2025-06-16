using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonControl : MonoBehaviour
{
    Rigidbody2D rb;
    public Slider hp;

    // Velocidade de movimento
    [SerializeField] float enemyPatrolSpeed = 1.5f;
    [SerializeField] float enemyChaseSpeed = 3;

    // Pontos de patrulha
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    private Transform target;

    // Verificar se player foi detectado
    public bool playerDetected = true;
    PlayerControl player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerControl>();
        transform.position = A.position;
        target = B;
    }

    private void FixedUpdate()
    {
        if (playerDetected == false)
        {
            // Faz com que o inimigo se move de sua posição atual até a posição alvo (A ou B) na velocidade definida
            transform.position = Vector2.MoveTowards(transform.position, target.position, enemyPatrolSpeed * Time.deltaTime);
            // Se a distância dos dois pontos for menor que 0.1, ele faz uma checagem de alvo, se o alvo atual for A troca para B, se for B troca para A
            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                if (target == A) target = B;
                else target = A;
            }

            // O inimigo vira em direção ao ponto que ele está indo, mudando a direção do Detection Range com ele
            if (target.position.x > transform.position.x)
                transform.localScale = new Vector2(1, 1);
            else if (target.position.x < transform.position.x)
                transform.localScale = new Vector2(-1, 1);
        }

        else
        {
            if (player.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }


            // Evita movimentação indesejada, fazendo com que se mova somente quando longe do player
            if (Vector2.Distance(transform.position, player.transform.position) > 0.05f)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * enemyChaseSpeed, rb.velocity.y);
            }
        }
    }

    // Recebe dano da flecha, e se o hp for menor ou igual a 0, morre
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            // Causa dano na barra de vida, levando em conta o dano do player (o quanto o ataque foi carregado) dividido pela defesa (no caso multiplicar por numeros abaixo de 0 funciona como divisao)
            hp.value -= player.arrowCharge * 0.25f;
            // Destruir a flecha
            Destroy(collision.gameObject);
            // Resetar o dano do player
            player.arrowCharge = 0;

            if(hp.value <= 0){
                Destroy(this.gameObject);
            }
        }
    }
}
