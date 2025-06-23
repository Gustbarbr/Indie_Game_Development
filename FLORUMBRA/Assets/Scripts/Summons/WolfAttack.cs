using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttack : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    private float attackCooldown;
    [SerializeField] private float damage = 0.2f;

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
        if (collision.CompareTag("Enemy"))
        {
            SkeletonControl skeleton = collision.GetComponent<SkeletonControl>();
            skeleton.TakeDamage(damage);
            circleCollider.enabled = false;
        }
    }
}
