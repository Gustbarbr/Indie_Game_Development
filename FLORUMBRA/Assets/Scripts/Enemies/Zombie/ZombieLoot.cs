using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieLoot : MonoBehaviour
{
    ZombieControl zombie;
    PlayerControl player;
    public GameObject loot;

    public TextMeshProUGUI crownQuantityText;
    public GameObject crownLoot;

    public TextMeshProUGUI leatherQuantityText;
    public GameObject leatherLoot;

    private bool nextToPlayer = false;
    private bool defineCrownValue = true;
    private bool crownWasDropped = false;

    private bool defineLeatherValue = true;
    private bool leatherWasDropped = false;

    private int enemyDropedLoot = 0;
    public int crownQuantityToPickup;
    public int leatherQuantityToPickup;

    private void Start()
    {
        zombie = GetComponent<ZombieControl>();
        player = FindAnyObjectByType<PlayerControl>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= 2.5f)
            nextToPlayer = true;
        else
            nextToPlayer = false;

        EnemyDropedCrown();
        EnemyDropedLeather();

        if (nextToPlayer && enemyDropedLoot >= 1)
            loot.SetActive(true);
        else
            loot.SetActive(false);
    }

    private void EnemyDropedCrown()
    {
        if (defineCrownValue)
        {
            if (zombie.hp.value <= 0)
            {
                int CrownDrop = Random.Range(0, 100);
                int crownQuantity = Random.Range(25, 100);

                if (CrownDrop <= 60)
                {
                    enemyDropedLoot += 1;
                    crownLoot.transform.localPosition = new Vector2(0, 90);
                    crownLoot.SetActive(true);
                    crownQuantityToPickup = crownQuantity;
                    crownQuantityText.SetText("X" + crownQuantity.ToString());
                    crownWasDropped = true;
                }
                else
                {
                    crownLoot.SetActive(false);
                }

                defineCrownValue = false;
            }
        }
    }

    public void OnLootCrown()
    {
        player.AddCrowns(crownQuantityToPickup);
        crownLoot.SetActive(false);
        enemyDropedLoot -= 1;

        if (leatherWasDropped)
            leatherLoot.transform.localPosition = new Vector2(0, 90);
    }

    private void EnemyDropedLeather()
    {
        if (defineLeatherValue)
        {
            if (zombie.hp.value <= 0)
            {
                int leatherDrop = Random.Range(0, 100);
                int leatherQuantity = Random.Range(1, 5);

                if (leatherDrop <= 40)
                {
                    enemyDropedLoot += 1;
                    if (crownWasDropped)
                        leatherLoot.transform.localPosition = new Vector2(0, -1000);
                    else
                        leatherLoot.transform.localPosition = new Vector2(0, 90);
                    leatherLoot.SetActive(true);
                    leatherQuantityToPickup = leatherQuantity;
                    leatherQuantityText.SetText("X" + leatherQuantity.ToString());
                    leatherWasDropped = true;
                }
                else
                {
                    leatherLoot.SetActive(false);
                }

                defineLeatherValue = false;
            }
        }
    }

    public void OnLootLeather()
    {
        player.AddLeather(leatherQuantityToPickup);
        leatherLoot.SetActive(false);
        enemyDropedLoot -= 1;
    }

    public void ResetLootDrop()
    {
        defineCrownValue = true;
        defineLeatherValue = true;
        crownWasDropped = false;
        leatherWasDropped = false;
        enemyDropedLoot = 0;
        crownLoot.SetActive(false);
        leatherLoot.SetActive(false);
        loot.SetActive(false);
    }
}
