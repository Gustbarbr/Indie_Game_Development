using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinPoisonBottle : MonoBehaviour
{

    PlayerControl player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.poisonArrow = true;
            Destroy(gameObject);
        }
    }
}
