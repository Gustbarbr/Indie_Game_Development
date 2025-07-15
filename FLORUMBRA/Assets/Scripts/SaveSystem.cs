using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Torna SaveSystem acessível globalmente, impede que uma cópia seja criada.
    // get - qualquer codigo pode acessar seus valores por ser público; private set - somente a instancia pode escrever valores.
    public static SaveSystem Instance { get; private set; }

    public float savedHp;
    public float savedMana;
    public float savedStamina;
    public int savedLevel;
    public int savedCoins;


    private void Awake()
    {
        // Verifica se existe uma instancia de SaveSystem
        if(Instance != null && Instance != this)
        {
            // Se existir, essa eh destruida, e já sai do Awake
            Destroy(gameObject);
            return;
        }

        // Do contrario, ela se torna a instancia global
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SavePlayer(PlayerControl player)
    {
        savedHp = player.hp;
        savedMana = player.mana;
        savedStamina = player.stamina;
        savedLevel = player.level;
        savedCoins = player.coins;
    }

    public void LoadPlayer(PlayerControl player)
    {
        player.hp = savedHp;
        player.mana = savedMana;
        player.stamina = savedStamina;
        player.level = savedLevel;
        player.coins = savedCoins;
    }
}
