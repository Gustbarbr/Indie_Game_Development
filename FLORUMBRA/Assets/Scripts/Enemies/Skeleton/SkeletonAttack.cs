using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    public PlayerControl player;
    public CircleCollider2D circleCollider;
    private float attackCooldown;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= 1.5)
        {
            circleCollider.enabled = true;
            attackCooldown = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.hpBar.value -= 0.2f;
            circleCollider.enabled = false;
        }
    }
}
