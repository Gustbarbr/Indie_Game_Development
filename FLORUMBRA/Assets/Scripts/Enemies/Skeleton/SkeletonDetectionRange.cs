using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDetectionRange : MonoBehaviour
{
    public SkeletonControl skeleton;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            skeleton.playerDetected = true;
        }
    }
}
