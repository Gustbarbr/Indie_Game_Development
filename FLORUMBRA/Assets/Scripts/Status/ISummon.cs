using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ISummon
{
    void OnSummon(Vector3 position); // Controla a posicao onde o summon aparece
    void OnDismiss(); // Controla a parte de enviar o summon para casa
    void OnRessurrect(); // Controla a ressurreicao dos summons
    bool IsAlive(); // Checa se está summonado
    Slider GetHp(); // Pega o HP
}
