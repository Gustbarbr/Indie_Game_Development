using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WolfControl : MonoBehaviour
{

    public PlayerControl player;
    private float moveSpeed = 5;
    private float detectionRange = 10;
    private Rigidbody2D rb;

    // Ficar perto do player patrulhando
    private Vector3 patrolTarget;
    private bool movingRight = true;
    private float maxDistanceFromPlayer = 15;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);

        // O lobo é mandado para casa caso se afaste muito do player
        if(distanceFromPlayer > maxDistanceFromPlayer)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }

        GameObject closestEnemy = ChaseEnemy();

        if(closestEnemy != null)
        {
            // Calcula a distancia entre o lobo e o inimigo e o normalized garante que a velocidade de perseguicao sera constante, mesmo com o inimigo se afastando
            Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;

            // Retorna a informacao se na frente do lobo (a uma distancia de 0.1) há um objeto da layer "Barrier"
            RaycastHit2D hitBarrier = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Barrier"));

            if(hitBarrier.collider == null && Vector2.Distance(transform.position, closestEnemy.transform.position) >= 2)
                // Atualiza a velocidade do lobo com base na direção e movespeed
                rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
            else
                rb.velocity = Vector2.zero;

            // Mudar para onde o lobo olha
            if (direction.x > 0)
                transform.localScale = new Vector2(1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector2(-1, 1);
        }

        else
        {
            ProtectPlayer();
        }
        
    }

    void OnEnable()
    {
        transform.position = player.transform.position;
        movingRight = true;
    }

    GameObject ChaseEnemy()
    {
        // Armazena objetos com a tag "enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Define a principio que o inimigo mais proximo eh nulo, deixando a variavel limpa
        GameObject closest = null;

        // Define a distancia minima como infinito, um valor absurdamente alto,
        // então logo na primeira comparação o primeiro inimigo, por ter uma distancia real menor que infinito sera definido como mais proximo
        float minDistance = Mathf.Infinity;

        // Para cada objeto com a tag inimigo, será feita uma checagem, onde o inimigo mais proximo do player sera definido como "closest"
        // e o lobo perseguira o mais proximo
        foreach (GameObject enemy in enemies)
        {
            float distanceToPlayer = Vector2.Distance(player.transform.position, enemy.transform.position);

            if (distanceToPlayer <= detectionRange && distanceToPlayer < minDistance)
            {
                minDistance = distanceToPlayer;
                closest = enemy;
            }
        }

        return closest;
    }

    // Quando não há inimigos por perto o lobo ficará rondando o player
    void ProtectPlayer()
    {
        if (movingRight)
        {
            patrolTarget = player.transform.position + new Vector3(5, 0, 0);
        }

        else
        {
            patrolTarget = player.transform.position + new Vector3(-5, 0, 0);
        }

        // Calcula a direcao horizontal que o lobo deve seguir ate chegar ao ponto final de patrulha
        Vector2 direction = new Vector2(patrolTarget.x - transform.position.x, 0).normalized;

        // Faz com que o raycast fique um pouco a frente do lobo, para que ele nao detecte o proprio colisor
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;

        // Checa se na frente do player (distancia de 0.5) há uma layer de nome "Barrier", o que retornaria hitBarrier diferente de null
        RaycastHit2D hitBarrier = Physics2D.Raycast(origin, direction, 0.5f, LayerMask.GetMask("Barrier"));

        if (hitBarrier.collider == null)
            transform.position = Vector2.MoveTowards(transform.position, patrolTarget, moveSpeed * Time.deltaTime);

        else
            rb.velocity = Vector2.zero;

        // Mudar para onde o lobo olha
        if (direction.x > 0)
            transform.localScale = new Vector2(1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector2(-1, 1);

        if (Vector2.Distance(transform.position, patrolTarget) <= 0.1f)
            movingRight = !movingRight;
    }
}
