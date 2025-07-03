using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    public PlayerControl player;
    public CircleCollider2D circleCollider;
    private float attackCooldown;
    private int damage = 20;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= 1.5)
            circleCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IDamageable player = collision.GetComponent<IDamageable>();
            player.TakeDamage(damage);
            circleCollider.enabled = false;
            attackCooldown = 0;
        }

        if (collision.CompareTag("Summon"))
        {
            IDamageable summon = collision.GetComponent<IDamageable>();
            summon.TakeDamage(damage);
            circleCollider.enabled = false;
            attackCooldown = 0;
        }
    }
}
