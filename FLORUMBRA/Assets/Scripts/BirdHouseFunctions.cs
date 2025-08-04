using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHouseFunctions : MonoBehaviour
{
    PlayerControl player;
    public GameObject interactionButtonDisplay;

    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= 1.5f)
        {
            interactionButtonDisplay.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                RestorePlayerCondition();
                ResetEnemies();
            }
        }
        else
        {
            interactionButtonDisplay.SetActive(false);
            player.resting = false;
        }
    }

    void RestorePlayerCondition()
    {
        player.hp += player.hpBar.maxValue - player.hpBar.value;
        player.mana += player.manaBar.maxValue - player.manaBar.value;
        player.stamina += player.staminaBar.maxValue - player.staminaBar.value;
        player.hpPotion = player.allocatedHpPotion;
        player.manaPotion = player.allocatedManaPotion;
        player.staminaPotion = player.allocatedStaminaPotion;
        player.resting = true;
    }

    void ResetEnemies()
    {

    }
}
