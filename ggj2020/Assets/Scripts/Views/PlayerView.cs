using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{

    public void Bind(Player player)
    {
        GetComponentInChildren<ShardView>().Bind(player);
        GetComponentInChildren<HealthView>().Bind(player);
        GetComponentInChildren<ChaosView>().Bind(player);
    }
}
