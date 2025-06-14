using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    private float arrowVelocity = 10;
    private float arrowDirection;

    public void SetDirection(float dir)
    {
        // Armazena a direção que a flecha será disparada
        arrowDirection = dir;

        // Muda visualmente a direção da flecha
        transform.localScale = new Vector3(arrowDirection, 1, 1);
    }

    void Start()
    {
        // Utiliza a velocidade e direção no movimento
        GetComponent<Rigidbody2D>().velocity = new Vector2(arrowDirection * arrowVelocity, 0);
    }
}
