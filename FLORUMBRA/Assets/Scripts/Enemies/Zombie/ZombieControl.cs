using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZombieControl : MonoBehaviour, IApplyBleed, IApplyPoison
{
    Rigidbody2D rb;
    public Slider hp;

    // Velocidade de movimento
    private float enemyPatrolSpeed = 0.5f;
    private float enemyChaseSpeed = 4.5f;

    [SerializeField] float defense = 0.35f;

    // Pontos de patrulha
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    private Transform target;

    // Verificar se player foi detectado
    public bool playerDetected = false;
    PlayerControl player;

    // Armazenar cada summon para checar se est�o mais pr�ximos que o player
    GameObject closestSummon;

    // Habilidades dos summons
    public bool WolfApplyBleed { get; set; } // Eh necessario incluir essa variavel pois ela tambem esta no ApplyStatus

    // Habilidades do player
    public bool PlayerApplyPoison { get; set; }
    public int poisonMeter = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerControl>();
        transform.position = A.position;
        target = B;
    }

    private void FixedUpdate()
    {
        ZombieMovement();

        // Se o player se afastar muito do inimigo, ele deixa de perseguir
        if (Vector2.Distance(transform.position, player.transform.position) > 10)
            playerDetected = false;
    }

    void ZombieMovement()
    {
        TargetSummon();

        bool summonIsActive = closestSummon != null && closestSummon.activeInHierarchy; // Essa vari�vel s� � verdadeira se existir um summon e ele estiver ativo
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        float distanceToSummon = 0;

        if (summonIsActive)
            distanceToSummon = Vector2.Distance(transform.position, closestSummon.transform.position);
        else
            distanceToSummon = Mathf.Infinity;


        if (playerDetected == false)
        {
            ZombiePatrol();
            return;
        }

        // Se a dist�ncia at� o summon for menor que a dist�ncia at� o player, o alvo ser� o summon, sen�o ser� o player
        GameObject currentTarget = (distanceToSummon < distanceToPlayer) ? closestSummon : player.gameObject;

        // Vira para o alvo
        if (currentTarget.transform.position.x > transform.position.x)
            transform.localScale = new Vector2(1, 1);
        else
            transform.localScale = new Vector2(-1, 1);


        // Evita movimenta��o indesejada se estiver muito perto
        if (Vector2.Distance(transform.position, currentTarget.transform.position) > 2)
        {
            Vector2 direction = (currentTarget.transform.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * enemyChaseSpeed, rb.velocity.y);
        }

        else
            rb.velocity = Vector2.zero;
    }

    private void ZombiePatrol()
    {
        // Faz com que o inimigo se move de sua posi��o atual at� a posi��o alvo (A ou B) na velocidade definida
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemyPatrolSpeed * Time.deltaTime);
        // Se a dist�ncia dos dois pontos for menor que 0.1, ele faz uma checagem de alvo, se o alvo atual for A troca para B, se for B troca para A
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            if (target == A) target = B;
            else target = A;
        }

        // O inimigo vira em dire��o ao ponto que ele est� indo, mudando a dire��o do Detection Range com ele
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

            PoisonArrowEffect();
        }
    }

    public void ApplyBleed(float bleedDamage, float bleedDuration, float bleedInterval)
    {
        // Se o inimigo n�o estiver sangrando, pode aplicar o sangramento
        if (!WolfApplyBleed)
            StartCoroutine(Bleeding(bleedDamage, bleedDuration, bleedInterval));
    }

    IEnumerator Bleeding(float bleedDamage, float bleedDuration, float bleedInterval)
    {
        WolfApplyBleed = true;
        float elapsedBleedTime = 0;

        // Enquanto a duracao total nao for atingida, o inimigo toma dano equivalente ao sangramento (o valor do sangramento est� no wolf attack)
        while (elapsedBleedTime < bleedDuration)
        {
            TakeDamage(bleedDamage);
            yield return new WaitForSeconds(bleedInterval);
            elapsedBleedTime += bleedInterval;
        }

        WolfApplyBleed = false;
    }

    public void ApplyPoison(float poisonDamage, float poisonDuration, float poisonInterval)
    {
        // Se o inimigo n�o estiver sangrando, pode aplicar o sangramento
        if (!PlayerApplyPoison)
            StartCoroutine(Poison(poisonDamage, poisonDuration, poisonInterval));
    }

    IEnumerator Poison(float poisonDamage, float poisonDuration, float poisonInterval)
    {
        PlayerApplyPoison = true;
        float elapsedPoisonTime = 0;

        // Enquanto a duracao total nao for atingida, o inimigo toma dano equivalente ao sangramento (o valor do sangramento est� no wolf attack)
        while (elapsedPoisonTime < poisonDuration)
        {
            TakeDamage(poisonDamage);
            yield return new WaitForSeconds(poisonInterval);
            elapsedPoisonTime += poisonInterval;
        }

        PlayerApplyPoison = false;
    }

    private void PoisonArrowEffect()
    {
        if (player.poisonArrow == true && !PlayerApplyPoison)
        {
            poisonMeter += 50;

            if (poisonMeter >= 100)
            {
                poisonMeter = 0;
                ApplyPoison(0.2f, 5, 1);
            }
        }
    }

    // Faz com que o summon ativo seja o �nico considerado como summon ativo,
    // ou seja, mesmo ao trocar de summon ativo, o inimigo ainda o atacar�
    void TargetSummon()
    {
        // Busca todos os objetos que possuam a tag "Summon"
        GameObject[] summons = GameObject.FindGameObjectsWithTag("Summon");

        // Garante que nenhum summon est� selecionado
        closestSummon = null;

        // A distancia inicial � o infinito, ou seja, qualquer distancia menor que essa ser� considerada como mais proxima
        float minDistance = Mathf.Infinity;

        // Percorra todos os summons
        foreach (GameObject s in summons)
        {
            // S� considera o summon ativo
            if (s.activeInHierarchy)
            {
                // Calcula a distancia entre o inimigo e o summon
                float distance = Vector2.Distance(transform.position, s.transform.position);

                // Se a distancia entre ambos for menor que infinito (sempre ser�),
                // a nova distancia minima sera a distancia entre inimigo e summon e o summon mais proximo sera o ativo
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestSummon = s;
                }
            }
        }
    }
}
