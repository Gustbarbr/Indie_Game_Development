using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BullControl : MonoBehaviour, ISummon
{
    // Summonar o touro
    public GameObject bullParent;

    Rigidbody2D rb;

    public PlayerControl player;
    private float moveSpeed = 5;
    private int damage = 40;
    public int level = 1;
    private Vector2 summonDirection;

    // Armazena os inimigos em uma hash, evitando causar duas vezes o mesmo dano
    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, player.transform.position);

        // Retorna a informacao se na frente do touro há um objeto da layer "Barrier"
        RaycastHit2D hitBarrier = Physics2D.Raycast(transform.position, summonDirection, 2.5f, LayerMask.GetMask("Barrier"));

        // Verifica se colidiu com uma parede quebravel
        RaycastHit2D hitBreakableWall = Physics2D.Raycast(transform.position, summonDirection, 0.5f, LayerMask.GetMask("BreakableWall"));

        if (hitBarrier.collider == null)
        {
            // Atualiza a velocidade do touro com base na direção e movespeed
            rb.velocity = new Vector2(summonDirection.x * moveSpeed, rb.velocity.y);
            DamageEnemiesInPath(summonDirection);
        }
            
        else
        {
            if(hitBreakableWall.collider != null)
                BreakWall(hitBreakableWall.collider.gameObject);
            player.isSummoned = false;
            player.ressurrecting = true;
            damagedEnemies.Clear();
            bullParent.gameObject.SetActive(false);
        }
            

        // Mudar para onde o touro olha
        if (summonDirection.x > 0)
            transform.localScale = new Vector2(1, 1);
        else if (summonDirection.x < 0)
            transform.localScale = new Vector2(-1, 1);

    }

    void OnEnable()
    {
        transform.position = player.transform.position;
    }

    void DamageEnemiesInPath(Vector2 direction)
    {
        // A distancia de efeito do acerto
        float range = moveSpeed * Time.deltaTime * 3;
        // Verifica todos os inimigos na linha reta da direcao, sendo que só são considerados aqueles dentro do alcance e que estão na layer "Enemy"
        RaycastHit2D[] hitEnemies = Physics2D.RaycastAll(transform.position, direction, range, LayerMask.GetMask("Enemy"));

        // Para cada objeto detectado pelo Raycast, causa dano
        foreach (RaycastHit2D hit in hitEnemies) {

            // O inimigo sera o objeto atingido pelo Raycast, uma vez que não ocorrerá coli~sao com o touro
            GameObject enemy = hit.collider.gameObject;

            // Verifica se dentro da hash de inimigos danificados já existe o inimigo atual
            if (!damagedEnemies.Contains(enemy)) {
                IDamageable dealDamage = enemy.GetComponent<IDamageable>();

                if (dealDamage != null) {
                    // Causa dano ao inimigo
                    dealDamage.TakeDamage(damage * level);
                    // Adiciona o inimigo danificado a hash, evitando tomar duas vezes o dano
                    damagedEnemies.Add(enemy);
                }
            }
        }
    }

    void BreakWall(GameObject wall)
    {
        Destroy(wall);
    }

    public void TakeDamage(float amount) { }

    // Parte referente ao controle genérico de summon (ISummon)
    public void OnSummon(Vector3 position)
    {
        if (player.mana >= 50)
        {
            bullParent.transform.position = position;
            player.mana -= 50f;
            summonDirection = new Vector2(player.transform.localScale.x, 0).normalized;
            damagedEnemies.Clear();
            player.isSummoned = true;
            bullParent.gameObject.SetActive(true);
        }
    }

    public void OnDismiss()
    {
        damagedEnemies.Clear();
        player.isSummoned = false;
        bullParent.gameObject.SetActive(false);
    }

    public void OnRessurrect() { }

    public bool IsAlive()
    {
        return true;
    }

    public Slider GetHp()
    {
        return null;
    }
}
