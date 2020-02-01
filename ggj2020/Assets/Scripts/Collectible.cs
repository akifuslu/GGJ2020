using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
            MessageBroker.Default.Publish(new CollectedEvent());
        }
    }
}


public class CollectedEvent
{

}