using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour, IApplyPoison, IDamageable
{
    Rigidbody2D rb;

    [Header("Movimentação")]
    [SerializeField] float velocity = 5;
    [SerializeField] float baseSpeed = 5;
    [SerializeField] float sprintVelocity = 2.5f;
    [SerializeField] bool exhaustion = false;

    [Header("HUD")]
    public Slider hpBar;
    public Slider manaBar;
    public Slider staminaBar;
    [HideInInspector] public float mana = 100f;
    private float stamina = 100f;

    [Header("ATK")]
    public GameObject arrowPrefab;
    private float attackCoolDown = 0.5f;
    private float arrowRecharge;
    public float arrowCharge = 0; // Dano do player, pode ser carregado ao segurar o botão para causar mais dano

    [Header("Itens do player")]
    public bool poisonArrow = false;

    // Efeitos dos inimigos
    public bool HitApplyPoison { get; set; }

    [Header("Summons")]
    public List<GameObject> summons = new List<GameObject>(); // Cria uma lista para guardar os summons
    private int currentSummonIndex = 0; // Indice da lista de summons
    private ISummon currentSummon; // Interface ISummon
    public bool isSummoned = false; // Verifica se o companion está invocado
    public float ressurrectionCooldown = 0; // Tempo para ressucitar
    public bool ressurrecting = false; // Checa se está ressucitando ou nao
    JumpControl onGround; // Só pode invocar se o player não estiver pulando

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        onGround = GetComponent<JumpControl>();

        if(summons.Count > 0)
            currentSummon = summons[0].GetComponentInChildren<ISummon>();
    }

    // Por conta do rigidbody é mais recomendado usar fixedupdate
    void FixedUpdate()
    {
        PlayerMovement();
    }

    // Por ser necessário atualizar frame a frama é mais recomendado usar update
    void Update()
    {
        ShootArrow();
        SummonCompanion();
        RessurrectCompanion();
        SwitchSummon();
    }

    public void PlayerMovement()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");

        bool sprint = Input.GetKey(KeyCode.LeftShift);

        // Muda para onde o player olha de acordo com input do teclado
        if(horizontalMovement == 1)
        {
            transform.localScale = new Vector2(1, 1);
        }

        else if (horizontalMovement == -1)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        // Se o player apertar para correr e não estiver exausto, vai começar a correr
        if (sprint && !exhaustion)
        {
            velocity = baseSpeed + sprintVelocity;
            stamina -= 20 * Time.deltaTime;
            if (stamina < 0) stamina = 0;
            // Se a stamina for 0 entra em exaustão
            if (stamina == 0) exhaustion = true;
        }

        // Se exausto fica mais lento e não pode correr
        else if (exhaustion)
        {
            velocity = baseSpeed - sprintVelocity;
            if (!Input.GetButton("Fire1"))
                stamina += 10f * Time.deltaTime;
            // Se a stamina chegar a 30% pode voltar a correr
            if (stamina >= 30f) exhaustion = false;
        }

        // Player anda com velocidade padrão e regenera stamina
        else if (!sprint)
        {
            velocity = baseSpeed;
            if (!Input.GetButton("Fire1"))
                stamina += 20f * Time.deltaTime;
            if (stamina > 100) stamina = 100;
        }

        // Atualiza a barra de stamina
        staminaBar.value = stamina;

        rb.velocity = new Vector2(horizontalMovement * velocity, rb.velocity.y);
    }

    public void ShootArrow()
    {
        // Tempo entre cada ataque
        arrowRecharge += Time.deltaTime;

        // Enquanto o botão for pressionado, stamina será gasta
        if (Input.GetButton("Fire1") && arrowRecharge > attackCoolDown && staminaBar.value > 1)
        {
            // Carrega a flecha até seu tempo máximo
            if (arrowCharge <= 100)
            {
                arrowCharge += Time.deltaTime * 100;
                stamina -= 25 * Time.deltaTime;
            }
                
            if (stamina <= 0) stamina = 0;
        }

        // Só pode atacar se o botão do mouse for solto, estiver fora do tempo de recarga e tiver stamina
        else if ((Input.GetButtonUp("Fire1") && arrowRecharge > attackCoolDown && staminaBar.value > 0) || staminaBar.value <=0)
        {
            // Determina a direção da flecha com base na escala do jogador, ou seja, para onde está olhando
            float direction = transform.localScale.x;

            // Cria a flecha um pouco a frente do player para evitar colisões erradas
            Vector3 spawnPosition = transform.position + new Vector3(transform.localScale.x * 0.5f, 0, 0);

            // Instância a arrow com todos seus valores de posição e rotação
            GameObject arrow = Instantiate(arrowPrefab, spawnPosition, Quaternion.identity);

            // Define a direção que a flecha será disparada com base na variável direction
            arrow.GetComponent<ArrowControl>().SetDirection(direction);

            // Entra em tempo de recarga
            arrowRecharge = 0;
        }
    }

    public void SummonCompanion()
    {
        if (ressurrecting == false)
        {
            // Checa se o botão pressionado for o "Q", está no solo e não há companions invocados
            if (Input.GetKeyDown(KeyCode.Q) && onGround.canJump && isSummoned == false)
                currentSummon.OnSummon(transform.position);

            else if (Input.GetKeyDown(KeyCode.Q) && isSummoned == true)
                currentSummon.OnDismiss();

        }

        // Se o lobo está invocado, a mana se regenera mais lentamente
        mana += (isSummoned ? 1f : 2.5f) * Time.deltaTime;

        manaBar.value = mana;

    }

    public void RessurrectCompanion()
    {
        if(!isSummoned && !ressurrecting && !currentSummon.IsAlive())
            ressurrecting = true;
            
        if (ressurrecting)
            ressurrectionCooldown += Time.deltaTime;

        if (ressurrectionCooldown >= 5)
        {
            currentSummon.OnRessurrect();
            ressurrecting = false;
            ressurrectionCooldown = 0;
        }
    }

    public void SwitchSummon()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isSummoned){

            // Avança para o proximo indice da lista (currentSummonIndex + 1) e garante que ao chegar no final, retorne ao primeiro ( % summons.Count)
            currentSummonIndex = (currentSummonIndex + 1) % summons.Count;
            // Acessa o objeto da lista que possui a interface ISummon
            currentSummon = summons[currentSummonIndex].GetComponentInChildren<ISummon>();
            Debug.Log("Summon atual: " + currentSummon);
        }
    }

    public void TakeDamage(float amount)
    {
        hpBar.value -= amount;

        
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
}
