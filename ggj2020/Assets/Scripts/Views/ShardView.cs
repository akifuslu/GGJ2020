using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class ShardView : MonoBehaviour
{
    private Text _text;

    public void Bind(Player level)
    {
        _text = GetComponent<Text>();
        level.Counter.Subscribe(ev => 
        {
            _text.text = ev + "/" + level.CollectCount;
        });
    }    
}
