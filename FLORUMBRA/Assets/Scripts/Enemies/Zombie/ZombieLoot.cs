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

    public TextMeshProUGUI rottenMeatQuantityText;
    public GameObject rottenMeatLoot;

    private bool nextToPlayer = false;
    private bool defineCrownValue = true;
    private bool crownWasDropped = false;

    private bool defineRottenMeatValue = true;
    private bool rottenMeatWasDropped = false;

    private int enemyDropedLoot = 0;
    public int crownQuantityToPickup;
    public int rottenMeatQuantityToPickup;

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
        EnemyDropedRottenMeat();

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
                int crownQuantity = Random.Range(10, 35);

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

        if (rottenMeatWasDropped)
            rottenMeatLoot.transform.localPosition = new Vector2(0, 90);
    }

    private void EnemyDropedRottenMeat()
    {
        if (defineRottenMeatValue)
        {
            if (zombie.hp.value <= 0)
            {
                int rottenMeatDrop = Random.Range(0, 100);
                int rottenMeatQuantity = Random.Range(5, 15);

                if (rottenMeatDrop <= 15)
                {
                    enemyDropedLoot += 1;
                    if (crownWasDropped)
                        rottenMeatLoot.transform.localPosition = new Vector2(0, -1000);
                    else
                        rottenMeatLoot.transform.localPosition = new Vector2(0, 90);
                    rottenMeatLoot.SetActive(true);
                    rottenMeatQuantityToPickup = rottenMeatQuantity;
                    rottenMeatQuantityText.SetText("X" + rottenMeatQuantity.ToString());
                    rottenMeatWasDropped = true;
                }
                else
                {
                    rottenMeatLoot.SetActive(false);
                }

                defineRottenMeatValue = false;
            }
        }
    }

    public void OnLootRottenMeat()
    {
        player.AddRottenMeat(rottenMeatQuantityToPickup);
        rottenMeatLoot.SetActive(false);
        enemyDropedLoot -= 1;
    }

    public void ResetLootDrop()
    {
        defineCrownValue = true;
        defineRottenMeatValue = true;
        crownWasDropped = false;
        rottenMeatWasDropped = false;
        enemyDropedLoot = 0;
        crownLoot.SetActive(false);
        rottenMeatLoot.SetActive(false);
        loot.SetActive(false);
    }
}
