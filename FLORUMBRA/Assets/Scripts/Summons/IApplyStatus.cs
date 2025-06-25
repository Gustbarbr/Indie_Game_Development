using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IApplyStatus
{
    // Essa eh uma interface, que por si so nao faz nada, mas permite criar codigos genericos e reutilizaveis.
    // Ela servira para cuidar dos status que os summons conseguem causar nos inimigos
    void TakeDamage(float damage);
    // "get;set;" diz que a propriedade pode ser lida (get) e modificada (set)
    bool WolfApplyBleed { get; set; }
    // Aumentar escalabilidade do sangramento, para que mesmo quando o lobo for mandado para casa, o inimigo ainda sangre
    void ApplyBleed(float damage, float duration, float interval);
}
