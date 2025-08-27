using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BlacksmithStore : MonoBehaviour
{
    PlayerControl player;
    public GameObject blacksmithItems;
    public GameObject interactionButtonDisplay;

    public TextMeshProUGUI arrowLevelText;
    public TextMeshProUGUI quiverLevelText;
    public TextMeshProUGUI armorLevelText;

    public TextMeshProUGUI arrowUpgradeMaterialOwned;
    public TextMeshProUGUI arrowUpgradeMaterialCost;
    public TextMeshProUGUI arrowUpgradeCrownCost;

    public TextMeshProUGUI quiverUpgradeMaterialOwned;
    public TextMeshProUGUI quiverUpgradeMaterialCost;
    public TextMeshProUGUI quiverUpgradeCrownCost;

    public TextMeshProUGUI armorUpgradeMaterialOwned;
    public TextMeshProUGUI armorUpgradeMaterialCost;
    public TextMeshProUGUI armorUpgradeCrownCost;

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

        ArrowUpgradeMaterials();
        QuiverUpgradeMaterials();
        ArmorUpgradeMaterials();
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

        else if (player.arrowLevel == 2 && player.metal >= 5 && player.crown >= 300)
        {
            player.damage *= 1.20f;
            player.metal -= 6;
            player.crown -= 300;
            player.arrowLevel = 3;
            player.metalAmount.SetText("X" + player.metal.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            arrowLevelText.SetText("Lvl. " + player.arrowLevel.ToString());
        }

        else if (player.arrowLevel == 3 && player.metal >= 10 && player.crown >= 750)
        {
            player.damage *= 1.20f;
            player.metal -= 10;
            player.crown -= 750;
            player.arrowLevel = 4;
            player.metalAmount.SetText("X" + player.metal.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            arrowLevelText.SetText("Lvl. " + player.arrowLevel.ToString());
        }

        else if (player.arrowLevel == 4 && player.metal >= 15 && player.crown >= 1250)
        {
            player.damage *= 1.20f;
            player.metal -= 15;
            player.crown -= 1250;
            player.arrowLevel = 5;
            player.metalAmount.SetText("X" + player.metal.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            arrowLevelText.SetText("Lvl. " + player.arrowLevel.ToString());
        }

        else if (player.arrowLevel == 5 && player.metal >= 25 && player.crown >= 2000)
        {
            player.damage *= 1.20f;
            player.metal -= 25;
            player.crown -= 2000;
            player.arrowLevel = 6;
            player.metalAmount.SetText("X" + player.metal.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            arrowLevelText.SetText("Lvl. " + player.arrowLevel.ToString());
        }
    }

    void ArrowUpgradeMaterials()
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
            arrowUpgradeMaterialOwned.SetText(player.metal.ToString());
            arrowUpgradeMaterialCost.SetText("/ 10");
            arrowUpgradeCrownCost.SetText("(-750)");
        }

        if (player.arrowLevel == 4)
        {
            arrowUpgradeMaterialOwned.SetText(player.metal.ToString());
            arrowUpgradeMaterialCost.SetText("/ 15");
            arrowUpgradeCrownCost.SetText("(-1250)");
        }

        if (player.arrowLevel == 5)
        {
            arrowUpgradeMaterialOwned.SetText(player.metal.ToString());
            arrowUpgradeMaterialCost.SetText("/ 25");
            arrowUpgradeCrownCost.SetText("(-2000)");
        }

        if (player.arrowLevel == 6)
        {
            arrowUpgradeMaterialCost.SetText("Máx");
        }
    }

    public void QuiverUpgrade()
    {
        if (player.quiverLevel == 1 && player.leather >= 1 && player.crown >= 75)
        {
            player.arrowRechargeMultiplier *= 1.15f;
            player.leather -= 1;
            player.crown -= 75;
            player.quiverLevel = 2;
            player.leatherAmount.SetText("X" + player.leather.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            quiverLevelText.SetText("Lvl. " + player.quiverLevel.ToString());
        }

        else if (player.quiverLevel == 2 && player.leather >= 4 && player.crown >= 250)
        {
            player.arrowRechargeMultiplier *= 1.15f;
            player.leather -= 4;
            player.crown -= 250;
            player.quiverLevel = 3;
            player.leatherAmount.SetText("X" + player.leather.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            quiverLevelText.SetText("Lvl. " + player.quiverLevel.ToString());
        }

        else if (player.quiverLevel == 3 && player.leather >= 6 && player.crown >= 600)
        {
            player.arrowRechargeMultiplier *= 1.15f;
            player.leather -= 6;
            player.crown -= 600;
            player.quiverLevel = 4;
            player.leatherAmount.SetText("X" + player.leather.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            quiverLevelText.SetText("Lvl. " + player.quiverLevel.ToString());
        }

        else if (player.quiverLevel == 4 && player.leather >= 10 && player.crown >= 1000)
        {
            player.arrowRechargeMultiplier *= 1.15f;
            player.leather -= 10;
            player.crown -= 1000;
            player.quiverLevel = 5;
            player.leatherAmount.SetText("X" + player.leather.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            quiverLevelText.SetText("Lvl. " + player.quiverLevel.ToString());
        }

        else if (player.quiverLevel == 5 && player.leather >= 15 && player.crown >= 1750)
        {
            player.arrowRechargeMultiplier *= 1.15f;
            player.leather -= 15;
            player.crown -= 1750;
            player.quiverLevel = 6;
            player.leatherAmount.SetText("X" + player.leather.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            quiverLevelText.SetText("Lvl. " + player.quiverLevel.ToString());
        }

    }

    void QuiverUpgradeMaterials()
    {
        if (player.quiverLevel == 1)
        {
            quiverUpgradeMaterialOwned.SetText(player.leather.ToString());
            quiverUpgradeMaterialCost.SetText("/ 1");
            quiverUpgradeCrownCost.SetText("(-75)");
        }

        if (player.quiverLevel == 2)
        {
            quiverUpgradeMaterialOwned.SetText(player.leather.ToString());
            quiverUpgradeMaterialCost.SetText("/ 4");
            quiverUpgradeCrownCost.SetText("(-250)");
        }

        if (player.quiverLevel == 3)
        {
            quiverUpgradeMaterialOwned.SetText(player.leather.ToString());
            quiverUpgradeMaterialCost.SetText("/ 6");
            quiverUpgradeCrownCost.SetText("(-600)");
        }

        if (player.quiverLevel == 4)
        {
            quiverUpgradeMaterialOwned.SetText(player.leather.ToString());
            quiverUpgradeMaterialCost.SetText("/ 10");
            quiverUpgradeCrownCost.SetText("(-1000)");
        }

        if (player.quiverLevel == 5)
        {
            quiverUpgradeMaterialOwned.SetText(player.leather.ToString());
            quiverUpgradeMaterialCost.SetText("/ 15");
            quiverUpgradeCrownCost.SetText("(-1750)");
        }

        if (player.quiverLevel == 6)
        {
            quiverUpgradeMaterialCost.SetText("Máx");
        }
    }

    public void ArmorUpgrade()
    {
        if (player.armorLevel == 1 && player.ancientOre >= 1 && player.crown >= 250)
        {
            player.def += 30;
            player.ancientOre -= 1;
            player.crown -= 250;
            player.armorLevel = 2;
            player.ancientOreAmount.SetText("X" + player.ancientOre.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            armorLevelText.SetText("Lvl. " + player.armorLevel.ToString());
        }

        else if (player.armorLevel == 2 && player.ancientOre >= 2 && player.crown >= 550)
        {
            player.def += 30;
            player.ancientOre -= 2;
            player.crown -= 550;
            player.armorLevel = 3;
            player.ancientOreAmount.SetText("X" + player.ancientOre.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            armorLevelText.SetText("Lvl. " + player.armorLevel.ToString());
        }

        else if (player.armorLevel == 3 && player.ancientOre >= 4 && player.crown >= 1200)
        {
            player.def += 30;
            player.ancientOre -= 4;
            player.crown -= 1200;
            player.armorLevel = 4;
            player.ancientOreAmount.SetText("X" + player.ancientOre.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            armorLevelText.SetText("Lvl. " + player.armorLevel.ToString());
        }

        else if (player.armorLevel == 4 && player.ancientOre >= 6 && player.crown >= 1700)
        {
            player.def += 30;
            player.ancientOre -= 6;
            player.crown -= 1700;
            player.armorLevel = 5;
            player.ancientOreAmount.SetText("X" + player.ancientOre.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            armorLevelText.SetText("Lvl. " + player.armorLevel.ToString());
        }

        else if (player.armorLevel == 5 && player.ancientOre >= 10 && player.crown >= 2500)
        {
            player.def += 30;
            player.ancientOre -= 10;
            player.crown -= 2500;
            player.armorLevel = 6;
            player.ancientOreAmount.SetText("X" + player.ancientOre.ToString());
            player.crownAmount.SetText("X" + player.crown.ToString());
            armorLevelText.SetText("Lvl. " + player.armorLevel.ToString());
        }

    }

    void ArmorUpgradeMaterials()
    {
        if (player.armorLevel == 1)
        {
            armorUpgradeMaterialOwned.SetText(player.ancientOre.ToString());
            armorUpgradeMaterialCost.SetText("/ 1");
            armorUpgradeCrownCost.SetText("(-250)");
        }

        if (player.armorLevel == 2)
        {
            armorUpgradeMaterialOwned.SetText(player.ancientOre.ToString());
            armorUpgradeMaterialCost.SetText("/ 2");
            armorUpgradeCrownCost.SetText("(-550)");
        }

        if (player.armorLevel == 3)
        {
            armorUpgradeMaterialOwned.SetText(player.ancientOre.ToString());
            armorUpgradeMaterialCost.SetText("/ 4");
            armorUpgradeCrownCost.SetText("(-1200)");
        }

        if (player.armorLevel == 4)
        {
            armorUpgradeMaterialOwned.SetText(player.ancientOre.ToString());
            armorUpgradeMaterialCost.SetText("/ 6");
            armorUpgradeCrownCost.SetText("(-1700)");
        }

        if (player.armorLevel == 5)
        {
            armorUpgradeMaterialOwned.SetText(player.ancientOre.ToString());
            armorUpgradeMaterialCost.SetText("/ 10");
            armorUpgradeCrownCost.SetText("(-2500)");
        }

        if (player.armorLevel == 6)
        {
            armorUpgradeMaterialCost.SetText("Máx");
        }
    }
}
