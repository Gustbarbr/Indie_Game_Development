using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpControl : MonoBehaviour
{
    Rigidbody2D rb;

    // Pulo
    private int jumpHeight = 7; // Define a força do pulo
    private float buttonPressingTime = 0.3f; // Limita o tempo do pulo
    [HideInInspector] public bool canJump; // Checa se o player está tocando o solo
    float jumpTime; // Contabiliza o tempo do pulo
    bool awayFromGround; // Checa se o player está pulando
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Faz um cálculo de física para checar se tem algo colidindo com a capsula, levando em conta o centro[groundCheck.position], o tamanho [1, 0.3f], a direção [Horizontal], o âmgulo [0] e a layerMask [groundLayer])
        canJump = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        // Se o botão for pressionado, a bool ficará true, o que fará com que o rigidbody ganhe um impulso vertical, gerando um pulo
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            awayFromGround = true;
            jumpTime = 0;
        }

        if (awayFromGround)
        {
            // Mantém a velocidade horizontal e aplica uma força vertical
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpTime += Time.deltaTime;
        }

        // Se o espaço deixar de ser pressionado, ou o limite de tempo no ar (buttonPressingTime) for ultrapassado, o player começa a cair
        if (Input.GetKeyUp(KeyCode.Space) || jumpTime > buttonPressingTime)
        {
            awayFromGround = false;
        }
    }
}
