using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Crack : MonoBehaviour
{

    public Sprite Repaired;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessageBroker.Default.Publish(new RepairCrackEvent() { Crack = transform, Fixed = Repaired});
        }
    }
}

public class RepairCrackEvent
{
    public Transform Crack;
    public Sprite Fixed;
}
