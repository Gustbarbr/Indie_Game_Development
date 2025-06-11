using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpControl : MonoBehaviour
{

    Rigidbody2D rb;

    // Pulo
    [SerializeField] int jumpHeight;
    public Transform groundCheck;
    public LayerMask groundLayer;
    bool canJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Faz um cálculo de física para checar se tem algo colidindo com a capsula, levando em conta o centro[groundCheck.position], o tamanho [1, 0.3f], a direção [Horizontal], o âmgulo [0] e a layerMask [groundLayer])
        canJump = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (Input.GetButtonDown("Jump") && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }
}
