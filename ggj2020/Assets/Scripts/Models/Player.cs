using UnityEngine;
using UniRx;
using System;
using Assets.Scripts.Utils;

public class Player : MonoBehaviour
{
    public bool AllCollected { get => Counter.Value == CollectCount;}
    public int CollectCount;
    public IntReactiveProperty Health;
    public IntReactiveProperty Counter;
    public IntReactiveProperty Chaos;

    private Vector3 _startPos;
    private Transform _player;
    private CameraController _camera;
    private IDisposable _d, _d1;

    // Start is called before the first frame update
    void Start()
    {
        // init properties 
        Health = new IntReactiveProperty(4);
        Counter = new IntReactiveProperty(0);
        Chaos = new IntReactiveProperty(0);
        
        // bind ui view
        FindObjectOfType<PlayerView>().Bind(this);

        _camera = FindObjectOfType<CameraController>();
        _player = FindObjectOfType<PlayerController>().transform;
        _startPos = _player.transform.position;
        MessageBus.OnEvent<CollectedEvent>().Subscribe(ev => 
        {
            Counter.Value++;
        });

        MessageBus.OnEvent<RepairCrackEvent>().Subscribe(ev =>
        {
            if (Counter.Value != CollectCount)
                return;

            MessageBroker.Default.Publish(new LevelEndedEvent() { Success = true });
            _player.GetComponent<PlayerController>().enabled = false;
            _camera.enabled = false;
            var target = ev.Crack.position;
            target.y = transform.position.y;
            target.z = transform.position.z;
            transform.position = target;
            _d = Observable.EveryUpdate().Subscribe(up =>
            {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 2, Time.deltaTime);
            });
            Observable.Timer(TimeSpan.FromSeconds(.5f)).Subscribe((Action<long>)(ti =>
            {
                ev.Crack.GetComponent<SpriteRenderer>().sprite = ev.Fixed;
                _d.Dispose();
            }));
        });

        MessageBus.OnEvent<PlayerDamagedEvent>().Subscribe(ev =>
        {
            Health.Value--;
            if(Health.Value <= 0)
            {
                MessageBroker.Default.Publish(new LevelEndedEvent() { Success = false});
                _player.GetComponent<PlayerController>().enabled = false;
                _camera.enabled = false;
            }
            else
            {
                _camera.enabled = false;
                _player.GetComponent<PlayerController>().enabled = false;
                _player.GetComponent<Collider2D>().enabled = false;
                Observable.Timer(TimeSpan.FromSeconds(.5f)).Subscribe(ev2 =>
                {
                    _camera.enabled = true;
                    _player.GetComponent<PlayerController>().enabled = true;
                    _player.GetComponent<Collider2D>().enabled = true;
                    _player.position = _startPos;
                    _camera.Reset();
                    _player.GetComponent<PlayerController>().Reset();
                });
            }
        });

        MessageBus.OnEvent<ChaosEvent>().Subscribe(ev =>
        {
            Chaos.Value += ev.Amount;
            if(Chaos.Value >= 100)
            {
                MessageBroker.Default.Publish(new PlayerDamagedEvent());
                Observable.Timer(TimeSpan.FromSeconds(.5f)).Subscribe(ev2 =>
                {
                    Chaos.Value = 0;
                });
            }
        });

        _d1 = Observable.Interval(TimeSpan.FromSeconds(.25f)).Subscribe(ev => { if(Chaos.Value > 0) Chaos.Value--; });
    }

    private void OnDestroy()
    {
        _d1?.Dispose();
    }
}

public class ChaosEvent : GameEvent
{
    public int Amount;
}

public class LevelEndedEvent : GameEvent
{
    public bool Success;
}