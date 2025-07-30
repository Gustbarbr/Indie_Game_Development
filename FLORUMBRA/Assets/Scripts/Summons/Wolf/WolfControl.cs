using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WolfControl : MonoBehaviour, IApplyPoison, IDamageable, ISummon
{
    public Slider hp;

    // Summonar o lobo
    public GameObject wolfParent;

    public PlayerControl player;
    private float moveSpeed = 5;
    private float detectionRange = 20;
    private Rigidbody2D rb;

    // Ficar perto do player patrulhando
    private Vector3 patrolTarget;
    private bool movingRight = true;
    private float maxDistanceFromPlayer = 25;

    // Efeitos dos inimigos
    public bool HitApplyPoison { get; set; }

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

            if(hitBarrier.collider == null && Vector2.Distance(transform.position, closestEnemy.transform.position) >= 1.5f)
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
            FollowPlayer();
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
            float distanceToWolf = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToWolf <= detectionRange && distanceToWolf < minDistance)
            {
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                RaycastHit2D wall = Physics2D.Raycast(transform.position, direction, distanceToEnemy, LayerMask.GetMask("Barrier"));
                if (wall.collider == null)
                {
                    minDistance = distanceToWolf;
                    closest = enemy;
                }
            }
        }

        return closest;
    }

    // Quando não há inimigos por perto o lobo seguirá o player
    void FollowPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        if(Vector2.Distance(player.transform.position, transform.position) >= 2.5f)
        {
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);

            if (direction.x > 0)
                transform.localScale = new Vector2(1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector2(-1, 1);
        }

        else
            rb.velocity = Vector2.zero;
    }

    public void TakeDamage(float amount)
    {
        hp.value -= amount;

        if (hp.value <= 0)
        {
            wolfParent.gameObject.SetActive(false);
            player.isSummoned = false;
        }
    }

    public void ApplyPoison(float poisonDamage, float poisonDuration, float poisonInterval)
    {
        // Se o inimigo não estiver envenenado, pode aplicar o envenenamento
        if (!HitApplyPoison)
            StartCoroutine(Poison(poisonDamage, poisonDuration, poisonInterval));
    }

    IEnumerator Poison(float poisonDamage, float poisonDuration, float poisonInterval)
    {
        HitApplyPoison = true;
        float elapsedPoisonTime = 0;

        // Enquanto a duracao total nao for atingida, o inimigo toma dano equivalente ao sangramento (o valor do sangramento está no wolf attack)
        while (elapsedPoisonTime < poisonDuration)
        {
            TakeDamage(poisonDamage);
            yield return new WaitForSeconds(poisonInterval);
            elapsedPoisonTime += poisonInterval;
        }

        HitApplyPoison = false;
    }

    // Parte referente ao controle genérico de summon (ISummon)
    public void OnSummon(Vector3 position)
    {
        if(player.mana >= 35)
        {
            wolfParent.transform.position = position;
            player.mana -= 35f;
            player.isSummoned = true;
            wolfParent.gameObject.SetActive(true);
        }
    }

    public void OnDismiss()
    {
        player.isSummoned = false;
        wolfParent.gameObject.SetActive(false);
    }

    public void OnRessurrect()
    {
        hp.value = 150;
    }

    public bool IsAlive()
    {
        return hp.value > 0;
    }

    public Slider GetHp()
    {
        return hp;
    }
}
