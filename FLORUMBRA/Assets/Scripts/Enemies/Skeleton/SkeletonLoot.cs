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

    private bool nextToPlayer = false;
    private bool generateCrownLoot = true;

    private int enemyDropedLoot = 0;
    public int crownQuantityToPickup;

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

        if (nextToPlayer && enemyDropedLoot >= 1)
            loot.SetActive(true);
        else
            loot.SetActive(false);
    }

    private void EnemyDropedCrown()
    {
        if (generateCrownLoot)
        {
            if (skeleton.hp.value <= 0)
            {
                int CrownDrop = Random.Range(0, 100);
                int crownQuantity = Random.Range(5, 15);

                if (CrownDrop <= 65)
                {
                    enemyDropedLoot += 1;
                    crownLoot.transform.localPosition = new Vector2(0, 0);
                    crownLoot.SetActive(true);
                    crownQuantityToPickup = crownQuantity;
                    crownQuantityText.SetText("X" + crownQuantity.ToString());
                }
                else
                {
                    crownLoot.SetActive(false);
                }

                generateCrownLoot = false;
            }
        }
    }

    public void OnLootCrown()
    {
        player.AddCrowns(crownQuantityToPickup);
        crownLoot.SetActive(false);
        enemyDropedLoot -= 1;
    }
}
