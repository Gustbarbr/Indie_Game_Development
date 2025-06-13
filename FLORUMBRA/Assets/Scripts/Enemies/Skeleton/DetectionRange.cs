using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    public SkeletonControl skeleton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        skeleton.playerDetected = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        skeleton.playerDetected = false;
    }
}
