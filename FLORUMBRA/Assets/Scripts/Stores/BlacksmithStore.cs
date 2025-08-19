using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlacksmithStore : MonoBehaviour
{
    PlayerControl player;
    public GameObject blacksmithItems;
    public GameObject interactionButtonDisplay;

    public TextMeshProUGUI arrowLevelText;

    public TextMeshProUGUI arrowUpgradeMaterialOwned;
    public TextMeshProUGUI arrowUpgradeMaterialCost;
    public TextMeshProUGUI arrowUpgradeCrownCost;

    private void Start()
    {
        player = FindObjectOfType<PlayerControl>();
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= 1.5f)
        {
            interactionButtonDisplay.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
                blacksmithItems.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape))
                blacksmithItems.SetActive(false);
        }
        else
        {
            interactionButtonDisplay.SetActive(false);
            blacksmithItems.SetActive(false);
        }

        UpgradeMaterials();
    }

    void UpgradeMaterials()
    {
        if (player.arrowLevel == 1)
        {
            arrowUpgradeMaterialOwned.SetText(player.metal.ToString());
            arrowUpgradeMaterialCost.SetText("/ 2");
            arrowUpgradeCrownCost.SetText("(-100)");
        }

        if (player.arrowLevel == 2)
        {
            arrowUpgradeMaterialOwned.SetText(player.metal.ToString());
            arrowUpgradeMaterialCost.SetText("/ 6");
            arrowUpgradeCrownCost.SetText("(-300)");
        }

        if (player.arrowLevel == 3)
        {
            arrowUpgradeMaterialOwned.SetText(" ");
        }
    }

    public void ArrowUpgrade()
    {
        if(player.arrowLevel == 1 && player.metal >= 2 && player.crown >= 100)
        {
            player.damage *= 1.20f;
            player.metal -= 2;
            player.crown -= 100;
            player.arrowLevel = 2;
            player.metalAmount.SetText("X" + player.metal.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            arrowLevelText.SetText("Lvl. " + player.arrowLevel.ToString());
        }

        if (player.arrowLevel == 2 && player.metal >= 6 && player.crown >= 300)
        {
            player.damage *= 1.20f;
            player.metal -= 6;
            player.crown -= 300;
            player.arrowLevel = 3;
            player.metalAmount.SetText("X" + player.metal.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            arrowLevelText.SetText("Lvl. " + player.arrowLevel.ToString());
        }
    }
}
