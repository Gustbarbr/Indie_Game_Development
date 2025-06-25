using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonControl : MonoBehaviour, IApplyStatus
{
    Rigidbody2D rb;
    public Slider hp;

    // Velocidade de movimento
    [SerializeField] float enemyPatrolSpeed = 1.5f;
    [SerializeField] float enemyChaseSpeed = 3;

    [SerializeField] float defense = 0.25f;

    // Pontos de patrulha
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    private Transform target;

    // Verificar se player foi detectado
    public bool playerDetected = true;
    PlayerControl player;

    // Habilidades dos summons
    public bool wolfBleed = false;
    public bool WolfApplyBleed { get; set; } // Eh necessario incluir essa variavel pois ela tambem esta no ApplyStatus

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerControl>();
        transform.position = A.position;
        target = B;
    }

    private void FixedUpdate()
    {
        SkeletonMovement();

        // Se o player se afastar muito do inimigo, ele deixa de perseguir
        if(Vector2.Distance(transform.position, player.transform.position) > 15)
            playerDetected = false;
    }

    void SkeletonMovement()
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
                transform.localScale = new Vector2(1, 1);
            else if (player.transform.position.x < transform.position.x)
                transform.localScale = new Vector2(-1, 1);


            // Evita movimentação indesejada, fazendo com que se mova somente quando longe do player
            if (Vector2.Distance(transform.position, player.transform.position) > 2)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                rb.velocity = new Vector2(direction.x * enemyChaseSpeed, rb.velocity.y);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        hp.value -= amount;

        if (hp.value <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Recebe dano da flecha, e se o hp for menor ou igual a 0, morre
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            // Causa dano na barra de vida, levando em conta o dano do player (o quanto o ataque foi carregado) dividido pela defesa (no caso multiplicar por numeros abaixo de 0 funciona como divisao)
            float arrowDamage = player.arrowCharge * defense;
            TakeDamage(arrowDamage);

            Destroy(collision.gameObject);
            player.arrowCharge = 0;
        }
    }

    public void ApplyBleed(float bleedDamage, float bleedDuration, float bleedInterval)
    {
        // Se o inimigo não estiver sangrando, pode aplicar o sangramento
        if (!WolfApplyBleed)
            StartCoroutine(Bleeding(bleedDamage, bleedDuration, bleedInterval));
    }

    IEnumerator Bleeding(float bleedDamage, float bleedDuration, float bleedInterval)
    {
        WolfApplyBleed = true;
        float elapsedBleedTime = 0;

        // Enquanto a duracao total nao for atingida, o inimigo toma dano equivalente ao sangramento (o valor do sangramento está no wolf attack)
        while (elapsedBleedTime < bleedDuration) {
            TakeDamage(bleedDamage);
            yield return new WaitForSeconds(bleedInterval);
            elapsedBleedTime += bleedInterval;
        }

        WolfApplyBleed = false;
    }
}
