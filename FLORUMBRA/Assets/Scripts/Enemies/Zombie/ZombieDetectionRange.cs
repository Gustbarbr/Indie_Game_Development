using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDetectionRange : MonoBehaviour
{
    public ZombieControl zombie;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
            zombie.playerDetected = true;

        if (collider.CompareTag("Summon"))
            zombie.summonDetected = true;
    }
}
