using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfMap : MonoBehaviour
{
    public PlayerControl player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Retorna o topo da hierarquia do objeto, ou seja, retornará sempre o pai
        /*Transform root = collision.transform.root;

        if (root.CompareTag("Summon"))
            root.gameObject.SetActive(false);*/

        if (collision.CompareTag("Player"))
            player.hpBar.value = 0;

        else
            Destroy(collision.gameObject);
    }
}
