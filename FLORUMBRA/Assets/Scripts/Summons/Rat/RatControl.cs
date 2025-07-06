using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatControl : MonoBehaviour, ISummon, IDamageable
{
    // Summonar o rato
    public GameObject ratParent;

    Rigidbody2D rb;

    public PlayerControl player;
    private float moveSpeed = 5;
    private float detectionRange = 20;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);

        GameObject closestEnemy = ChaseEnemy();

        if (closestEnemy != null)
        {
            // Calcula a distancia entre o lobo e o inimigo e o normalized garante que a velocidade de perseguicao sera constante, mesmo com o inimigo se afastando
            Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;

            // Retorna a informacao se na frente do lobo (a uma distancia de 0.1) há um objeto da layer "Barrier"
            RaycastHit2D hitBarrier = Physics2D.Raycast(transform.position, direction, 0.5f, LayerMask.GetMask("Barrier"));

            if (hitBarrier.collider == null && Vector2.Distance(transform.position, closestEnemy.transform.position) >= 1)
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

    public void TakeDamage(float amount){}

    // Parte referente ao controle genérico de summon (ISummon)
    public void OnSummon(Vector3 position)
    {
        ratParent.transform.position = new Vector3(player.transform.position.x, -0.8f);
        ratParent.gameObject.SetActive(true);
    }

    public void OnDismiss()
    {
        ratParent.gameObject.SetActive(false);
    }

    public void OnRessurrect(){}

    public bool IsAlive()
    {
        return true;
    }

    public Slider GetHp()
    {
        return null;
    }
}

