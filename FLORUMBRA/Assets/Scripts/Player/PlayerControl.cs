using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    Rigidbody2D rb;

    // Movespeed
    [SerializeField] float velocity;
    [SerializeField] float baseSpeed;
    [SerializeField] float sprintVelocity;
    [SerializeField] bool exhaustion = false;

    public Slider hpBar;
    public Slider manaBar;
    public Slider staminaBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        float HorizontalMovement = Input.GetAxisRaw("Horizontal");

        bool sprint = Input.GetKey(KeyCode.LeftShift);
        float stamina = staminaBar.value;

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
            stamina += 0.1f * Time.deltaTime;
            // Se a stamina chegar a 30% pode voltar a correr
            if (stamina >= 0.3f) exhaustion = false;
        }

        // Player anda com velocidade padrão e regenera stamina
        else if (!sprint)
        {
            velocity = baseSpeed;
            stamina += 0.2f * Time.deltaTime;
            if (stamina > 1) stamina = 1;
        }

        // Atualiza a barra de stamina
        staminaBar.value = stamina;

        rb.velocity = new Vector2(HorizontalMovement * velocity, rb.velocity.y);
    }

}
