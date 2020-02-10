using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class ChaosView : MonoBehaviour
{

    public Image Fill;

    private IDisposable _d;
    private float _tar;

    public void Bind(Player level)
    {
        level.Chaos.Subscribe(ev =>
        {
            _tar = ev / 100f;
        });

        _d = Observable.EveryUpdate().Subscribe(ev =>
        {
            Fill.fillAmount = Mathf.Lerp(Fill.fillAmount, _tar, Time.deltaTime * 2);
        });
    }

    private void OnDestroy()
    {
        _d?.Dispose();
    }
}
