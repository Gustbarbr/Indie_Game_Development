using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStore : MonoBehaviour
{
    public PlayerControl player;
    public BatAttack bat;
    public RatControl rat;
    public BullControl bull;
    public WolfAttack wolf;

    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        bat = GetComponent<BatAttack>();
        rat = GetComponent<RatControl>();
        bull = GetComponent<BullControl>();
        wolf = GetComponent<WolfAttack>();
    }

    void Update()
    {
        
    }

    void UpgradeBat()
    {

    }
}
