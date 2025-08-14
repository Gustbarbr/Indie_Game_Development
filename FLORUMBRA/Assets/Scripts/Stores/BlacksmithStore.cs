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
    }

    public void ArrowUpgrade()
    {
        if(player.arrowLevel == 1 && player.metal >= 2 && player.crown >= 1)
        {
            player.damage *= 1.20f;
            player.metal -= 2;
            player.crown -= 1;
            player.arrowLevel = 2;
            player.metalAmount.SetText("X" + player.metal.ToString());
            arrowLevelText.SetText("Lvl. " + player.arrowLevel.ToString());
        }
    }
}
