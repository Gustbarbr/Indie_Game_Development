using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    private float arrowVelocity = 15;
    private float arrowDirection;
    public float lifeTime = 0; // Tempo de vida da flecha

    void Start()
    {
        // Utiliza a velocidade e direção no movimento
        GetComponent<Rigidbody2D>().velocity = new Vector2(arrowDirection * arrowVelocity, 0);
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;

        // Destrói a flecha se ela ficar 3 segundos ou mais no ar
        if(lifeTime >= 3)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetDirection(float dir)
    {
        // Armazena a direção que a flecha será disparada
        arrowDirection = dir;

        // Muda visualmente a direção da flecha
        transform.localScale = new Vector3(arrowDirection, 1, 1);
    }
}
