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

        rb.velocity = new Vector2(HorizontalMovement * velocity, rb.velocity.y);
    }

}
