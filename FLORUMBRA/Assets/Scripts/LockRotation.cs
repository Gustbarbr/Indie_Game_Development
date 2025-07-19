using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{

    public Transform enemy;
    public Vector3 offset = new Vector3(0, 0.2f, 0);

    void LateUpdate()
    {
        if(enemy != null)
            transform.position = enemy.position + offset;
    }
}
