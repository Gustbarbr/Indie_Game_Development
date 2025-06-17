using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;

    // Movespeed
    [SerializeField] float velocity = 5;
    [SerializeField] float baseSpeed = 5;
    [SerializeField] float sprintVelocity = 2.5f;
    [SerializeField] bool exhaustion = false;

    // HUD
    public Slider hpBar;
    public Slider manaBar;
    public Slider staminaBar;

    private float mana = 1f;
    private float stamina = 1f;

    // Atk
    public GameObject arrowPrefab;
    private float attackCoolDown = 0.5f;
    private float arrowRecharge;
    public float arrowCharge = 0; // Dano do player, pode ser carregado ao segurar o botão para causar mais dano

    // Summons
    public GameObject summonPoint;
    public GameObject wolf;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            stamina -= 0.2f * Time.deltaTime;
            if (stamina < 0) stamina = 0;
            // Se a stamina for 0 entra em exaustão
            if (stamina == 0) exhaustion = true;
        }

        // Se exausto fica mais lento e não pode correr
        else if (exhaustion)
        {
            velocity = baseSpeed - sprintVelocity;
            if (!Input.GetButton("Fire1"))
                stamina += 0.1f * Time.deltaTime;
            // Se a stamina chegar a 30% pode voltar a correr
            if (stamina >= 0.3f) exhaustion = false;
        }

        // Player anda com velocidade padrão e regenera stamina
        else if (!sprint)
        {
            velocity = baseSpeed;
            if (!Input.GetButton("Fire1"))
                stamina += 0.2f * Time.deltaTime;
            if (stamina > 1) stamina = 1;
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
        if (Input.GetButton("Fire1") && arrowRecharge > attackCoolDown && staminaBar.value > 0.1f)
        {
            // Carrega a flecha até seu tempo máximo
            if (arrowCharge <= 1)
                arrowCharge += Time.deltaTime * 2;
            stamina -= 0.3f * Time.deltaTime;
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
        mana += 0.05f * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q) && manaBar.value >= 0.2)
        {
            mana -= 0.2f;
            wolf.gameObject.SetActive(true);
            wolf.transform.position = summonPoint.transform.position;
        }

        manaBar.value = mana;
            
    }
}
