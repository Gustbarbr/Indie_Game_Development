using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    Rigidbody2D rb;

    // Movespeed
    [SerializeField] float velocity = 5f;

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
        // Pega o valor inteiro do input (-1, 0 ou 1)
        float HorizontalMovement = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(HorizontalMovement * velocity, rb.velocity.y);
    }

}
