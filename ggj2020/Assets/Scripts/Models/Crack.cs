using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Crack : MonoBehaviour
{

    public Sprite Repaired;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MessageBus.Publish(new RepairCrackEvent() { Crack = transform, Fixed = Repaired});
        }
    }
}

public class RepairCrackEvent : GameEvent
{
    public Transform Crack;
    public Sprite Fixed;
}
