using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class ChaosView : MonoBehaviour
{

    public Image Fill;

    private IDisposable _d;
    private IDisposable _d2;
    private float _tar;

    public void Bind(Level level)
    {
        _d = level.Chaos.Subscribe(ev =>
        {
            _tar = ev / 100f;
        });

        _d2 = Observable.EveryUpdate().Subscribe(ev =>
        {
            Fill.fillAmount = Mathf.Lerp(Fill.fillAmount, _tar, Time.deltaTime * 2);
        });
    }

    private void OnDestroy()
    {
        _d?.Dispose();
        _d2?.Dispose();
    }
}
