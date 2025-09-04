using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonControl : MonoBehaviour, IDamageable
{
    Rigidbody2D rb;
    public Slider hp;
    PlayerControl player;
    public GameObject hudHpBar;
    SkeletonLoot loot;

    // Velocidade de movimento
    private float enemyPatrolSpeed = 1.5f;
    private float enemyChaseSpeed = 3;

    private float defense = 0.25f;
    private Vector2 initialPosition;

    // Pontos de patrulha
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    private Transform target;

    // Verificar se player foi detectado
    public bool playerDetected = false;
    public bool summonDetected = false;

    // Armazenar cada summon para checar se estão mais próximos que o player
    GameObject closestSummon;

    // Itens que podem ser dropados

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerControl>();
        loot = GetComponent<SkeletonLoot>();
        transform.position = A.position;
        initialPosition = transform.position;
        target = B;
    }

    private void FixedUpdate()
    {
        SkeletonMovement();
    }

    void SkeletonMovement()
    {
        TargetSummon();

        bool summonIsActive = closestSummon != null && closestSummon.activeInHierarchy; // Essa variável só é verdadeira se existir um summon e ele estiver ativo
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        float distanceToSummon = summonIsActive ? Vector2.Distance(transform.position, closestSummon.transform.position) : Mathf.Infinity;

        // Se a distância até o summon for menor que a distância até o player, o alvo será o summon, senão será o player
        GameObject currentTarget = (distanceToSummon < distanceToPlayer) ? closestSummon : player.gameObject;

        // Se o player se afastar muito do inimigo, ele deixa de perseguir
        if (Vector2.Distance(transform.position, player.transform.position) > 10)
            playerDetected = false;
        // Se o summon se afastar muito do inimigo, ele deixa de perseguir
        if (summonIsActive && Vector2.Distance(transform.position, closestSummon.transform.position) > 10)
            summonDetected = false;

        if (playerDetected == false && summonDetected == false)
        {
            SkeletonPatrol();
            return;
        }

        // Vira para o alvo
        if (currentTarget.transform.position.x > transform.position.x)
            transform.localScale = new Vector2(1, 1);
        else
            transform.localScale = new Vector2(-1, 1);


        // Evita movimentação indesejada se estiver muito perto
        if(Vector2.Distance(transform.position, currentTarget.transform.position) > 2)
        {
            Vector2 direction = (currentTarget.transform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * enemyChaseSpeed, rb.velocity.y);
        }

        else
            rb.velocity = Vector2.zero;
    }

    private void SkeletonPatrol()
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

    public void TakeDamage(float amount)
    {
        hp.value -= amount;

        if (hp.value <= 0)
        {
            player.xp += 10 + 2 * player.level;

            hudHpBar.SetActive(false);

            int natureSoulQuatity = Random.Range(5, 15);

            player.natureSoul += natureSoulQuatity;
            player.natureSoulAmount.SetText(player.natureSoul.ToString());

            // O objeto responsavel pelo ataque eh o segundo filho
            Transform attack = transform.GetChild(2);
            attack.gameObject.SetActive(false); // Desativa o segundo filho

            CapsuleCollider2D body = GetComponent<CapsuleCollider2D>();
            body.enabled = false; // Desativa o colisor

            this.tag = "Defeated"; // Impede que summons detectem

            this.enabled = false; // Desativa esse componente
        }
    }

    // Recebe dano da flecha, e se o hp for menor ou igual a 0, morre
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arrow"))
        {
            // Causa dano na barra de vida, levando em conta o dano do player (o quanto o ataque foi carregado) dividido pela defesa (no caso multiplicar por numeros abaixo de 0 funciona como divisao)
            float arrowDamage = player.finalDamage * defense;
            TakeDamage(arrowDamage);

            Destroy(collision.gameObject);
            player.arrowCharge = 0;
        }
    }

    // Faz com que o summon ativo seja o único considerado como summon ativo,
    // ou seja, mesmo ao trocar de summon ativo, o inimigo ainda o atacará
    void TargetSummon()
    {
        // Busca todos os objetos que possuam a tag "Summon"
        GameObject[] summons = GameObject.FindGameObjectsWithTag("Summon");

        // Garante que nenhum summon está selecionado
        closestSummon = null;

        // A distancia inicial é o infinito, ou seja, qualquer distancia menor que essa será considerada como mais proxima
        float minDistance = Mathf.Infinity;

        // Percorra todos os summons
        foreach (GameObject s in summons)
        {
            // Só considera o summon ativo
            if (s.activeInHierarchy)
            {
                // Calcula a distancia entre o inimigo e o summon
                float distance = Vector2.Distance(transform.position, s.transform.position);

                // Se a distancia entre ambos for menor que infinito (sempre será),
                // a nova distancia minima sera a distancia entre inimigo e summon e o summon mais proximo sera o ativo
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestSummon = s;
                }
            }
        }
    }

    public void ResetEnemy()
    {
        transform.position = initialPosition;
        rb.velocity = Vector2.zero;
        target = B;
        playerDetected = false;
        summonDetected = false;
        closestSummon = null;
        hp.value = hp.maxValue;
        hudHpBar.SetActive(true);

        Transform attack = transform.GetChild(2);
        attack.gameObject.SetActive(true);

        CapsuleCollider2D body = GetComponent<CapsuleCollider2D>();
        body.enabled = true;
        this.tag = "Enemy";
        this.enabled = true;
        loot.ResetLootDrop();
    }
}
