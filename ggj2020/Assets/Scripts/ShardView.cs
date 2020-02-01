using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class ShardView : MonoBehaviour
{
    private Text _text;
    private IDisposable _d;

    public void Bind(Level level)
    {
        _text = GetComponent<Text>();
        _d = level.Counter.Subscribe(ev => 
        {
            _text.text = ev + "/" + level.CollectCount;
        });
    }

    private void OnDestroy()
    {
        _d.Dispose();
    }
}
