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
                player.RestorePlayerStatus();
                ResetEnemies();
            }
        }
        else
        {
            interactionButtonDisplay.SetActive(false);
        }
    }

    void ResetEnemies()
    {
        SkeletonControl[] skeleton = FindObjectsOfType<SkeletonControl>();
        foreach(SkeletonControl skel in skeleton)
        {
            skel.ResetEnemy();
        }

        ZombieControl[] zombie = FindObjectsOfType<ZombieControl>();
        foreach (ZombieControl zomb in zombie)
        {
            zomb.ResetEnemy();
        }
    }
}
