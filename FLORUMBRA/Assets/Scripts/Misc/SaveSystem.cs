using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Torna SaveSystem acess�vel globalmente, impede que uma c�pia seja criada.
    // get - qualquer codigo pode acessar seus valores por ser p�blico; private set - somente a instancia pode escrever valores.
    public static SaveSystem Instance { get; private set; }

    public int savedLevel;
    public int savedCrown;


    private void Awake()
    {
        // Verifica se existe uma instancia de SaveSystem
        if(Instance != null && Instance != this)
        {
            // Se existir, essa eh destruida, e j� sai do Awake
            Destroy(gameObject);
            return;
        }

        // Do contrario, ela se torna a instancia global
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SavePlayer(PlayerControl player)
    {
        savedLevel = player.level;
        savedCrown = player.crown;
    }

    public void LoadPlayer(PlayerControl player)
    {
        player.level = savedLevel;
        player.crown = savedCrown;
    }
}
