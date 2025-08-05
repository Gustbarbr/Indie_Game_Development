using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkeletonLoot : MonoBehaviour
{
    SkeletonControl skeleton;
    PlayerControl player;
    public GameObject loot;

    public TextMeshProUGUI crownQuantityText;
    public GameObject crownLoot;

    public TextMeshProUGUI metalQuantityText;
    public GameObject metalLoot;

    private bool nextToPlayer = false;
    private bool defineCrownValue = true;
    private bool crownWasDropped = false;

    private bool defineMetalValue = true;
    private bool metalWasDropped = false;

    private int enemyDropedLoot = 0;
    public int crownQuantityToPickup;
    public int metalQuantityToPickup;

    private void Start()
    {
        skeleton = GetComponent<SkeletonControl>();
        player = FindAnyObjectByType<PlayerControl>();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= 2.5f)
            nextToPlayer = true;
        else
            nextToPlayer = false;

        EnemyDropedCrown();
        EnemyDropedMetal();

        if (nextToPlayer && enemyDropedLoot >= 1)
            loot.SetActive(true);
        else
            loot.SetActive(false);
    }

    private void EnemyDropedCrown()
    {
        if (defineCrownValue)
        {
            if (skeleton.hp.value <= 0)
            {
                int CrownDrop = Random.Range(0, 100);
                int crownQuantity = Random.Range(5, 15);

                if (CrownDrop <= 85)
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

        if(metalWasDropped)
            metalLoot.transform.localPosition = new Vector2(0, 90);
    }

    private void EnemyDropedMetal()
    {
        if (defineMetalValue)
        {
            if (skeleton.hp.value <= 0)
            {
                int metalDrop = Random.Range(0, 100);
                int metalQuantity = Random.Range(5, 15);

                if (metalDrop <= 30)
                {
                    enemyDropedLoot += 1;
                    if(crownWasDropped)
                        metalLoot.transform.localPosition = new Vector2(0, -1000);
                    else
                        metalLoot.transform.localPosition = new Vector2(0, 90);
                    metalLoot.SetActive(true);
                    metalQuantityToPickup = metalQuantity;
                    metalQuantityText.SetText("X" + metalQuantity.ToString());
                    metalWasDropped = true;
                }
                else
                {
                    metalLoot.SetActive(false);
                }

                defineMetalValue = false;
            }
        }
    }

    public void OnLootMetal()
    {
        player.AddMetal(metalQuantityToPickup);
        metalLoot.SetActive(false);
        enemyDropedLoot -= 1;
    }

    public void ResetLootDrop()
    {
        defineCrownValue = true;
        defineMetalValue = true;
        crownWasDropped = false;
        metalWasDropped = false;
        enemyDropedLoot = 0;
        crownLoot.SetActive(false);
        metalLoot.SetActive(false);
        loot.SetActive(false);
    }
}
