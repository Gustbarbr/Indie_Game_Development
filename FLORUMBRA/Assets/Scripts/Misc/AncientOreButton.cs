using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientOreButton : MonoBehaviour
{
    PlayerControl player;
    public GameObject displayButton;
    public GameObject ancientOre;

    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        displayButton.SetActive(false);
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= 1.5f)
        {
            displayButton.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                player.ancientOre += 1;
                player.ancientOreAmount.SetText("X" + player.ancientOre.ToString());
                Destroy(ancientOre);
            }
                
        }
        else
            displayButton.SetActive(false);
    }
}
