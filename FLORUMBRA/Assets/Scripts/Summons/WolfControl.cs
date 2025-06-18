using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfControl : MonoBehaviour
{

    public PlayerControl player;
    private float moveSpeed = 5;
    private float detectionRange = 15;

    void Update()
    {
        GameObject closestEnemy = ChaseEnemy();

        if(closestEnemy != null)
        {
            // Calcula a distancia entre o lobo e o inimigo e o normalized garante que a velocidade de perseguicao sera constante, mesmo com o inimigo se afastando
            Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;

            // Transforma o calculo de movimento (direction * moveSpeed * Time.deltaTime) em um vector 3, para que a velocidade possa ser adicionada ao objeto,
            // pois transform.position eh um vetor
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
        
    }

    GameObject ChaseEnemy()
    {
        // Armazena objetos com a tag "enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Define a principio que o inimigo mais proximo eh nulo, deixando a variavel limpa
        GameObject closest = null;

        // Define a distancia minima como infinito, um valor absurdamente alto,
        // então logo na primeira comparação o primeiro inimigo, por ter uma distancia real menor que infinito sera definido como mais proximo
        float minDistance = Mathf.Infinity;

        // Para cada objeto com a tag inimigo, será feita uma checagem, onde o inimigo mais proximo do player sera definido como "closest"
        // e o lobo perseguira o mais proximo
        foreach (GameObject enemy in enemies)
        {
            float distanceToPlayer = Vector2.Distance(player.transform.position, enemy.transform.position);

            if (distanceToPlayer <= detectionRange && distanceToPlayer < minDistance)
            {
                minDistance = distanceToPlayer;
                closest = enemy;
            }

            else
            {
                Debug.Log("Ficarei próximo de ti!");
            }
        }

        return closest;
    }
}
