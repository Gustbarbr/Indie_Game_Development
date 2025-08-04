using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour, IApplyPoison, IDamageable
{
    Rigidbody2D rb;
    [HideInInspector] public bool resting = false;

    [Header("Movimentação")]
    [SerializeField] float velocity = 5;
    [SerializeField] float baseSpeed = 5;
    [SerializeField] float sprintVelocity = 2.5f;
    [SerializeField] bool exhaustion = false;

    [Header("Esquiva")]
    public float dashDistance = 1f;
    [HideInInspector] public float dashDuration = 0.2f;
    [HideInInspector] public float dashCooldown = 1.5f;
    private bool isPerformingDash = false;
    private bool invencibility = false;
    private float dashCooldownTimer = 0f;

    [Header("HUD")]
    public Slider hpBar;
    public Slider manaBar;
    public Slider staminaBar;
    public Slider xpBar;
    [HideInInspector] public float hp = 100f;
    [HideInInspector] public float mana = 100f;
    [HideInInspector] public float stamina = 100f;
    [HideInInspector] public int level = 0;
    [HideInInspector] public int xp = 0;
    public GameObject inventory;
    public TextMeshProUGUI levelText;
    [HideInInspector] public int crown = 0;
    public TextMeshProUGUI crownAmount;

    [Header("Inventário")]
    public TextMeshProUGUI metalAmount;
    [HideInInspector] public int metal = 0;
    public TextMeshProUGUI rottenMeatAmount;
    [HideInInspector] public int rottenMeat = 0;

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
    public float ressurrectionCooldown = 10; // Tempo para ressucitar
    public bool ressurrecting = false; // Checa se está ressucitando ou nao
    JumpControl onGround; // Só pode invocar se o player não estiver pulando

    [Header("Poções")]
    [HideInInspector] public int hpPotion = 0; // Pocoes de vida atuais
    [HideInInspector] public int manaPotion = 0;
    [HideInInspector] public int staminaPotion = 0;
    [HideInInspector] public int allocatedHpPotion = 0; // Pocoes de vida alocadas (As que serao rcarregadas ao descansar)
    [HideInInspector] public int allocatedManaPotion = 0;
    [HideInInspector] public int allocatedStaminaPotion = 0;
    [HideInInspector] public int maxAllocablePotion = 3; // Quantia maxima de pocoes que podem ser alocada
    public TextMeshProUGUI hpPotionQuantity;
    public TextMeshProUGUI manaPotionQuantity;
    public TextMeshProUGUI staminaPotionQuantity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        onGround = GetComponent<JumpControl>();

        if(summons.Count > 0)
            currentSummon = summons[0].GetComponentInChildren<ISummon>();

        // Se o singleton SaveSystem existir e se ha progresso salvo, carrega os dados do usuário
        if(SaveSystem.Instance != null && SaveSystem.Instance.savedLevel > 0)
        {
            SaveSystem.Instance.LoadPlayer(this);
        }

        hpPotion = 1;
        manaPotion = 1;
        staminaPotion = 1;
        allocatedHpPotion = hpPotion;
        allocatedManaPotion = manaPotion;
        allocatedStaminaPotion = staminaPotion;
        levelText.SetText(level.ToString());
        crownAmount.SetText("0");
    }

    // Por conta do rigidbody é mais recomendado usar fixedupdate
    void FixedUpdate()
    {
        PlayerMovement();
    }

    // Por ser necessário atualizar frame a frama é mais recomendado usar update
    void Update()
    {
        Dash();
        ShootArrow();
        SummonCompanion();
        RessurrectCompanion();
        SwitchSummon();
        if(level <= 20)
            LevelUp();
        hpBar.value = hp;
        staminaBar.value = stamina;
        manaBar.value = mana;
        xpBar.value = xp;
        OpenInventory();
        UsePotion();
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
                stamina += 5f * Time.deltaTime;
            // Se a stamina chegar a 30% pode voltar a correr
            if (stamina >= 30f) exhaustion = false;
        }

        // Player anda com velocidade padrão e regenera stamina
        else if (!sprint)
        {
            velocity = baseSpeed;
            if (!Input.GetButton("Fire1"))
                stamina += 10f * Time.deltaTime;
            if (stamina > staminaBar.maxValue) stamina = staminaBar.maxValue;
        }

        rb.velocity = new Vector2(horizontalMovement * velocity, rb.velocity.y);
    }

    private void Dash()
    {
        if(dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetButton("Fire2") && !isPerformingDash && dashCooldownTimer <= 0f && stamina >= 15f)
        {
            stamina -= 15f;
            StartCoroutine(PerformDash());
            dashCooldownTimer = dashCooldown;
        }
            
    }

    private IEnumerator PerformDash()
    {
        isPerformingDash = true;
        invencibility = true;

        float direction = transform.localScale.x;
        float elapsedTime = 0f;
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(direction * dashDistance, 0);

        while(elapsedTime < dashDuration)
        {
            float t = elapsedTime / dashDuration; // Parâmetro de interpolação 
            rb.MovePosition(Vector2.Lerp(start, end, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.MovePosition(end);
        isPerformingDash = false;

        yield return new WaitForSeconds(0.2f);
        invencibility = false;
    }

    public void ShootArrow()
    {
        // Tempo entre cada ataque
        arrowRecharge += Time.deltaTime;

        // Verifica se o mouse está em cima de um objeto UI (eh usado para evitar conflito com o botao de loot)
        bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();

        // Enquanto o botão for pressionado, stamina será gasta
        if (Input.GetButton("Fire1") && arrowRecharge > attackCoolDown && staminaBar.value > 1 && !mouseOverUI)
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
        else if ((Input.GetButtonUp("Fire1") && arrowRecharge > attackCoolDown && staminaBar.value > 0) && !mouseOverUI)
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
        if(mana < manaBar.maxValue)
            mana += (isSummoned ? 1f : 2.5f) * Time.deltaTime;
    }

    public void RessurrectCompanion()
    {
        if(!isSummoned && !ressurrecting && !currentSummon.IsAlive())
            ressurrecting = true;
            
        if (ressurrecting)
            ressurrectionCooldown -= Time.deltaTime;

        if (ressurrectionCooldown <= 0)
        {
            currentSummon.OnRessurrect();
            ressurrecting = false;
            ressurrectionCooldown = 10 - level * 0.5f;
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
        if (invencibility) return;
        hp -= amount;
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
        while (elapsedPoisonTime < poisonDuration && resting == false)
        {
            TakeDamage(poisonDamage);
            yield return new WaitForSeconds(poisonInterval);
            elapsedPoisonTime += poisonInterval;
        }

        HitApplyPoison = false;
    }

    private void UpdateStats()
    {
        hp = 100f + 50 * level;
        mana = 100f + 25 * level;
        stamina = 100f + 10 * level;
        hpBar.maxValue = hp;
        manaBar.maxValue = mana;
        staminaBar.maxValue = stamina;
    }

    private void LevelUp()
    {
        if(xp >= xpBar.maxValue)
        {
            level += 1;
            xp = 0;
            xpBar.maxValue += 100 * level;
            levelText.SetText(level.ToString());
            ressurrectionCooldown = 10 - level * 0.5f;
            UpdateStats();
        }
    }

    private void OpenInventory()
    {
        if (Input.GetKey(KeyCode.I))
            inventory.SetActive(true);
        else 
            inventory.SetActive(false);
    }

    private void UsePotion()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hpPotion >= 1)
        {
            hpPotion -= 1;
            hp += 20;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && manaPotion >= 1)
        {
            manaPotion -= 1;
            mana += 20;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && staminaPotion >= 1)
        {
            staminaPotion -= 1;
            stamina += 20;
        }

        hpPotionQuantity.SetText("x" + hpPotion.ToString());
        manaPotionQuantity.SetText("x" + manaPotion.ToString());
        staminaPotionQuantity.SetText("x" + staminaPotion.ToString());
    }

    public void AddCrowns(int amount)
    {
        crown += amount;
        crownAmount.SetText(crown.ToString());
    }

    public void AddMetal(int amount)
    {
        metal += amount;
        metalAmount.SetText("X" + metal.ToString());
    }

    public void AddRottenMeat(int amount)
    {
        rottenMeat += amount;
        rottenMeatAmount.SetText("X" + rottenMeat.ToString());
    }
}